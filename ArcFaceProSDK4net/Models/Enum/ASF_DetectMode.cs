using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public enum ASF_DetectMode : uint
    {
        ASF_DETECT_MODE_VIDEO = 0x00000000, //VIDEO模式，一般用于多帧连续检测
        ASF_DETECT_MODE_IMAGE = 0xFFFFFFFF //IMAGE模式，一般用于静态图的单次检测
    }
}
