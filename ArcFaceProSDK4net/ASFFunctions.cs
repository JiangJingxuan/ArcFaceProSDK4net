using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ArcFaceProSDK4net.Models;

namespace ArcFaceProSDK4net
{
    /// <summary>
    /// sdk import
    /// </summary>
    public partial class ASFFunctions
    {
        /// <summary>
        /// dll path
        /// </summary>
        public const string Dll_PATH = "libarcsoft_face_engine.dll";

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern ASF_VERSION ASFGetVersion();
        /// <summary>
        /// 获取激活文件信息。
        /// </summary>
        /// <param name="activeFileInfo">激活文件信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetActiveFileInfo(out ASF_ActiveFileInfo activeFileInfo);

        /// <summary>
        /// 用于在线激活SDK。
        /// </summary>
        /// <param name="AppId">官网获取的APPID</param>
        /// <param name="SDKKey">官网获取的SDKKEY</param>
        /// <param name="ActiveKey">官网获取的ACTIVEKEY</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFOnlineActivation(string AppId, string SDKKey, string ActiveKey);
        /// <summary>
        /// 采集当前设备信息（可离线）。
        /// </summary>
        /// <param name="deviceInfo">采集的设备信息，用于上传到后台生成离线授权文件</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetActiveDeviceInfo(out IntPtr deviceInfo);
        /// <summary>
        /// 用于离线激活SDK。
        /// </summary>
        /// <param name="filePath">许可文件路径(虹软开放平台开发者中心端获取的文件)</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFOfflineActivation(string filePath);
        /// <summary>
        /// 初始化引擎。
        /// </summary>
        /// <param name="detectMode">VIDEO模式:处理连续帧的图像数据        IMAGE模式:处理单张的图像数据</param>
        /// <param name="detectFaceOrientPriority">人脸检测角度，推荐单一角度检测</param>
        /// <param name="detectFaceMaxNum">最大需要检测的人脸个数，取值范围[1,10]</param>
        /// <param name="combinedMask">需要启用的功能组合，可多选</param>
        /// <param name="hEngine">引擎句柄</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFInitEngine(ASF_DetectMode detectMode, ArcSoftFace_OrientPriority detectFaceOrientPriority, int detectFaceMaxNum, int combinedMask, out IntPtr hEngine);
        /// <summary>
        /// 检测人脸信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度，为4的倍数</param>
        /// <param name="height">图片高度，YUYV/I420/NV21/NV12格式为2的倍数；BGR24/GRAY/DEPTH_U16格式无限制；</param>
        /// <param name="format">图像的颜色格式</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="detectModel">预留字段，当前版本使用默认参数即可</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFDetectFaces(IntPtr hEngine, int width, int height, int format, IntPtr imgData, out ASF_MultiFaceInfo detectedFaces, ASF_DetectModel detectModel = ASF_DetectModel.ASF_DETECT_MODEL_RGB);
        /// <summary>
        /// 检测人脸信息。
        /// <para>注：该接口与 ASFDetectFaces 功能一致，但采用结构体的形式传入图像数据，对更高精度的图像兼容性更好。</para>
        /// </summary>
        /// <param name="hEngine"></param>
        /// <param name="imgData"></param>
        /// <param name="detectedFaces"></param>
        /// <param name="detectModel"></param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFDetectFacesEx(IntPtr hEngine, ASVLOFFSCREEN imgData, out ASF_MultiFaceInfo detectedFaces, ASF_DetectModel detectModel = ASF_DetectModel.ASF_DETECT_MODEL_RGB);
        /// <summary>
        /// 更新人脸信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度，为4的倍数</param>
        /// <param name="height">图片高度，YUYV/I420/NV21/NV12格式为2的倍数；BGR24/GRAY/DEPTH_U16格式无限制；</param>
        /// <param name="format">图像的颜色格式</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">更新后的人脸信息（既为入参也为出参，该接口主要是更新faceDataInfoList值）</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFUpdateFaceData(IntPtr hEngine, int width, int height, int format, IntPtr imgData, out ASF_MultiFaceInfo detectedFaces);
        /// <summary>
        /// 更新人脸信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">更新后的人脸信息（既为入参也为出参，该接口主要是更新faceDataInfoList值）</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFUpdateFaceDataEx(IntPtr hEngine, ASVLOFFSCREEN imgData, out ASF_MultiFaceInfo detectedFaces);
        /// <summary>
        /// 支持单人脸图像质量检测。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="imgData">图片数据</param>
        /// <param name="faceInfo">人脸位置信息</param>
        /// <param name="isMask">仅支持传入1、0、-1，戴口罩 1，否则认为未戴口罩</param>
        /// <param name="confidenceLevel">人脸图像质量检测结果</param>
        /// <param name="detectModel">预留字段，当前版本使用默认参数即可</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFImageQualityDetect(IntPtr hEngine, int width, int height, int format, IntPtr imgData, ASF_SingleFaceInfo faceInfo, int isMask, out float confidenceLevel, ASF_DetectModel detectModel = ASF_DetectModel.ASF_DETECT_MODEL_RGB);

        /// <summary>
        /// 支持单人脸图像质量检测。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="faceInfo">人脸位置信息</param>
        /// <param name="isMask">仅支持传入1、0、-1，戴口罩 1，否则认为未戴口罩</param>
        /// <param name="confidenceLevel">人脸图像质量检测结果</param>
        /// <param name="detectModel">预留字段，当前版本使用默认参数即可</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFImageQualityDetectEx(IntPtr hEngine, ASVLOFFSCREEN imgData, ASF_SingleFaceInfo faceInfo, int isMask, out float confidenceLevel, ASF_DetectModel detectModel = ASF_DetectModel.ASF_DETECT_MODEL_RGB);


        /// <summary>
        /// 单人脸特征提取。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度，为4的倍数</param>
        /// <param name="height">图片高度，YUYV/I420/NV21/NV12格式为2的倍数；BGR24/GRAY/DEPTH_U16格式无限制；</param>
        /// <param name="format">图像的颜色格式</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="faceInfo">单人脸信息（人脸框、人脸角度、人脸数据）</param>
        /// <param name="registerOrNot">注册照：ASF_REGISTER  识别照：ASF_RECOGNITION</param>
        /// <param name="mask">带口罩 1，否则0</param>
        /// <param name="feature">提取到的人脸特征信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFFaceFeatureExtract(IntPtr hEngine, int width, int height, int format, IntPtr imgData, ASF_SingleFaceInfo faceInfo, ASF_RegisterOrNot registerOrNot, int mask, out ASF_FaceFeature feature);
        /// <summary>
        /// 单人特征提取。
        /// <para>注：该接口与 ASFFaceFeatureExtract 功能一致，但采用结构体的形式传入图像数据，对更高精度的图像兼容性更好。</para>
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="faceInfo">单人脸信息（人脸框、人脸角度）</param>
        /// <param name="registerOrNot">注册照：ASF_REGISTER        识别照：ASF_RECOGNITION</param>
        /// <param name="mask">带口罩 1，否则0</param>
        /// <param name="feature">提取到的人脸特征信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFFaceFeatureExtractEx(IntPtr hEngine, ASVLOFFSCREEN imgData, ASF_SingleFaceInfo faceInfo, ASF_RegisterOrNot registerOrNot, int mask, out ASF_FaceFeature feature);

        /// <summary>
        /// 人脸属性检测（年龄/性别/3D角度/口罩/额头区域），最多支持4张人脸信息检测，超过部分返回未知（活体仅支持单张人脸检测，超出返回未知）,接口不支持IR图像检测。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度，为4的倍数</param>
        /// <param name="height">图片高度，YUYV/I420/NV21/NV12格式为2的倍数；BGR24格式无限制；</param>
        /// <param name="format">支持YUYV/I420/NV21/NV12/BGR24</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">多人脸信息</param>
        /// <param name="combinedMask">1.检测的属性（ASF_AGE、ASF_GENDER、 ASF_FACE3DANGLE、ASF_LIVENESS、ASF_FACELANDMARK、ASF_MASKDETECT）支持多选;2.检测的属性须在引擎初始化接口的combinedMask参数中启用</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFProcess(IntPtr hEngine, int width, int height, int format, IntPtr imgData, ASF_MultiFaceInfo detectedFaces, int combinedMask);
        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度），最多支持4张人脸信息检测，超过部分返回未知（活体仅支持单张人脸检测，超出返回未知）,接口不支持IR图像检测。
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图片宽度为4的倍数且大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="combinedMask">初始化中参数combinedMask与ASF_AGE| ASF_GENDER| ASF_FACE3DANGLE的交集的子集</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern MResult ASFProcessV2(IntPtr pEngine, int width, int height, int format, IntPtr pImageData, ref ASF_MultiFaceInfo detectedFaces, uint combinedMask);
        /// <summary>
        /// 人脸属性检测（年龄/性别/人脸3D角度/口罩/额头区域），最多支持4张人脸信息检测，超过部分返回未知（活体仅支持单张人脸检测，超出返回未知）,接口不支持IR图像检测。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">多人脸信息</param>
        /// <param name="combinedMask">1.检测的属性（ASF_AGE、ASF_GENDER、 ASF_FACE3DANGLE、ASF_LIVENESS、ASF_FACELANDMARK、ASF_MASKDETECT）支持多选2.检测的属性须在引擎初始化接口的combinedMask参数中启用</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFProcessEx(IntPtr hEngine, ASVLOFFSCREEN imgData, ASF_MultiFaceInfo detectedFaces, int combinedMask);
        /// <summary>
        /// 获取年龄信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="ageInfo">检测到的年龄信息数组</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetAge(IntPtr hEngine, out ASF_AgeInfo ageInfo);
        /// <summary>
        /// 获取性别信息。
        /// </summary>
        /// <param name="hEngine"></param>
        /// <param name="genderInfo"></param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetGender(IntPtr hEngine, out ASF_GenderInfo genderInfo);

        /// <summary>
        /// 获取3D角度信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="face3DAngle">检测到的3D角度信息数组</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetFace3DAngle(IntPtr hEngine, out ASF_Face3DAngle face3DAngle);
        /// <summary>
        /// 获取RGB活体信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="livenessInfo">检测到的活体信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetLivenessScore(IntPtr hEngine, out ASF_LivenessInfo livenessInfo);

        /// <summary>
        /// 设置遮挡算法检测的阈值。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="ShelterThreshhold">设置遮挡算法检测的阈值</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFSetFaceShelterParam(IntPtr hEngine, float ShelterThreshhold);
        /// <summary>
        /// 获取人脸是否戴口罩。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="maskInfo">检测到人脸是否戴口罩的信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetMask(IntPtr hEngine, out ASF_MaskInfo maskInfo);
        /// <summary>
        /// 获取额头区域位置。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="LandMarkInfo">人脸额头点数组，每张人脸额头区域通过四个点表示</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetFaceLandMark(IntPtr hEngine, out ASF_LandMarkInfo LandMarkInfo);
        /// <summary>
        /// 该接口仅支持单人脸 IR 活体检测，超出返回未知。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="width">图片宽度，为4的倍数</param>
        /// <param name="height">图片高度</param>
        /// <param name="format">图像颜色格式</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">多人脸信息</param>
        /// <param name="combinedMask">目前仅支持 ASF_IR_LIVENESS</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFProcess_IR(IntPtr hEngine, int width, int height, int format, IntPtr imgData, ASF_MultiFaceInfo detectedFaces, ASF_Mask combinedMask = ASF_Mask.ASF_IR_LIVENESS);
        /// <summary>
        /// 该接口仅支持单人脸 IR 活体检测，超出返回未知。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">多人脸信息</param>
        /// <param name="combinedMask">目前仅支持 ASF_IR_LIVENESS</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFProcessEx_IR(IntPtr hEngine, ASVLOFFSCREEN imgData, ASF_MultiFaceInfo detectedFaces, ASF_Mask combinedMask = ASF_Mask.ASF_IR_LIVENESS);
        /// <summary>
        /// 获取IR活体信息。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="livenessInfo">检测到的IR活体信息</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFGetLivenessScore_IR(IntPtr hEngine, out ASF_LivenessInfo livenessInfo);

        /// <summary>
        /// 人脸特征比对，输出比对相似度。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <param name="faceFeature1">人脸特征</param>
        /// <param name="faceFeature2">人脸特征</param>
        /// <param name="confidenceLevel">比对相似度</param>
        /// <param name="compareModel">选择人脸特征比对模型，默认为ASF_LIFE_PHOTO。1. ASF_LIFE_PHOTO：用于生活照之间的特征比对，推荐阈值0.80；2. ASF_ID_PHOTO：用于证件照或证件照和生活照之间的特征比对，推荐阈值0.82；</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFFaceFeatureCompare(IntPtr hEngine, ASF_FaceFeature faceFeature1, ASF_FaceFeature faceFeature2, out float confidenceLevel, ASF_CompareModel compareModel = ASF_CompareModel.ASF_LIFE_PHOTO);

        /// <summary>
        /// 设置RGB/IR活体阈值，若不设置内部默认RGB：0.5 IR：0.7。
        /// </summary>
        /// <param name="hEngine"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFSetLivenessParam(IntPtr hEngine, ASF_LivenessThreshold threshold);





        /// <summary>
        /// 销毁SDK引擎。
        /// </summary>
        /// <param name="hEngine">引擎句柄</param>
        /// <returns></returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern MResult ASFUninitEngine(IntPtr hEngine);
    }
}
