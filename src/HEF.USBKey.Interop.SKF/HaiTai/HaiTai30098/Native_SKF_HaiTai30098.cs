using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    internal static class Native_SKF_HaiTai30098
    {
        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumDev(bool bPresent, IntPtr szNameList, ref uint pulSize);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevState(string szDevName, out uint pulDevState);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ConnectDev(string szName, out IntPtr phDev);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevInfo(IntPtr hDev, IntPtr pDevInfo);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_DisConnectDev(IntPtr hDev);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumApplication(IntPtr hDev, IntPtr szAppNameList, ref uint pulSize);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenApplication(IntPtr hDev, string szAppName, out IntPtr phApplication);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseApplication(IntPtr hApplication);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumContainer(IntPtr hApplication, IntPtr szContainerNameList, ref uint pulSize);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenContainer(IntPtr hApplication, string szContainerName, out IntPtr phContainer);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseContainer(IntPtr hContainer);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag, byte[] pbCert, ref uint pulCertLen);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_WaitForDevEvent(StringBuilder szDevName, ref uint pulDevNameLen, out uint pulEvent);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CancelWaitForDevEvent();

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ChangePIN(IntPtr hApplication, uint ulPINType, string szOldPin, string szNewPin, out uint pulRetryCount);

        [DllImport(@"\HaiTai30098\SKFAPI30098.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_VerifyPIN(IntPtr hApplication, uint ulPINType, string szPin, out uint pulRetryCount);
    }
}
