using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public enum ASF_CompareModel
    {
        ASF_LIFE_PHOTO = 0x1, //用于生活照之间的特征比对，推荐阈值0.80
        ASF_ID_PHOTO = 0x2 //用于证件照或生活照与证件照之间的特征比对，推荐阈值0.82
    }
}
