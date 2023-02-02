using ArcFaceProSDK4net.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net
{
    public partial class ASFFunctions
    {
        /// <summary>
        /// 兼容低版本SDK
        /// </summary>
        public static class Compatible
        {
            /// <summary>
            /// 获取版本信息(2.x)
            /// </summary>
            /// <param name="hEngine">引擎 handle </param>
            /// <returns></returns>
            [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ASFGetVersion(IntPtr hEngine);
            /// <summary>
            /// 初始化引擎。
            /// </summary>
            /// <param name="detectMode">VIDEO模式:处理连续帧的图像数据    IMAGE模式:处理单张的图像数据</param>
            /// <param name="detectFaceOrientPriority">人脸检测角度，推荐单一角度检测</param>
            /// <param name="detectFaceScaleVal">识别的最小人脸比例（图片长边与人脸框长边的比值）  VIDEO模式取值范围[2, 32]，推荐值为16 IMAGE模式取值范围[2, 32]，推荐值为32</param>
            /// <param name="detectFaceMaxNum">最大需要检测的人脸个数，取值范围[1,50]</param>
            /// <param name="combinedMask">需要启用的功能组合，可多选</param>
            /// <param name="hEngine">引擎句柄</param>
            /// <returns></returns>
            [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
            public static extern MResult ASFInitEngine(ASF_DetectMode detectMode, ArcSoftFace_OrientPriority detectFaceOrientPriority, int detectFaceScaleVal, int detectFaceMaxNum, int combinedMask, out IntPtr hEngine);


            /// <summary>
            /// 单人脸特征提取。
            /// </summary>
            /// <param name="hEngine">引擎句柄</param>
            /// <param name="width">图片宽度，为4的倍数</param>
            /// <param name="height">图片高度，YUYV/I420/NV21/NV12格式为2的倍数；BGR24/GRAY/DEPTH_U16格式无限制；</param>
            /// <param name="format">图像的颜色格式</param>
            /// <param name="imgData">图像数据</param>
            /// <param name="faceInfo">单人脸信息（人脸框、人脸角度、人脸数据）</param>
            /// <param name="feature">提取到的人脸特征信息</param>
            /// <param name="threadNum"></param>
            /// <returns></returns>
            [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
            public static extern MResult ASFFaceFeatureExtract(IntPtr hEngine, int width, int height, int format, IntPtr imgData, ASF_SingleFaceInfo faceInfo, out ASF_FaceFeature feature, int threadNum = 1);

            /// <summary>
            /// 单人脸特征提取2.x版本
            /// </summary>
            /// <param name="pEngine">引擎handle</param>
            /// <param name="width">图片宽度为4的倍数且大于0</param>
            /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
            /// <param name="format">颜色空间格式</param>
            /// <param name="pImageData">图片数据</param>
            /// <param name="faceInfo">单张人脸位置和角度信息</param>
            /// <param name="faceFeature">人脸特征</param>
            /// <returns></returns>
            [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFFaceFeatureExtract", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern MResult ASFFaceFeatureExtractV2(IntPtr pEngine, int width, int height, int format, IntPtr pImageData, ref ASF_SingleFaceInfo faceInfo, out ASF_FaceFeature faceFeature);

            /// <summary>
            /// 单人特征提取。
            /// <para>注：该接口与 ASFFaceFeatureExtract 功能一致，但采用结构体的形式传入图像数据，对更高精度的图像兼容性更好。</para>
            /// </summary>
            /// <param name="hEngine">引擎句柄</param>
            /// <param name="imgData">图像数据</param>
            /// <param name="faceInfo">单人脸信息（人脸框、人脸角度）</param>
            /// <param name="feature">提取到的人脸特征信息</param>
            /// <param name="threadNum"></param>
            /// <returns></returns>
            [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
            public static extern MResult ASFFaceFeatureExtractEx(IntPtr hEngine, ASVLOFFSCREEN imgData, ASF_SingleFaceInfo faceInfo, out ASF_FaceFeature feature, int threadNum = 1);
            /// <summary>
            /// 用于免费版在线激活SDK。
            /// </summary>
            /// <param name="AppId">官网获取的APPID</param>
            /// <param name="SDKKey">官网获取的SDKKEY</param>
            /// <returns></returns>
            [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
            public static extern MResult ASFOnlineActivation(string AppId, string SDKKey);
        }
    }
}
