using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcFaceProSDK4net.Models
{
    public class SDKVersion
    {
        public SDKVersion(ASF_VERSION asfversion)
        {
            Version = Marshal.PtrToStringAnsi(asfversion.Version);
            BuildDate = Marshal.PtrToStringAnsi(asfversion.BuildDate);
            CopyRight = Marshal.PtrToStringAnsi(asfversion.CopyRight);
        }
        public string Version { get; set; }
        public string BuildDate { get; set; }
        public string CopyRight { get; set; }
    }
}
