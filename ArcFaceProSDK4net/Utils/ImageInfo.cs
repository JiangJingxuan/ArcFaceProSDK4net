using ArcFaceProSDK4net.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net.Utils
{
    public class ImageInfo : IDisposable
    {

        /// <summary>
        /// 最大宽度
        /// </summary>
        private static int maxWidth = 1536;

        /// <summary>
        /// 最大高度
        /// </summary>
        private static int maxHeight = 1536;
        private static void CheckImageWidthAndHeight(ref Bitmap image)
        {
            if (image == null)
            {
                return;
            }
            try
            {
                if (image.Width > maxWidth || image.Height > maxHeight)
                {
                    image = ScaleImage(image, maxWidth, maxHeight);
                }
            }
            catch { }
        }

        private static Bitmap ScaleImage(Bitmap image, int dstWidth, int dstHeight)
        {
            Graphics g = null;
            try
            {
                //按比例缩放           
                float scaleRate = getWidthAndHeight(image.Width, image.Height, dstWidth, dstHeight);
                int width = (int)(image.Width * scaleRate);
                int height = (int)(image.Height * scaleRate);

                //将宽度调整为4的整数倍
                if (width % 4 != 0)
                {
                    width = width - width % 4;
                }

                Bitmap destBitmap = new Bitmap(width, height);
                g = Graphics.FromImage(destBitmap);
                g.Clear(Color.Transparent);

                //设置画布的描绘质量         
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle((width - width) / 2, (height - height) / 2, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

                return destBitmap;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取图片缩放比例
        /// </summary>
        /// <param name="oldWidth">原图片宽</param>
        /// <param name="oldHeigt">原图片高</param>
        /// <param name="newWidth">目标图片宽</param>
        /// <param name="newHeight">目标图片高</param>
        /// <returns></returns>
        private static float getWidthAndHeight(int oldWidth, int oldHeigt, int newWidth, int newHeight)
        {
            //按比例缩放           
            float scaleRate = 0.0f;
            if (oldWidth >= newWidth && oldHeigt >= newHeight)
            {
                int widthDis = oldWidth - newWidth;
                int heightDis = oldHeigt - newHeight;
                if (widthDis > heightDis)
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
                else
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
            }
            else if (oldWidth >= newWidth && oldHeigt < newHeight)
            {
                scaleRate = newWidth * 1f / oldWidth;
            }
            else if (oldWidth < newWidth && oldHeigt >= newHeight)
            {
                scaleRate = newHeight * 1f / oldHeigt;
            }
            else
            {
                int widthDis = newWidth - oldWidth;
                int heightDis = newHeight - oldHeigt;
                if (widthDis > heightDis)
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
                else
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
            }
            return scaleRate;
        }
        /// <summary>
        /// 获取RGB图片信息
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns>图片数据</returns>
        public static ImageInfo ReadBMP(Bitmap bm)
        {
            CheckImageWidthAndHeight(ref bm);
            //调整图像宽度，需要宽度为4的倍数
            if (bm.Width % 4 != 0)
            {
                bm = ScaleImage(bm, bm.Width - (bm.Width % 4), bm.Height);
            }
            ImageInfo imageInfo = new ImageInfo();

            //将Image转换为Format24bppRgb格式的BMP
            BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
                IntPtr ptr = data.Scan0;

                //定义数组长度
                int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                byte[] sourceBitArray = new byte[soureBitArrayLength];

                //将bitmap中的内容拷贝到ptr_bgr数组中
                Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

                //填充引用对象字段值
                imageInfo.width = data.Width;
                imageInfo.height = data.Height;
                imageInfo.format = (int)ASF_ImagePixelFormat.ASVL_PAF_RGB24_B8G8R8;
                //步长的设置
                imageInfo.widthStep = data.Stride;

                imageInfo.imgData = Marshal.AllocHGlobal(sourceBitArray.Length);

                ASVLOFFSCREEN asfImageData = new ASVLOFFSCREEN();
                asfImageData.i32Width = imageInfo.width;
                asfImageData.i32Height = imageInfo.height;
                asfImageData.u32PixelArrayFormat = (uint)imageInfo.format;
                asfImageData.pi32Pitch = new int[4];
                asfImageData.ppu8Plane = new IntPtr[4];
                asfImageData.pi32Pitch[0] = imageInfo.widthStep;
                asfImageData.ppu8Plane[0] = imageInfo.imgData;
                imageInfo.ASFImageData = asfImageData;

                Marshal.Copy(sourceBitArray, 0, imageInfo.imgData, sourceBitArray.Length);

                return imageInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bm.UnlockBits(data);
            }
        }

        /// <summary>
        /// 获取IR图片信息
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns>图片数据</returns>
        public static ImageInfo ReadBMP_IR(Image image)
        {
            ImageInfo imageInfo = new ImageInfo();

            //将Image转换为Format24bppRgb格式的BMP
            Bitmap bm = new Bitmap(image);
            BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
                IntPtr ptr = data.Scan0;

                //定义数组长度
                int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                byte[] sourceBitArray = new byte[soureBitArrayLength];

                //将bitmap中的内容拷贝到ptr_bgr数组中
                Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

                //填充引用对象字段值
                imageInfo.width = data.Width;
                imageInfo.height = data.Height;
                imageInfo.format = (int)ASF_ImagePixelFormat.ASVL_PAF_GRAY;
                //步长的设置
                imageInfo.widthStep = data.Width;

                //获取去除对齐位后度图像数据
                int line = imageInfo.width;
                int pitch = Math.Abs(data.Stride);
                int ir_len = line * imageInfo.height;
                byte[] destBitArray = new byte[ir_len];

                //灰度化
                int j = 0;
                double colortemp = 0;
                for (int i = 0; i < sourceBitArray.Length; i += 3)
                {
                    colortemp = sourceBitArray[i + 2] * 0.299 + sourceBitArray[i + 1] * 0.587 + sourceBitArray[i] * 0.114;
                    destBitArray[j++] = (byte)colortemp;
                }

                imageInfo.imgData = Marshal.AllocHGlobal(destBitArray.Length);
                Marshal.Copy(destBitArray, 0, imageInfo.imgData, destBitArray.Length);

                return imageInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bm.UnlockBits(data);
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(imgData);
        }


        /// <summary>
        /// 图片的像素数据
        /// </summary>
        public IntPtr imgData { get; set; }

        /// <summary>
        /// 图片像素宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 图片像素高
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        public int format { get; set; }

        /// <summary>
        /// 步长
        /// </summary>
        public int widthStep { get; set; }

        public ASVLOFFSCREEN ASFImageData { get; set; }
    }
}
