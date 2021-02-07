using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public struct ASF_SingleFaceInfo
    {
        public MRECT faceRect;
        public int faceOrient;
        public ASF_FaceDataInfo faceDataInfo;
    }


    public struct MRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

    }
}
