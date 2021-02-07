using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public class SingleFaceInfo
    {
        public MRECT FaceRect { get; set; }
        public ArcSoftFace_OrientCode FaceOrient { get; set; }
        public int FaceID { get; set; }
        public bool WearGlasses { get; set; }
        public bool FaceShelter { get; set; }
        public bool LeftEyeClosed { get; set; }
        public bool RightEyeClosed { get; set; }
        public bool Liveness { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public bool Mask { get; set; }
        public Face3DAngle Face3DAngle { get; set; }
        public ASF_FaceLandmark FaceLandmark { get; set; }
        public ASF_SingleFaceInfo ASFSingleFaceInfo { get; set; }
    }
}
