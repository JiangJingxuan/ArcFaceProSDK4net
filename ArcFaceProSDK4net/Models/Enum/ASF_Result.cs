using System;
using System.Collections.Generic;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public enum MResult
    {
        MOK = 0x0,// 成功
        MERR_UNKNOWN = 0x1,// 错误原因不明
        MERR_INVALID_PARAM = 0x2,// 无效的参数
        MERR_UNSUPPORTED = 0x3,// 引擎不支持
        MERR_NO_MEMORY = 0x4,// 内存不足
        MERR_BAD_STATE = 0x5,// 状态错误
        MERR_USER_CANCEL = 0x6,// 用户取消相关操作
        MERR_EXPIRED = 0x7,// 操作时间过期
        MERR_USER_PAUSE = 0x8,// 用户暂停操作
        MERR_BUFFER_OVERFLOW = 0x9,// 缓冲上溢
        MERR_BUFFER_UNDERFLOW = 0xA,// 缓冲下溢
        MERR_NO_DISKSPACE = 0xB,// 存贮空间不足
        MERR_COMPONENT_NOT_EXIST = 0xC,// 组件不存在
        MERR_GLOBAL_DATA_NOT_EXIST = 0xD,// 全局数据不存在
        MERR_FSDK_INVALID_APP_ID = 0x7001,// 无效的AppId
        MERR_FSDK_INVALID_SDK_ID = 0x7002,// 无效的SDKkey
        MERR_FSDK_INVALID_ID_PAIR = 0x7003,// AppId和SDKKey不匹配
        MERR_FSDK_MISMATCH_ID_AND_SDK = 0x7004,// SDKKey和使用的SDK不匹配, 请检查入参
        MERR_FSDK_SYSTEM_VERSION_UNSUPPORTED = 0x7005,// 系统版本不被当前SDK所支持
        MERR_FSDK_LICENCE_EXPIRED = 0x7006,// SDK有效期过期，需要重新下载更新
        MERR_FSDK_FR_INVALID_MEMORY_INFO = 0x12001,// 无效的输入内存
        MERR_FSDK_FR_INVALID_IMAGE_INFO = 0x12002,// 无效的输入图像参数
        MERR_FSDK_FR_INVALID_FACE_INFO = 0x12003,// 无效的脸部信息
        MERR_FSDK_FR_NO_GPU_AVAILABLE = 0x12004,// 当前设备无GPU可用
        MERR_FSDK_FR_MISMATCHED_FEATURE_LEVEL = 0x12005,// 待比较的两个人脸特征的版本不一致
        MERR_FSDK_FACEFEATURE_UNKNOWN = 0x14001,// 人脸特征检测错误未知
        MERR_FSDK_FACEFEATURE_MEMORY = 0x14002,// 人脸特征检测内存错误
        MERR_FSDK_FACEFEATURE_INVALID_FORMAT = 0x14003,// 人脸特征检测格式错误
        MERR_FSDK_FACEFEATURE_INVALID_PARAM = 0x14004,// 人脸特征检测参数错误
        MERR_FSDK_FACEFEATURE_LOW_CONFIDENCE_LEVEL = 0x14005,// 人脸特征检测结果置信度低
        MERR_FSDK_FACEFEATURE_EXPIRED = 0x14006,// 人脸特征检测结果操作过期
        MERR_FSDK_FACEFEATURE_MISSFACE = 0x14007,// 人脸特征检测人脸丢失
        MERR_FSDK_FACEFEATURE_NO_FACE = 0x14008,// 人脸特征检测没有人脸
        MERR_FSDK_FACEFEATURE_FACEDATE = 0x14009,// 人脸特征检测人脸信息错误
        MERR_ASF_EX_FEATURE_UNSUPPORTED_ON_INIT = 0x15001,// Engine不支持的检测属性
        MERR_ASF_EX_FEATURE_UNINITED = 0x15002,// 需要检测的属性未初始化
        MERR_ASF_EX_FEATURE_UNPROCESSED = 0x15003,// 待获取的属性未在process中处理过
        MERR_ASF_EX_FEATURE_UNSUPPORTED_ON_PROCESS = 0x15004,// PROCESS不支持的检测属性，例如FR，有自己独立的处理函数
        MERR_ASF_EX_INVALID_IMAGE_INFO = 0x15005,// 无效的输入图像
        MERR_ASF_EX_INVALID_FACE_INFO = 0x15006,// 无效的脸部信息
        MERR_ASF_ACTIVATION_FAIL = 0x16001,// SDK激活失败，请打开读写权限
        MERR_ASF_ALREADY_ACTIVATED = 0x16002,// SDK已激活
        MERR_ASF_NOT_ACTIVATED = 0x16003,// SDK未激活
        MERR_ASF_SCALE_NOT_SUPPORT = 0x16004,// detectFaceScaleVal不支持
        MERR_ASF_ACTIVEFILE_SDKTYPE_MISMATCH = 0x16005,// 激活文件与SDK类型不匹配，请确认使用的sdk
        MERR_ASF_DEVICE_MISMATCH = 0x16006,// 设备不匹配
        MERR_ASF_UNIQUE_IDENTIFIER_ILLEGAL = 0x16007,// 唯一标识不合法
        MERR_ASF_PARAM_NULL = 0x16008,// 参数为空
        MERR_ASF_LIVENESS_EXPIRED = 0x16009,// 活体已过期
        MERR_ASF_VERSION_NOT_SUPPORT = 0x1600A,// 版本不支持
        MERR_ASF_SIGN_ERROR = 0x1600B,// 签名错误
        MERR_ASF_DATABASE_ERROR = 0x1600C,// 激活信息保存异常
        MERR_ASF_UNIQUE_CHECKOUT_FAIL = 0x1600D,// 唯一标识符校验失败
        MERR_ASF_COLOR_SPACE_NOT_SUPPORT = 0x1600E,// 颜色空间不支持
        MERR_ASF_IMAGE_WIDTH_HEIGHT_NOT_SUPPORT = 0x1600F,// 图片宽高不支持，宽度需四字节对齐
        MERR_ASF_READ_PHONE_STATE_DENIED = 0x16010,// android.permission.READ_PHONE_STATE权限被拒绝
        MERR_ASF_ACTIVATION_DATA_DESTROYED = 0x16011,// 激活数据被破坏, 请删除激活文件，重新进行激活
        MERR_ASF_SERVER_UNKNOWN_ERROR = 0x16012,// 服务端未知错误
        MERR_ASF_INTERNET_DENIED = 0x16013,// INTERNET权限被拒绝
        MERR_ASF_ACTIVEFILE_SDK_MISMATCH = 0x16014,// 激活文件与SDK版本不匹配, 请重新激活
        MERR_ASF_DEVICEINFO_LESS = 0x16015,// 设备信息太少，不足以生成设备指纹
        MERR_ASF_LOCAL_TIME_NOT_CALIBRATED = 0x16016,//客户端时间与服务器时间（即北京时间）前后相差在30分钟以上
        MERR_ASF_APPID_DATA_DECRYPT = 0x16017,// 数据校验异常
        MERR_ASF_APPID_APPKEY_SDK_MISMATCH = 0x16018,// 传入的AppId和AppKey与使用的SDK版本不一致
        MERR_ASF_NO_REQUEST = 0x16019,// 短时间大量请求会被禁止请求,30分钟之后解封
        MERR_ASF_ACTIVE_FILE_NO_EXIST = 0x1601A,// 激活文件不存在
        MERR_ASF_DETECT_MODEL_UNSUPPORTED = 0x1601B,//检测模型不支持，请查看对应接口说明，使用当前支持的检测模型
        MERR_ASF_CURRENT_DEVICE_TIME_INCORRECT = 0x1601C,// 当前设备时间不正确，请调整设备时间
        MERR_ASF_ACTIVATION_QUANTITY_OUT_OF_LIMIT = 0x1601D,// 年度激活数量超出限制，次年清零
        MERR_ASF_NETWORK_COULDNT_RESOLVE_HOST = 0x17001,// 无法解析主机地址
        MERR_ASF_NETWORK_COULDNT_CONNECT_SERVER = 0x17002,// 无法连接服务器
        MERR_ASF_NETWORK_CONNECT_TIMEOUT = 0x17003,// 网络连接超时
        MERR_ASF_NETWORK_UNKNOWN_ERROR = 0x17004,// 网络未知错误
        MERR_ASF_ACTIVEKEY_COULDNT_CONNECT_SERVER = 0x18001,// 无法连接激活服务器
        MERR_ASF_ACTIVEKEY_SERVER_SYSTEM_ERROR = 0x18002,// 服务器系统错误
        MERR_ASF_ACTIVEKEY_POST_PARM_ERROR = 0x18003,// 请求参数错误
        MERR_ASF_ACTIVEKEY_PARM_MISMATCH = 0x18004,// ACTIVEKEY与APPID、SDKKEY不匹配
        MERR_ASF_ACTIVEKEY_ACTIVEKEY_ACTIVATED = 0x18005,// ACTIVEKEY已经被使用
        MERR_ASF_ACTIVEKEY_ACTIVEKEY_FORMAT_ERROR = 0x18006,// ACTIVEKEY信息异常
        MERR_ASF_ACTIVEKEY_APPID_PARM_MISMATCH = 0x18007,// ACTIVEKEY与APPID不匹配
        MERR_ASF_ACTIVEKEY_SDK_FILE_MISMATCH = 0x18008,// SDK与激活文件版本不匹配
        MERR_ASF_ACTIVEKEY_EXPIRED = 0x18009,// ACTIVEKEY已过期
        MERR_ASF_ACTIVEKEY_DEVICE_OUT_OF_LIMIT = 0x1800A,// 批量授权激活码设备数超过限制
        MERR_ASF_LICENSE_FILE_NOT_EXIST = 0x19001,// 离线授权文件不存在或无读写权限
        MERR_ASF_LICENSE_FILE_DATA_DESTROYED = 0x19002,// 离线授权文件已损坏
        MERR_ASF_LICENSE_FILE_SDK_MISMATCH = 0x19003,// 离线授权文件与SDK版本不匹配
        MERR_ASF_LICENSE_FILEINFO_SDKINFO_MISMATCH = 0x19004,// 离线授权文件与SDK信息不匹配
        MERR_ASF_LICENSE_FILE_FINGERPRINT_MISMATCH = 0x19005,// 离线授权文件与设备指纹不匹配
        MERR_ASF_LICENSE_FILE_EXPIRED = 0x19006,// 离线授权文件已过期
        MERR_ASF_LOCAL_EXIST_USEFUL_ACTIVE_FILE = 0x19007,// 离线授权文件不可用，本地原有激活文件可继续使用
        MERR_ASF_LICENSE_FILE_VERSION_TOO_LOW = 0x19008,//离线授权文件版本过低，请使用新版本激活助手重新进行离线激活
    }

    
}
