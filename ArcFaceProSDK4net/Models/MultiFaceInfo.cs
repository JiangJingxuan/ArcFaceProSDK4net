using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public class MultiFaceInfo
    {
        public ASF_MultiFaceInfo ASFMultiFaceInfo { get; set; }
        public MultiFaceInfo()
        {

        }
        public MultiFaceInfo(ASF_MultiFaceInfo multiFaceInfo)
        {
            UpdateMultiFaceInfo(multiFaceInfo);
        }

        public void UpdateMultiFaceInfo(ASF_MultiFaceInfo multiFaceInfo)
        {
            ASFMultiFaceInfo = multiFaceInfo;
            faceNum = multiFaceInfo.faceNum;
            if (faceNum == 0) return;
            var mrectSize = Marshal.SizeOf<MRECT>();
            var facedatainfoSize = Marshal.SizeOf<ASF_FaceDataInfo>();

            var _faceOrients = new int[faceNum];
            if (multiFaceInfo.faceOrient != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.faceOrient, _faceOrients, 0, faceNum);

            var _faceId = new int[faceNum];
            if (multiFaceInfo.faceID != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.faceID, _faceId, 0, faceNum);

            var _wearGlasses = new float[faceNum];
            if (multiFaceInfo.wearGlasses != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.wearGlasses, _wearGlasses, 0, faceNum);

            var _faceShelter = new int[faceNum];
            if (multiFaceInfo.faceShelter != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.faceShelter, _faceShelter, 0, faceNum);

            var _leftEyeClosed = new int[faceNum];
            if (multiFaceInfo.leftEyeClosed != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.leftEyeClosed, _leftEyeClosed, 0, faceNum);

            var _rightEyeClosed = new int[faceNum];
            if (multiFaceInfo.rightEyeClosed != IntPtr.Zero)
                Marshal.Copy(multiFaceInfo.rightEyeClosed, _rightEyeClosed, 0, faceNum);

            for (int i = 0; i < multiFaceInfo.faceNum; i++)
            {
                var rect = Marshal.PtrToStructure<MRECT>(IntPtr.Add(multiFaceInfo.faceRect, i * mrectSize));
                faceRects.Add(rect);

                ASF_FaceDataInfo faceData = new ASF_FaceDataInfo();
                if (multiFaceInfo.faceDataInfoList != IntPtr.Zero)
                {
                    faceData = Marshal.PtrToStructure<ASF_FaceDataInfo>(IntPtr.Add(multiFaceInfo.faceDataInfoList, i * facedatainfoSize));
                    FaceDataInfos.Add(faceData);
                }

                var fo = (ArcSoftFace_OrientCode)_faceOrients[i];
                faceOrients.Add(fo);

                var wg = _wearGlasses[i] > 0.5;
                WearGlasses.Add(wg);

                var fs = _faceShelter[i] == 1;
                FaceShelter.Add(fs);

                var lec = _leftEyeClosed[i] == 1;
                LeftEyeClosed.Add(lec);

                var rec = _rightEyeClosed[i] == 1;
                RightEyeClosed.Add(rec);

                var fi = _faceId[i];
                faceID.Add(fi);

                FaceInfos.Add(new SingleFaceInfo
                {
                    ASFSingleFaceInfo = new ASF_SingleFaceInfo
                    {
                        faceDataInfo = faceData,
                        faceOrient = _faceOrients[i],
                        faceRect = rect
                    },
                    FaceID = fi,
                    FaceOrient = fo,
                    FaceRect = rect,
                    FaceShelter = fs,
                    LeftEyeClosed = lec,
                    RightEyeClosed = rec,
                    WearGlasses = wg
                });
            }
        }

        /// <summary>
        /// 人脸Rect结果集
        /// </summary>
        public List<MRECT> faceRects { get; private set; } = new List<MRECT>();

        /// <summary>
        /// 人脸角度结果集，与faceRects一一对应  对应ASF_OrientCode
        /// </summary>
        public List<ArcSoftFace_OrientCode> faceOrients { get; set; } = new List<ArcSoftFace_OrientCode>();

        /// <summary>
        /// 结果集大小
        /// </summary>
        public int faceNum { get; private set; }

        /// <summary>
        /// face ID，IMAGE模式下不返回FaceID
        /// </summary>
        public List<int> faceID { get; private set; } = new List<int>();

        /// <summary>
        /// 是否戴眼镜
        /// </summary>
        public List<bool> WearGlasses { get; private set; } = new List<bool>();
        /// <summary>
        /// 面部是否遮挡
        /// </summary>
        public List<bool> FaceShelter { get; private set; } = new List<bool>();
        /// <summary>
        /// 左眼状态
        /// </summary>
        public List<bool> LeftEyeClosed { get; private set; } = new List<bool>();
        /// <summary>
        /// 右眼状态
        /// </summary>
        public List<bool> RightEyeClosed { get; private set; } = new List<bool>();

        public List<int> Ages { get; private set; } = new List<int>();

        public List<ASF_FaceLandmark> faceLandmarks { get; private set; } = new List<ASF_FaceLandmark>();

        /// <summary>
        /// 多张人脸信息
        /// </summary>
        public List<ASF_FaceDataInfo> FaceDataInfos { get; private set; } = new List<ASF_FaceDataInfo>();

        public List<SingleFaceInfo> FaceInfos { get; private set; } = new List<SingleFaceInfo>();

        public List<Face3DAngle> Face3DAngles { get; private set; } = new List<Face3DAngle>();

        public List<int> LivenessResult { get; private set; } = new List<int>();

        public List<int> MaskInfo { get; private set; } = new List<int>();

        internal void SetAgeInfo(ASF_AgeInfo ageInfo)
        {
            int[] _ages = new int[ageInfo.num];
            Marshal.Copy(ageInfo.ageArray, _ages, 0, ageInfo.num);
            Ages = new List<int>(_ages);
            for (int i = 0; i < ageInfo.num; i++)
            {
                FaceInfos[i].Age = _ages[i];
            }
        }

        internal void SetGenderInfo(ASF_GenderInfo genderInfo)
        {
            int[] _genders = new int[genderInfo.num];
            Marshal.Copy(genderInfo.genderArray, _genders, 0, genderInfo.num);
            Ages = new List<int>(_genders);
            for (int i = 0; i < genderInfo.num; i++)
            {
                FaceInfos[i].Gender = _genders[i];
            }
        }

        internal void SetFace3DAngle(ASF_Face3DAngle face3DAngle)
        {
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
                FaceInfos[i].Face3DAngle = angle;
            }
        }

        internal void SetFaceLandmark(ASF_LandMarkInfo faceLandmark)
        {
            var size = Marshal.SizeOf<ASF_FaceLandmark>();
            for (int i = 0; i < faceLandmark.num; i++)
            {
                var obj = Marshal.PtrToStructure<ASF_FaceLandmark>(IntPtr.Add(faceLandmark.point, size * i));
                faceLandmarks.Add(obj);
                FaceInfos[i].FaceLandmark = obj;
            }
        }

        internal void SetMaskInfo(ASF_MaskInfo maskInfo)
        {
            int[] _mask = new int[maskInfo.num];
            Marshal.Copy(maskInfo.maskArray, _mask, 0, maskInfo.num);
            MaskInfo = new List<int>(_mask);
            for (int i = 0; i < maskInfo.num; i++)
            {
                FaceInfos[i].Mask = _mask[i] == 1;
            }
        }

        internal void SetLivenessInfo(ASF_LivenessInfo livenessInfo)
        {
            int[] _liveness = new int[livenessInfo.num];
            Marshal.Copy(livenessInfo.isLive, _liveness, 0, livenessInfo.num);

            LivenessResult = new List<int>(_liveness);
            for (int i = 0; i < livenessInfo.num; i++)
            {
                FaceInfos[i].Liveness = _liveness[i] == 1;
            }
        }
    }
}
