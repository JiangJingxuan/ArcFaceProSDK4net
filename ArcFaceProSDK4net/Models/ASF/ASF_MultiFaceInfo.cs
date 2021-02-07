using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public struct ASF_MultiFaceInfo
    {
        public IntPtr faceRect;
        public IntPtr faceOrient;
        public int faceNum;
        public IntPtr faceID;
        public IntPtr wearGlasses;
        public IntPtr leftEyeClosed;
        public IntPtr rightEyeClosed;
        public IntPtr faceShelter;
        public IntPtr faceDataInfoList;
    }
}
