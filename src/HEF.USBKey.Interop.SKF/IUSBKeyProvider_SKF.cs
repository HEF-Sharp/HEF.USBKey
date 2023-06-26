using System;

namespace HEF.USBKey.Interop.SKF
{
    public interface IUSBKeyProvider_SKF
    {
        string ProviderName { get; }

        SKFResult<string[]> SKF_EnumDevice(bool bPresent);

        SKFResult<int> SKF_GetDeviceState(string szDevName);

        SKFResult<IntPtr> SKF_ConnectDevice(string szName);

        SKFResult<SKF_DeviceInfo> SKF_GetDeviceInfo(IntPtr hDev);

        SKFResult SKF_DisConnectDevice(IntPtr hDev);

        SKFResult<string[]> SKF_EnumApplication(IntPtr hDev);

        SKFResult<IntPtr> SKF_OpenApplication(IntPtr hDev, string szAppName);

        SKFResult SKF_CloseApplication(IntPtr hApplication);

        SKFResult<string[]> SKF_EnumContainer(IntPtr hApplication);
        
        SKFResult<IntPtr> SKF_OpenContainer(IntPtr hApplication, string szContainerName);
        
        SKFResult SKF_CloseContainer(IntPtr hContainer);

        SKFResult<byte[]> SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag);

        SKFResult<SKF_DeviceEvent> SKF_WaitForDeviceEvent();

        SKFResult SKF_CancelWaitForDeviceEvent();

        SKFResult<int> SKF_ChangePIN(IntPtr hApplication, int ulPINType, string szOldPin, string szNewPin);

        SKFResult<int> SKF_VerifyPIN(IntPtr hApplication, int ulPINType, string szPin);
    }
}
