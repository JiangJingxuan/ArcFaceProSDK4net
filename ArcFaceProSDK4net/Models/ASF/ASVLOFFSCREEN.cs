using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ASVLOFFSCREEN
    {
        /// <summary>
        /// 图片格式
        /// </summary>
        public uint u32PixelArrayFormat;

        /// <summary>
        /// 宽
        /// </summary>
        public int i32Width;

        /// <summary>
        /// 高
        /// </summary>
        public int i32Height;

        /// <summary>
        /// 图像数据
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public IntPtr[] ppu8Plane;

        /// <summary>
        /// 步长
        /// </summary>       
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I4)]
        public int[] pi32Pitch;
    }
}
