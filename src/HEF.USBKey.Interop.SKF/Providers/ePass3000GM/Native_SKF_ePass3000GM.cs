using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    internal static class Native_SKF_ePass3000GM
    {
        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumDev(bool bPresent, IntPtr szNameList, ref uint pulSize);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevState(string szDevName, out uint pulDevState);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ConnectDev(string szName, out IntPtr phDev);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevInfo(IntPtr hDev, IntPtr pDevInfo);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_DisConnectDev(IntPtr hDev);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumApplication(IntPtr hDev, IntPtr szAppNameList, ref uint pulSize);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenApplication(IntPtr hDev, string szAppName, out IntPtr phApplication);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseApplication(IntPtr hApplication);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumContainer(IntPtr hApplication, IntPtr szContainerNameList, ref uint pulSize);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenContainer(IntPtr hApplication, string szContainerName, out IntPtr phContainer);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseContainer(IntPtr hContainer);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag, byte[] pbCert, ref uint pulCertLen);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_WaitForDevEvent(StringBuilder szDevName, ref uint pulDevNameLen, out uint pulEvent);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CancelWaitForDevEvent();

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ChangePIN(IntPtr hApplication, uint ulPINType, string szOldPin, string szNewPin, out uint pulRetryCount);

        [DllImport(@"\ePass3000GM\ZJCAePass3000gm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_VerifyPIN(IntPtr hApplication, uint ulPINType, string szPin, out uint pulRetryCount);
    }
}
