using System;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    public class USBKeyProvider_SKF_HaiTai30098 : USBKeyProvider_SKF_Base, IUSBKeyProvider_SKF
    {
        public const string SKF_ProviderName = "HaiTai30098";

        public string ProviderName => SKF_ProviderName;

        #region Native Methods
        protected override uint Native_SKF_EnumDev(bool bPresent, IntPtr szNameList, ref uint pulSize)
        {
            return Native_SKF_HaiTai30098.SKF_EnumDev(bPresent, szNameList, ref pulSize);
        }

        protected override uint Native_SKF_GetDevState(string szDevName, out uint pulDevState)
        {
            return Native_SKF_HaiTai30098.SKF_GetDevState(szDevName, out pulDevState);
        }

        protected override uint Native_SKF_ConnectDev(string szName, out IntPtr phDev)
        {
            return Native_SKF_HaiTai30098.SKF_ConnectDev(szName, out phDev);
        }

        protected override uint Native_SKF_GetDevInfo(IntPtr hDev, IntPtr pDevInfo)
        {
            return Native_SKF_HaiTai30098.SKF_GetDevInfo(hDev, pDevInfo);
        }

        protected override uint Native_SKF_DisConnectDev(IntPtr hDev)
        {
            return Native_SKF_HaiTai30098.SKF_DisConnectDev(hDev);
        }

        protected override uint Native_SKF_EnumApplication(IntPtr hDev, IntPtr szAppNameList, ref uint pulSize)
        {
            return Native_SKF_HaiTai30098.SKF_EnumApplication(hDev, szAppNameList, ref pulSize);
        }

        protected override uint Native_SKF_OpenApplication(IntPtr hDev, string szAppName, out IntPtr phApplication)
        {
            return Native_SKF_HaiTai30098.SKF_OpenApplication(hDev, szAppName, out phApplication);
        }

        protected override uint Native_SKF_CloseApplication(IntPtr hApplication)
        {
            return Native_SKF_HaiTai30098.SKF_CloseApplication(hApplication);
        }

        protected override uint Native_SKF_EnumContainer(IntPtr hApplication, IntPtr szContainerNameList, ref uint pulSize)
        {
            return Native_SKF_HaiTai30098.SKF_EnumContainer(hApplication, szContainerNameList, ref pulSize);
        }

        protected override uint Native_SKF_OpenContainer(IntPtr hApplication, string szContainerName, out IntPtr phContainer)
        {
            return Native_SKF_HaiTai30098.SKF_OpenContainer(hApplication, szContainerName, out phContainer);
        }

        protected override uint Native_SKF_CloseContainer(IntPtr hContainer)
        {
            return Native_SKF_HaiTai30098.SKF_CloseContainer(hContainer);
        }

        protected override uint Native_SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag, byte[] pbCert, ref uint pulCertLen)
        {
            return Native_SKF_HaiTai30098.SKF_ExportCertificate(hContainer, bSignFlag, pbCert, ref pulCertLen);
        }

        protected override uint Native_SKF_WaitForDevEvent(StringBuilder szDevName, ref uint pulDevNameLen, out uint pulEvent)
        {
            return Native_SKF_HaiTai30098.SKF_WaitForDevEvent(szDevName, ref pulDevNameLen, out pulEvent);
        }

        protected override uint Native_SKF_CancelWaitForDevEvent()
        {
            return Native_SKF_HaiTai30098.SKF_CancelWaitForDevEvent();
        }

        protected override uint Native_SKF_ChangePIN(IntPtr hApplication, uint ulPINType, string szOldPin, string szNewPin, out uint pulRetryCount)
        {
            return Native_SKF_HaiTai30098.SKF_ChangePIN(hApplication, ulPINType, szOldPin, szNewPin, out pulRetryCount);
        }

        protected override uint Native_SKF_VerifyPIN(IntPtr hApplication, uint ulPINType, string szPin, out uint pulRetryCount)
        {
            return Native_SKF_HaiTai30098.SKF_VerifyPIN(hApplication, ulPINType, szPin, out pulRetryCount);
        }
        #endregion
    }
}
