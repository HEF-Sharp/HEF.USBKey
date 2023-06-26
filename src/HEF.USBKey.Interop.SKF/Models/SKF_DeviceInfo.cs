using System.Runtime.InteropServices;

namespace HEF.USBKey.Interop.SKF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SKF_Version
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public byte Major;
        /// <summary>
        /// 次版本号
        /// </summary>
        public byte Minor;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SKF_DeviceInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public SKF_Version Version;
        /// <summary>
        /// 设备厂商
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Manufacturer;
        /// <summary>
        /// 发行厂商
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Issuer;
        /// <summary>
        /// 设备标签
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] 
        public byte[] Label;
        /// <summary>
        /// 序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SerialNumber;
        /// <summary>
        /// 设备硬件版本
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public SKF_Version HWVersion;
        /// <summary>
        /// 设备固件版本
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public SKF_Version FirmwareVersion;
        /// <summary>
        /// 分组密码算法标识
        /// </summary>        
        public uint AlgSymCap;
        /// <summary>
        /// 非对称密码算法标识
        /// </summary>
        public uint AlgAsymCap;
        /// <summary>
        /// 密码杂凑算法标识
        /// </summary>
        public uint AlgHashCap;
        /// <summary>
        /// 设备认证使用的分组密码算法标识
        /// </summary>
        public uint DevAuthAlgId;
        /// <summary>
        /// 设备总空间大小
        /// </summary>
        public uint TotalSpace;
        /// <summary>
        /// 用户可用空间大小
        /// </summary>
        public uint FreeSpace;
        /// <summary>
        /// 能够处理的ECC加密数据大小
        /// </summary>
        public uint MaxECCBufferSize;
        /// <summary>
        /// 能够处理的分组运算和杂凑运算的数据大小
        /// </summary>
        public uint MaxBufferSize;
        /// <summary>
        /// 保留扩展
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Reserved;
    }
}
