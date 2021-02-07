using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public enum ArcSoftFace_OrientPriority
    {
        ASF_OP_0_ONLY = 0x1, // 常规预览下正方向
        ASF_OP_90_ONLY = 0x2, // 基于0°逆时针旋转90°的方向
        ASF_OP_270_ONLY = 0x3, // 基于0°逆时针旋转270°的方向
        ASF_OP_180_ONLY = 0x4, // 基于0°旋转180°的方向（逆时针、顺时针效果一样）
        ASF_OP_ALL_OUT = 0x5 // 全角度
    }
}
