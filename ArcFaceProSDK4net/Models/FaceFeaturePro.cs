using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public class FaceFeaturePro
    {
        public ASF_FaceFeature ASFFaceFeature { get; set; }
        public int Size { get; set; }
        public byte[] Buffers { get; set; }
    }
}
