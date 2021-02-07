using ArcFaceProSDK4net.Models;
using ArcFaceProSDK4net.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ArcFaceProSDK4net
{
    /// <summary>
    /// 人脸识别引擎
    /// </summary>
    public class FaceEngine : IDisposable
    {
        #region static function


        /// <summary>
        /// 采集当前设备信息（可离线）
        /// </summary>
        /// <returns></returns>
        public static string GetActiveDeviceInfo()
        {
            var result = ASFFunctions.ASFGetActiveDeviceInfo(out IntPtr ptr);
            if (result == MResult.MOK)
                return Marshal.PtrToStringAnsi(ptr);
            throw new Exception($"获取设备指纹信息错误.{result}");
        }

        /// <summary>
        /// 获取激活文件信息
        /// </summary>
        /// <returns></returns>
        public static ActiveFileInfo GetActiveFileInfo()
        {
            var code = ASFFunctions.ASFGetActiveFileInfo(out ASF_ActiveFileInfo activeFileInfo);
            if (code == MResult.MOK)
            {
                return new ActiveFileInfo(activeFileInfo);
            }
            return new ActiveFileInfo();
        }

        /// <summary>
        /// 在线激活
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="SDKKey"></param>
        /// <param name="ActiveKey"></param>
        /// <returns></returns>
        public static bool OnlineActivation(string AppId, string SDKKey, string ActiveKey)
        {
            var result = ASFFunctions.ASFOnlineActivation(AppId, SDKKey, ActiveKey);
            if (result == MResult.MOK || result == MResult.MERR_ASF_ALREADY_ACTIVATED) return true;
            throw new Exception($"在线激活错误.{result}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="SDKKey"></param>
        /// <returns></returns>
        public static bool OnlineActivationFree(string AppId, string SDKKey)
        {
            var result = ASFFunctions.Compatible.ASFOnlineActivation(AppId, SDKKey);
            if (result == MResult.MOK || result == MResult.MERR_ASF_ALREADY_ACTIVATED) return true;
            throw new Exception($"在线激活错误.{result}");
        }

        /// <summary>
        /// 离线激活.将离线激活文件放置到应用程序根目录下并重命名为ArcFacePro.dat
        /// </summary>
        /// <returns></returns>
        public static bool OfflineActivation()
        {
            var activeFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ArcFacePro.dat");
            if (!File.Exists(activeFile)) throw new Exception($"未找到离线激活文件{activeFile}");
            var result = ASFFunctions.ASFOfflineActivation(activeFile);
            if (result == MResult.MOK || result == MResult.MERR_ASF_ALREADY_ACTIVATED) return true;
            throw new Exception($"离线激活错误.{result}");
        }
        /// <summary>
        /// 离线激活.
        /// </summary>
        /// <param name="activeFile">离线激活文件路径</param>
        /// <returns></returns>
        public static bool OfflineActivation(string activeFile)
        {
            if (!File.Exists(activeFile)) throw new Exception($"未找到离线激活文件{activeFile}");
            var result = ASFFunctions.ASFOfflineActivation(activeFile);
            if (result == MResult.MOK || result == MResult.MERR_ASF_ALREADY_ACTIVATED) return true;
            throw new Exception($"离线激活错误.{result}");
        }
        #endregion

        private int _version = -1;

        /// <summary>
        /// 版本
        /// </summary>
        public SDKVersion Version { get; private set; }
        /// <summary>
        /// 引擎句柄
        /// </summary>
        public IntPtr EngineHandler { get; private set; }
        /// <summary>
        /// 当前引擎功能
        /// </summary>
        public ASF_Mask CombinedMask { get; private set; }
        /// <summary>
        /// 提取特征值用途
        /// </summary>
        public ASF_RegisterOrNot RegisterOrNot { get; private set; }
        /// <summary>
        /// 创建一个FaceEngine实例
        /// </summary>
        /// <param name="registerOrNot"></param>
        /// <param name="sdkversion"></param>
        public FaceEngine(ASF_RegisterOrNot registerOrNot, int sdkversion)
        {
            if (sdkversion < 2 || sdkversion > 4) throw new ArgumentOutOfRangeException("sdkversion", sdkversion, "不支持的sdk版本");
            _version = sdkversion;
            GetVersion();
            RegisterOrNot = registerOrNot;
        }

        /// <summary>
        /// 获取sdk版本信息
        /// </summary>
        /// <returns></returns>
        public SDKVersion GetVersion()
        {
            ASF_VERSION version;
            if (_version > 2 || EngineHandler != IntPtr.Zero)
            {
                if (_version > 2)
                    version = ASFFunctions.ASFGetVersion();
                else
                {
                    var r = ASFFunctions.Compatible.ASFGetVersion(EngineHandler);
                    version = Marshal.PtrToStructure<ASF_VERSION>(r);
                }
                Version = new SDKVersion(version);
            }
            return Version;
        }


        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="detectMode">识别模式</param>
        /// <param name="orientPriority">识别角度</param>
        /// <param name="detectFaceMaxNum">最大人脸数[1,10]</param>
        /// <param name="combinedMask">功能组合</param>
        /// <param name="detectFaceScaleVal">识别的最小人脸比例(仅支持2.x及3.x版本)</param>
        public void InitEngine(ASF_DetectMode detectMode, ArcSoftFace_OrientPriority orientPriority, int detectFaceMaxNum, ASF_Mask combinedMask, int detectFaceScaleVal = 16)
        {
            if (_version >= 4 && combinedMask.HasFlag(ASF_Mask.ASF_FACERECOGNITION) && !combinedMask.HasFlag(ASF_Mask.ASF_MASKDETECT))//4.x提取特征值必须要判断是否佩戴口罩特性
                combinedMask |= ASF_Mask.ASF_MASKDETECT;
            CombinedMask = combinedMask;
            IntPtr hEngine = IntPtr.Zero;
            var result =
                _version >= 4 ?
                ASFFunctions.ASFInitEngine(detectMode, orientPriority, detectFaceMaxNum, (int)combinedMask, out hEngine) :
                ASFFunctions.Compatible.ASFInitEngine(detectMode, orientPriority, detectFaceScaleVal, detectFaceMaxNum, (int)combinedMask, out hEngine);
            if (result == MResult.MOK)
            {
                EngineHandler = hEngine;
                if (_version == 2)//2.x需要先初始化引擎后才可以获取到版本信息
                    GetVersion();
                return;
            }
            throw new Exception($"引擎初始化错误.{result}");
        }
        /// <summary>
        /// 检测人脸信息。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="process">执行Process方法提取特性</param>
        /// <param name="detectonly">如果只检测人脸信息,不做特征值提取,可以设置为true则不会调用Process方法</param>
        /// <returns></returns>
        public MultiFaceInfo DetectFaces(ImageInfo image, bool process = false, bool detectonly = false)
        {
            var result =
            ASFFunctions.ASFDetectFaces(EngineHandler, image.width, image.height, image.format, image.imgData, out ASF_MultiFaceInfo multiFaceInfo);
            if (result == MResult.MOK)
            {
                var entity = new MultiFaceInfo(multiFaceInfo);
                if (_version >= 4 || process)//4.x需要用到是否带口罩特性
                {
                    if (!detectonly)
                    {
                        var processcombinMask = CombinedMask & ~ASF_Mask.ASF_FACE_DETECT & ~ASF_Mask.ASF_FACERECOGNITION & ~ASF_Mask.ASF_IMAGEQUALITY & ~ASF_Mask.ASF_IR_LIVENESS & ~ASF_Mask.ASF_FACESHELTER & ~ASF_Mask.ASF_UPDATE_FACEDATA;
                        result = ASFFunctions.ASFProcess(EngineHandler, image.width, image.height, image.format, image.imgData, multiFaceInfo, (int)processcombinMask);
                        if (result == MResult.MOK)
                        {
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_AGE) && ASFFunctions.ASFGetAge(EngineHandler, out ASF_AgeInfo ageInfo) == MResult.MOK)
                                entity.SetAgeInfo(ageInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_GENDER) && ASFFunctions.ASFGetGender(EngineHandler, out ASF_GenderInfo genderInfo) == MResult.MOK)
                                entity.SetGenderInfo(genderInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_FACE3DANGLE) && ASFFunctions.ASFGetFace3DAngle(EngineHandler, out ASF_Face3DAngle face3DAngle) == MResult.MOK)
                                entity.SetFace3DAngle(face3DAngle);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_LIVENESS) && ASFFunctions.ASFGetLivenessScore(EngineHandler, out ASF_LivenessInfo livenessInfo) == MResult.MOK)
                                entity.SetLivenessInfo(livenessInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_MASKDETECT) && ASFFunctions.ASFGetMask(EngineHandler, out ASF_MaskInfo maskInfo) == MResult.MOK)
                                entity.SetMaskInfo(maskInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_FACELANDMARK) && ASFFunctions.ASFGetFaceLandMark(EngineHandler, out ASF_LandMarkInfo faceLandmark) == MResult.MOK)
                                entity.SetFaceLandmark(faceLandmark);
                        }
                    }
                }
                return entity;
            }
            return null;
        }
        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸 3D 角度），最多支持 4 张人脸信息检测，超过部分返回未知（活体仅支持单张人脸检测，超出返回未知）,接口不支持 IR 图像检测。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public bool Process(ImageInfo image, MultiFaceInfo multiFaceInfo)
        {
            var processcombinMask = CombinedMask & ~ASF_Mask.ASF_FACE_DETECT & ~ASF_Mask.ASF_FACERECOGNITION & ~ASF_Mask.ASF_IMAGEQUALITY & ~ASF_Mask.ASF_IR_LIVENESS & ~ASF_Mask.ASF_FACESHELTER & ~ASF_Mask.ASF_UPDATE_FACEDATA;
            var result = ASFFunctions.ASFProcess(EngineHandler, image.width, image.height, image.format, image.imgData, multiFaceInfo.ASFMultiFaceInfo, (int)processcombinMask);
            return result == MResult.MOK;
        }
        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度），最多支持4张人脸信息检测，超过部分返回未知（活体仅支持单张人脸检测，超出返回未知）,接口不支持IR图像检测。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public bool ProcessEx(ImageInfo image, MultiFaceInfo multiFaceInfo)
        {
            if (_version == 2) return Process(image, multiFaceInfo);
            var processcombinMask = CombinedMask & ~ASF_Mask.ASF_FACE_DETECT & ~ASF_Mask.ASF_FACERECOGNITION & ~ASF_Mask.ASF_IMAGEQUALITY & ~ASF_Mask.ASF_IR_LIVENESS & ~ASF_Mask.ASF_FACESHELTER & ~ASF_Mask.ASF_UPDATE_FACEDATA;
            var result = ASFFunctions.ASFProcessEx(EngineHandler, image.ASFImageData, multiFaceInfo.ASFMultiFaceInfo, (int)processcombinMask);
            return result == MResult.MOK;
        }
        /// <summary>
        /// 获取活体信息
        /// </summary>
        /// <returns></returns>
        public int[] GetLivenessScore()
        {
            var result = ASFFunctions.ASFGetLivenessScore(EngineHandler, out ASF_LivenessInfo livenessInfo);
            if (result == MResult.MOK)
            {
                var _liveness = new int[livenessInfo.num];
                Marshal.Copy(livenessInfo.isLive, _liveness, 0, livenessInfo.num);
                return _liveness;
            }
            return null;
        }
        /// <summary>
        /// 获取年龄信息
        /// </summary>
        /// <returns></returns>
        public int[] GetAge()
        {
            var result = ASFFunctions.ASFGetAge(EngineHandler, out ASF_AgeInfo ageInfo);
            if (result == MResult.MOK)
            {
                var _copy = new int[ageInfo.num];
                Marshal.Copy(ageInfo.ageArray, _copy, 0, ageInfo.num);
                return _copy;
            }
            return null;
        }
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns></returns>
        public int[] GetGender()
        {
            var result = ASFFunctions.ASFGetGender(EngineHandler, out ASF_GenderInfo genderInfo);
            if (result == MResult.MOK)
            {
                var _copy = new int[genderInfo.num];
                Marshal.Copy(genderInfo.genderArray, _copy, 0, genderInfo.num);
                return _copy;
            }
            return null;
        }
        /// <summary>
        /// 获取3D角度信息。
        /// </summary>
        /// <returns></returns>
        public List<Face3DAngle> GetFace3DAngle()
        {
            var result = ASFFunctions.ASFGetFace3DAngle(EngineHandler, out ASF_Face3DAngle face3DAngle);
            if (result == MResult.MOK)
            {
                var Face3DAngles = new List<Face3DAngle>();
                float[] _roll = new float[face3DAngle.num];
                float[] _yaw = new float[face3DAngle.num];
                float[] _pitch = new float[face3DAngle.num];
                int[] _status = new int[face3DAngle.num];
                Marshal.Copy(face3DAngle.roll, _roll, 0, face3DAngle.num);
                Marshal.Copy(face3DAngle.yaw, _yaw, 0, face3DAngle.num);
                Marshal.Copy(face3DAngle.pitch, _pitch, 0, face3DAngle.num);
                Marshal.Copy(face3DAngle.status, _status, 0, face3DAngle.num);
                for (int i = 0; i < face3DAngle.num; i++)
                {
                    var angle = new Face3DAngle
                    {
                        pitch = _pitch[i],
                        roll = _roll[i],
                        status = _status[i],
                        yaw = _yaw[i]
                    };
                    Face3DAngles.Add(angle);
                }
                return Face3DAngles;
            }
            return null;
        }
        /// <summary>
        /// 获取人脸是否戴口罩。
        /// <para>仅支持4.0+</para>
        /// </summary>
        /// <returns></returns>
        public int[] GetMask()
        {
            if (_version >= 4)
            {
                var result = ASFFunctions.ASFGetMask(EngineHandler, out ASF_MaskInfo mask);
                if (result == MResult.MOK)
                {
                    var _copy = new int[mask.num];
                    Marshal.Copy(mask.maskArray, _copy, 0, mask.num);
                    return _copy;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取额头区域位置。
        /// <para>仅支持4.0+</para>
        /// </summary>
        /// <returns></returns>
        public List<ASF_FaceLandmark> GetFaceLandMark()
        {
            if (_version >= 4)
            {
                var result = ASFFunctions.ASFGetFaceLandMark(EngineHandler, out ASF_LandMarkInfo landMarkInfo);
                if (result == MResult.MOK)
                {
                    var faceLandmarks = new List<ASF_FaceLandmark>();
                    var size = Marshal.SizeOf<ASF_FaceLandmark>();
                    for (int i = 0; i < landMarkInfo.num; i++)
                    {
                        var obj = Marshal.PtrToStructure<ASF_FaceLandmark>(IntPtr.Add(landMarkInfo.point, size * i));
                        faceLandmarks.Add(obj);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 检测人脸信息。
        /// <para>注：该接口与 DetectFaces 功能一致，但采用结构体的形式传入图像数据，对更高精度的图像兼容性更好。</para>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="process">执行Process方法提取特性</param>
        /// <param name="detectonly">如果只检测人脸信息,不做特征值提取,可以设置为true则不会调用Process方法</param>
        /// <returns></returns>
        public MultiFaceInfo DetectFacesEx(ImageInfo image, bool process = false, bool detectonly = false)
        {
            if (_version == 2) return DetectFaces(image);
            var result =
            ASFFunctions.ASFDetectFacesEx(EngineHandler, image.ASFImageData, out ASF_MultiFaceInfo multiFaceInfo);
            if (result == MResult.MOK)
            {
                var entity = new MultiFaceInfo(multiFaceInfo);
                if (_version >= 4 || process)//4.x需要用到是否带口罩特性
                {
                    if (!detectonly)
                    {
                        var processcombinMask = CombinedMask & ~ASF_Mask.ASF_FACE_DETECT & ~ASF_Mask.ASF_FACERECOGNITION & ~ASF_Mask.ASF_IMAGEQUALITY & ~ASF_Mask.ASF_IR_LIVENESS & ~ASF_Mask.ASF_FACESHELTER & ~ASF_Mask.ASF_UPDATE_FACEDATA;
                        result = ASFFunctions.ASFProcessEx(EngineHandler, image.ASFImageData, multiFaceInfo, (int)processcombinMask);
                        if (result == MResult.MOK)
                        {
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_AGE) && ASFFunctions.ASFGetAge(EngineHandler, out ASF_AgeInfo ageInfo) == MResult.MOK)
                                entity.SetAgeInfo(ageInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_GENDER) && ASFFunctions.ASFGetGender(EngineHandler, out ASF_GenderInfo genderInfo) == MResult.MOK)
                                entity.SetGenderInfo(genderInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_FACE3DANGLE) && ASFFunctions.ASFGetFace3DAngle(EngineHandler, out ASF_Face3DAngle face3DAngle) == MResult.MOK)
                                entity.SetFace3DAngle(face3DAngle);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_LIVENESS) && ASFFunctions.ASFGetLivenessScore(EngineHandler, out ASF_LivenessInfo livenessInfo) == MResult.MOK)
                                entity.SetLivenessInfo(livenessInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_MASKDETECT) && ASFFunctions.ASFGetMask(EngineHandler, out ASF_MaskInfo maskInfo) == MResult.MOK)
                                entity.SetMaskInfo(maskInfo);
                            if (processcombinMask.HasFlag(ASF_Mask.ASF_FACELANDMARK) && ASFFunctions.ASFGetFaceLandMark(EngineHandler, out ASF_LandMarkInfo faceLandmark) == MResult.MOK)
                                entity.SetFaceLandmark(faceLandmark);
                        }
                    }
                }
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 提取特征值
        /// <para>使用当前引擎初始化时设置的RegisterOrNot</para>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <returns></returns>
        public FaceFeaturePro FaceFeatureExtract(ImageInfo image, SingleFaceInfo singleFaceInfo)
        {
            return FaceFeatureExtract(image, singleFaceInfo, RegisterOrNot);
        }
        /// <summary>
        /// 提取特征值
        /// <para>使用当前引擎初始化时设置的RegisterOrNot</para>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <returns></returns>
        public FaceFeaturePro FaceFeatureExtractEx(ImageInfo image, SingleFaceInfo singleFaceInfo)
        {
            return FaceFeatureExtractEx(image, singleFaceInfo, RegisterOrNot);
        }
        /// <summary>
        /// 提取特征值
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <param name="registerOrNot"></param>
        /// <returns></returns>
        public FaceFeaturePro FaceFeatureExtract(ImageInfo image, SingleFaceInfo singleFaceInfo, ASF_RegisterOrNot registerOrNot)
        {
            MResult result = MResult.MOK;
            ASF_FaceFeature feature = new ASF_FaceFeature { };
            if (_version >= 4)
                result = ASFFunctions.ASFFaceFeatureExtract(EngineHandler, image.width, image.height, image.format, image.imgData, singleFaceInfo.ASFSingleFaceInfo, registerOrNot, singleFaceInfo.Mask ? 1 : 0, out feature);
            else
                result = ASFFunctions.Compatible.ASFFaceFeatureExtract(EngineHandler, image.width, image.height, image.format, image.imgData, singleFaceInfo.ASFSingleFaceInfo, out feature);
            if (result == MResult.MOK)
            {
                var entity = new FaceFeaturePro();
                entity.Size = feature.featureSize;
                var buffers = new byte[feature.featureSize];
                Marshal.Copy(feature.feature, buffers, 0, feature.featureSize);
                entity.Buffers = buffers;
                entity.ASFFaceFeature = feature;
                return entity;
            }
            return null;
        }
        /// <summary>
        /// 提取特征值
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <param name="registerOrNot"></param>
        /// <returns></returns>
        public FaceFeaturePro FaceFeatureExtractEx(ImageInfo image, SingleFaceInfo singleFaceInfo, ASF_RegisterOrNot registerOrNot)
        {
            if (_version == 2)
                return FaceFeatureExtract(image, singleFaceInfo);
            MResult result = MResult.MOK;
            ASF_FaceFeature feature = new ASF_FaceFeature { };
            if (_version >= 4)
                result =
                    ASFFunctions.ASFFaceFeatureExtractEx(EngineHandler, image.ASFImageData, singleFaceInfo.ASFSingleFaceInfo, registerOrNot, singleFaceInfo.Mask ? 1 : 0, out feature);
            else
                result =
                    ASFFunctions.Compatible.ASFFaceFeatureExtractEx(EngineHandler, image.ASFImageData, singleFaceInfo.ASFSingleFaceInfo, out feature);
            if (result == MResult.MOK)
            {
                var entity = new FaceFeaturePro();
                entity.Size = feature.featureSize;
                var buffers = new byte[feature.featureSize];
                Marshal.Copy(feature.feature, buffers, 0, feature.featureSize);
                entity.Buffers = buffers;
                entity.ASFFaceFeature = feature;
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 更新人脸信息。
        /// <para>该接口主要用于在需要修改人脸框时候更新人脸数据，用于之后的算法检测。一般常用与双目摄像头对齐，对齐之后的人脸框传入该接口更新人脸数据用于之后的红外活体检测。</para>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public MultiFaceInfo UpdateFaceData(ImageInfo image, MultiFaceInfo multiFaceInfo)
        {
            if (_version >= 4)//4.x支持方法
            {
                var asfmultifaceinfo = multiFaceInfo.ASFMultiFaceInfo;
                var result = ASFFunctions.ASFUpdateFaceData(EngineHandler, image.width, image.height, image.format, image.imgData, out asfmultifaceinfo);
                if (result == MResult.MOK)
                {
                    multiFaceInfo.UpdateMultiFaceInfo(asfmultifaceinfo);
                }
            }
            return multiFaceInfo;
        }
        /// <summary>
        /// 更新人脸信息。
        /// <para>该接口主要用于在需要修改人脸框时候更新人脸数据，用于之后的算法检测。一般常用与双目摄像头对齐，对齐之后的人脸框传入该接口更新人脸数据用于之后的红外活体检测。</para>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public MultiFaceInfo UpdateFaceDataEx(ImageInfo image, MultiFaceInfo multiFaceInfo)
        {
            if (_version >= 4)//4.x支持方法
            {
                var asfmultifaceinfo = multiFaceInfo.ASFMultiFaceInfo;
                var result = ASFFunctions.ASFUpdateFaceDataEx(EngineHandler, image.ASFImageData, out asfmultifaceinfo);
                if (result == MResult.MOK)
                {
                    multiFaceInfo.UpdateMultiFaceInfo(asfmultifaceinfo);
                }
            }
            return multiFaceInfo;
        }

        /// <summary>
        /// 支持单人脸图像质量检测。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <returns></returns>
        public float ImageQualityDetect(ImageInfo image, SingleFaceInfo singleFaceInfo)
        {
            if (_version < 4) return 1;//2.x不支持,3.x结构差异大,不做兼容
            var result = ASFFunctions.ASFImageQualityDetect(EngineHandler, image.width, image.height, image.format, image.imgData, singleFaceInfo.ASFSingleFaceInfo, singleFaceInfo.Mask ? 1 : 1, out float confidenceLevel);
            if (result == MResult.MOK)
            {
                return confidenceLevel;
            }
            return 0;
        }

        /// <summary>
        /// 支持单人脸图像质量检测。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="singleFaceInfo"></param>
        /// <returns></returns>
        public float ImageQualityDetectEx(ImageInfo image, SingleFaceInfo singleFaceInfo)
        {
            if (_version < 4) return 1;//2.x不支持,3.x结构差异大,不做兼容
            var result = ASFFunctions.ASFImageQualityDetectEx(EngineHandler, image.ASFImageData, singleFaceInfo.ASFSingleFaceInfo, singleFaceInfo.Mask ? 1 : 1, out float confidenceLevel);
            if (result == MResult.MOK)
            {
                return confidenceLevel;
            }
            return 0;
        }
        /// <summary>
        /// 人脸特征比对，输出比对相似度。
        /// </summary>
        /// <param name="feature1"></param>
        /// <param name="feature2"></param>
        /// <returns></returns>
        public float FaceFeatureCompare(byte[] feature1, byte[] feature2)
        {
            var f1 = new ASF_FaceFeature
            {
                featureSize = feature1.Length
            };
            var f2 = new ASF_FaceFeature
            {
                featureSize = feature2.Length
            };
            var p1 = Marshal.AllocHGlobal(feature1.Length);
            var p2 = Marshal.AllocHGlobal(feature2.Length);
            Marshal.Copy(feature1, 0, p1, feature1.Length);
            Marshal.Copy(feature2, 0, p2, feature2.Length);
            f1.feature = p1;
            f2.feature = p2;
            ASFFunctions.ASFFaceFeatureCompare(EngineHandler, f1, f2, out float score, ASF_CompareModel.ASF_LIFE_PHOTO);
            Marshal.FreeHGlobal(p1);
            Marshal.FreeHGlobal(p2);
            return score;
        }
        /// <summary>
        /// 人脸特征比对，输出比对相似度。
        /// </summary>
        /// <param name="feature1"></param>
        /// <param name="feature2"></param>
        /// <param name="compareModel"></param>
        /// <returns></returns>
        public float FaceFeatureCompare(ASF_FaceFeature feature1, ASF_FaceFeature feature2, ASF_CompareModel compareModel = ASF_CompareModel.ASF_LIFE_PHOTO)
        {
            ASFFunctions.ASFFaceFeatureCompare(EngineHandler, feature1, feature2, out float score, compareModel);
            return score;
        }
        /// <summary>
        /// 人脸特征比对，输出比对相似度。
        /// </summary>
        /// <param name="feature1"></param>
        /// <param name="feature2"></param>
        /// <returns></returns>
        public float FaceFeatureCompare(ASF_FaceFeature feature1, byte[] feature2)
        {
            var f2 = new ASF_FaceFeature
            {
                featureSize = feature2.Length
            };
            var p2 = Marshal.AllocHGlobal(feature2.Length);
            f2.feature = p2;
            ASFFunctions.ASFFaceFeatureCompare(EngineHandler, feature1, f2, out float score, ASF_CompareModel.ASF_LIFE_PHOTO);
            return score;
        }

        /// <summary>
        /// 设置RGB/IR活体阈值，若不设置内部默认RGB：0.5 IR：0.7。
        /// </summary>
        /// <param name="thresholdmodel_BGR"></param>
        /// <param name="thresholdmodel_IR"></param>
        /// <returns></returns>
        public bool SetLivenessParam(float thresholdmodel_BGR, float thresholdmodel_IR)
        {
            return ASFFunctions.ASFSetLivenessParam(EngineHandler, new ASF_LivenessThreshold
            {
                thresholdmodel_BGR = thresholdmodel_BGR,
                thresholdmodel_IR = thresholdmodel_IR
            }) == MResult.MOK;
        }
        /// <summary>
        /// 设置遮挡算法检测的阈值。
        /// </summary>
        /// <param name="ShelterThreshhold"></param>
        /// <returns></returns>
        public bool SetFaceShelterParam(float ShelterThreshhold)
        {
            return ASFFunctions.ASFSetFaceShelterParam(EngineHandler, ShelterThreshhold) == MResult.MOK;
        }
        /// <summary>
        /// 该接口仅支持单人脸 IR 活体检测，超出返回未知。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public bool Process_IR(ImageInfo image, ref MultiFaceInfo multiFaceInfo)
        {
            var asfinfo = multiFaceInfo.ASFMultiFaceInfo;
            var result = ASFFunctions.ASFProcess_IR(EngineHandler, image.width, image.height, image.format, image.imgData, asfinfo);
            if (result == MResult.MOK)
            {
                multiFaceInfo.UpdateMultiFaceInfo(asfinfo);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 该接口仅支持单人脸 IR 活体检测，超出返回未知。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="multiFaceInfo"></param>
        /// <returns></returns>
        public bool ProcessEx_IR(ImageInfo image, ref MultiFaceInfo multiFaceInfo)
        {
            var asfinfo = multiFaceInfo.ASFMultiFaceInfo;
            var result = ASFFunctions.ASFProcessEx_IR(EngineHandler, image.ASFImageData, asfinfo);
            if (result == MResult.MOK)
            {
                multiFaceInfo.UpdateMultiFaceInfo(asfinfo);
                var liveness = GetLivenessScore_IR();
                multiFaceInfo.SetLivenessInfo(liveness);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取IR活体信息。
        /// </summary>
        /// <returns></returns>
        public ASF_LivenessInfo GetLivenessScore_IR()
        {
            ASFFunctions.ASFGetLivenessScore_IR(EngineHandler, out ASF_LivenessInfo livenessInfo);
            return livenessInfo;
        }
        /// <summary>
        /// 释放引擎
        /// </summary>
        public void Dispose()
        {
            ASFFunctions.ASFUninitEngine(EngineHandler);
        }
    }
}
