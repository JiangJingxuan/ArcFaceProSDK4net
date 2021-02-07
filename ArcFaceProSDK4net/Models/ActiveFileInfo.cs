using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public class ActiveFileInfo
    {
        public ActiveFileInfo()
        {

        }
        public ActiveFileInfo(ASF_ActiveFileInfo fileInfo)
        {
            startTime = Marshal.PtrToStringAnsi(fileInfo.startTime);
            endTime = Marshal.PtrToStringAnsi(fileInfo.endTime);
            activeKey = Marshal.PtrToStringAnsi(fileInfo.activeKey);
            platform = Marshal.PtrToStringAnsi(fileInfo.platform);
            sdkType = Marshal.PtrToStringAnsi(fileInfo.sdkType);
            appId = Marshal.PtrToStringAnsi(fileInfo.appId);
            sdkKey = Marshal.PtrToStringAnsi(fileInfo.sdkKey);
            sdkVersion = Marshal.PtrToStringAnsi(fileInfo.sdkVersion);
            fileVersion = Marshal.PtrToStringAnsi(fileInfo.fileVersion);
        }
        public string startTime { get; private set; }
        public string endTime { get; private set; }
        public string activeKey { get; private set; }
        public string platform { get; private set; }
        public string sdkType { get; private set; }
        public string appId { get; private set; }
        public string sdkKey { get; private set; }
        public string sdkVersion { get; private set; }
        public string fileVersion { get; private set; }
    }
}
