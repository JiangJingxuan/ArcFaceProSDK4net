using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ArcFaceProSDK4net;
using ArcFaceProSDK4net.Models;
using ArcFaceProSDK4net.Utils;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Testcorefx
{
    class Program
    {
        static void Main(string[] args)
        {
            FaceEngine.OfflineActivation();
            var faceengine = new FaceEngine(ASF_RegisterOrNot.ASF_REGISTER, 2);
            //faceengine.OnlineActivation("", "", "");
            //FaceEngine.OnlineActivationFree("", "");
            //faceengine.OfflineActivation()；


            faceengine.InitEngine(ASF_DetectMode.ASF_DETECT_MODE_IMAGE, ArcSoftFace_OrientPriority.ASF_OP_0_ONLY, 1, ASF_Mask.ASF_FACE_DETECT | ASF_Mask.ASF_FACERECOGNITION | ASF_Mask.ASF_AGE | ASF_Mask.ASF_LIVENESS);
            Console.WriteLine(faceengine.Version.BuildDate);
            Console.WriteLine(faceengine.Version.CopyRight);
            Console.WriteLine(faceengine.Version.Version);
            OpenCvSharp.VideoCapture videoCapture = new OpenCvSharp.VideoCapture();
            videoCapture.Open(0);

            var activeFile = FaceEngine.GetActiveFileInfo();


            ////Console.WriteLine(FaceEngine.GetActiveDeviceInfo());
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            //faceengine.InitEngine(ASF_DetectMode.ASF_DETECT_MODE_IMAGE, ArcSoftFace_OrientPriority.ASF_OP_ALL_OUT, 9,
            //    ASF_Mask.ASF_AGE | ASF_Mask.ASF_FACE3DANGLE | ASF_Mask.ASF_FACELANDMARK | ASF_Mask.ASF_FACERECOGNITION | ASF_Mask.ASF_FACESHELTER | ASF_Mask.ASF_FACE_DETECT |
            //     ASF_Mask.ASF_GENDER | ASF_Mask.ASF_IMAGEQUALITY | ASF_Mask.ASF_IR_LIVENESS | ASF_Mask.ASF_LIVENESS | ASF_Mask.ASF_MASKDETECT | ASF_Mask.ASF_UPDATE_FACEDATA);
            //Console.WriteLine($"引擎初始化: {stopwatch.ElapsedMilliseconds}ms");
            Mat mat = new Mat();
            //Mat mat = new Mat(@"C:\Users\Jch\Desktop\2.jpg");
            while (true)
            {
                stopwatch.Restart();

                if (videoCapture.Read(mat))
                    using (var img = mat.ToBitmap())
                    using (var imgInfo = ImageInfo.ReadBMP(img))
                    {
                        Console.WriteLine($"图片处理:{stopwatch.ElapsedMilliseconds}ms");
                        stopwatch.Restart();
                        var detectResult = faceengine.DetectFacesEx(imgInfo);
                        Console.WriteLine($"人脸定位:{stopwatch.ElapsedMilliseconds}ms");
                        if (detectResult != null)
                            foreach (var item in detectResult.FaceInfos)
                            {
                                Console.WriteLine($"Age: {item.Age}");
                                Console.WriteLine($"FaceID: {item.FaceID}");
                                Console.WriteLine($"FaceOrient: {item.FaceOrient}");
                                Console.WriteLine($"FaceShelter: {item.FaceShelter}");
                                Console.WriteLine($"Gender: {item.Gender}");
                                Console.WriteLine($"LeftEyeClosed: {item.LeftEyeClosed}");
                                Console.WriteLine($"Liveness: {item.Liveness}");
                                Console.WriteLine($"Mask: {item.Mask}");
                                Console.WriteLine($"RightEyeClosed: {item.RightEyeClosed}");
                                Console.WriteLine($"WearGlasses: {item.WearGlasses}");
                                Console.WriteLine($"FaceRect: bottom->{item.FaceRect.bottom} left->{item.FaceRect.left} right->{item.FaceRect.right} top->{item.FaceRect.top}");
                                Console.WriteLine($"FaceLandmark: x->{item.FaceLandmark.x} y->{item.FaceLandmark.x}");
                                Console.WriteLine($"Face3DAngle: {item.Face3DAngle.roll} {item.Face3DAngle.yaw} {item.Face3DAngle.pitch} {item.Face3DAngle.status}");
                                stopwatch.Restart();
                                var feature = faceengine.FaceFeatureExtractEx(imgInfo, item);
                                Console.WriteLine($"提取特征值: {stopwatch.ElapsedMilliseconds}ms");
                                if (feature != null)
                                {
                                    Console.WriteLine($"feature: {feature.Size}");
                                }
                                Console.WriteLine(faceengine.FaceFeatureCompare(feature.ASFFaceFeature, feature.ASFFaceFeature));
                                var score = faceengine.ImageQualityDetectEx(imgInfo, item);
                                Console.WriteLine($"人脸质量: {score}");
                                Console.WriteLine("--------------------------------------------");
                            }
                    }
            }

            Console.ReadLine();
        }
    }
}
