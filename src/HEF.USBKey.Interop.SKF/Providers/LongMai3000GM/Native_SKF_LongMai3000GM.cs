using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    internal static class Native_SKF_LongMai3000GM
    {
        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumDev(bool bPresent, IntPtr szNameList, ref uint pulSize);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevState(string szDevName, out uint pulDevState);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ConnectDev(string szName, out IntPtr phDev);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_GetDevInfo(IntPtr hDev, IntPtr pDevInfo);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_DisConnectDev(IntPtr hDev);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumApplication(IntPtr hDev, IntPtr szAppNameList, ref uint pulSize);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenApplication(IntPtr hDev, string szAppName, out IntPtr phApplication);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseApplication(IntPtr hApplication);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_EnumContainer(IntPtr hApplication, IntPtr szContainerNameList, ref uint pulSize);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_OpenContainer(IntPtr hApplication, string szContainerName, out IntPtr phContainer);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CloseContainer(IntPtr hContainer);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag, byte[] pbCert, ref uint pulCertLen);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_WaitForDevEvent(StringBuilder szDevName, ref uint pulDevNameLen, out uint pulEvent);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_CancelWaitForDevEvent();

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_ChangePIN(IntPtr hApplication, uint ulPINType, string szOldPin, string szNewPin, out uint pulRetryCount);

        [DllImport(@"\LongMai3000GM\GM3000SKF_ZJCA.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SKF_VerifyPIN(IntPtr hApplication, uint ulPINType, string szPin, out uint pulRetryCount);
    }
}
