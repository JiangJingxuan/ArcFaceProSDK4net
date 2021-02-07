using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public enum ASF_Mask
    {
        ASF_NONE = 0x00000000, //无属性
        ASF_FACE_DETECT = 0x00000001, //此处detect可以是tracking或者detection 两个引擎之一，具体的选择由detect mode 确定
        ASF_FACERECOGNITION = 0x00000004, //人脸特征
        ASF_AGE = 0x00000008, //年龄p
        ASF_GENDER = 0x00000010, //性别p
        ASF_FACE3DANGLE = 0x00000020, //3D角度p
        ASF_FACELANDMARK = 0x00000040, //额头区域检测p
        ASF_LIVENESS = 0x00000080, //RGB活体p
        ASF_IMAGEQUALITY = 0x00000200, //图像质量检测
        ASF_IR_LIVENESS = 0x00000400, //IR活体
        ASF_FACESHELTER = 0x00000800, //人脸遮挡
        ASF_MASKDETECT = 0x00001000, //口罩检测p
        ASF_UPDATE_FACEDATA = 0x00002000, //人脸信息

    }
}
