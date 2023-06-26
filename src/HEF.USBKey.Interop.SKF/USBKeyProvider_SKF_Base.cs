using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    public abstract class USBKeyProvider_SKF_Base
    {
        public SKFResult<string[]> SKF_EnumDevice(bool bPresent)
        {
            return SKF_EnumAction(
                (ref uint pulSize) => Native_SKF_EnumDev(bPresent, IntPtr.Zero, ref pulSize),
                (IntPtr szNamesPtr, ref uint pulSize) => Native_SKF_EnumDev(bPresent, szNamesPtr, ref pulSize)
            );
        }

        public SKFResult<int> SKF_GetDeviceState(string szDevName)
        {            
            var resultCode = Native_SKF_GetDevState(szDevName, out var pulDevState);

            return SKFResultHelper.FromResultCode(resultCode).WithData(pulDevState.AsInt32());
        }

        public SKFResult<IntPtr> SKF_ConnectDevice(string szName)
        {
            var resultCode = Native_SKF_ConnectDev(szName, out var hDev);

            return SKFResultHelper.FromResultCode(resultCode).WithData(hDev);
        }

        public SKFResult<SKF_DeviceInfo> SKF_GetDeviceInfo(IntPtr hDev)
        {
            var pDevInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SKF_DeviceInfo)));

            var resultCode = Native_SKF_GetDevInfo(hDev, pDevInfo);
            var skfResult = SKFResultHelper.FromResultCode(resultCode);

            if (!skfResult.IsSuccess())
            {
                Marshal.FreeHGlobal(pDevInfo);
                return skfResult.ToResult<SKF_DeviceInfo>();
            }                

            var deviceInfo = (SKF_DeviceInfo)Marshal.PtrToStructure(pDevInfo, typeof(SKF_DeviceInfo));
            Marshal.FreeHGlobal(pDevInfo);

            return skfResult.WithData(deviceInfo);
        }

        public SKFResult SKF_DisConnectDevice(IntPtr hDev)
        {
            var resultCode = Native_SKF_DisConnectDev(hDev);

            return SKFResultHelper.FromResultCode(resultCode);
        }

        public SKFResult<string[]> SKF_EnumApplication(IntPtr hDev)
        {
            return SKF_EnumAction(
               (ref uint pulSize) => Native_SKF_EnumApplication(hDev, IntPtr.Zero, ref pulSize),
               (IntPtr szAppNamesPtr, ref uint pulSize) => Native_SKF_EnumApplication(hDev, szAppNamesPtr, ref pulSize)
            );
        }

        public SKFResult<IntPtr> SKF_OpenApplication(IntPtr hDev, string szAppName)
        {
            var resultCode = Native_SKF_OpenApplication(hDev, szAppName, out var hApplication);

            return SKFResultHelper.FromResultCode(resultCode).WithData(hApplication);
        }

        public SKFResult SKF_CloseApplication(IntPtr hApplication)
        {
            var resultCode = Native_SKF_CloseApplication(hApplication);

            return SKFResultHelper.FromResultCode(resultCode);
        }

        public SKFResult<string[]> SKF_EnumContainer(IntPtr hApplication)
        {
            return SKF_EnumAction(
               (ref uint pulSize) => Native_SKF_EnumContainer(hApplication, IntPtr.Zero, ref pulSize),
               (IntPtr szContainerNamesPtr, ref uint pulSize) => Native_SKF_EnumContainer(hApplication, szContainerNamesPtr, ref pulSize)
            );
        }

        public SKFResult<IntPtr> SKF_OpenContainer(IntPtr hApplication, string szContainerName)
        {
            var resultCode = Native_SKF_OpenContainer(hApplication, szContainerName, out var hContainer);

            return SKFResultHelper.FromResultCode(resultCode).WithData(hContainer);
        }

        public SKFResult SKF_CloseContainer(IntPtr hContainer)
        {
            var resultCode = Native_SKF_CloseContainer(hContainer);

            return SKFResultHelper.FromResultCode(resultCode);
        }

        public SKFResult<byte[]> SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag)
        {
            uint pulCertLen = 0;

            var resultCode = Native_SKF_ExportCertificate(hContainer, bSignFlag, null, ref pulCertLen);
            var skfResult = SKFResultHelper.FromResultCode(resultCode);

            if (!skfResult.IsSuccess())
                return skfResult.ToResult<byte[]>();

            if (pulCertLen < 1)
                return skfResult.ToResult<byte[]>();

            var bCert = new byte[pulCertLen];
            resultCode = Native_SKF_ExportCertificate(hContainer, bSignFlag, bCert, ref pulCertLen);

            return SKFResultHelper.FromResultCode(resultCode).WithData(bCert);
        }

        public SKFResult<SKF_DeviceEvent> SKF_WaitForDeviceEvent()
        {
            uint pulDevNameLen = 256;
            var szDevName = new StringBuilder(pulDevNameLen.AsInt32());

            var resultCode = Native_SKF_WaitForDevEvent(szDevName, ref pulDevNameLen, out var pulEvent);
            var skfResult = SKFResultHelper.FromResultCode(resultCode);

            if (!skfResult.IsSuccess())
                return skfResult.ToResult<SKF_DeviceEvent>();

            var deviceEvent = new SKF_DeviceEvent { DeviceName = szDevName.ToString(), EventType = pulEvent.AsInt32() };
            return skfResult.WithData(deviceEvent);
        }

        public SKFResult SKF_CancelWaitForDeviceEvent()
        {
            var resultCode = Native_SKF_CancelWaitForDevEvent();

            return SKFResultHelper.FromResultCode(resultCode);
        }

        public SKFResult<int> SKF_ChangePIN(IntPtr hApplication, int ulPINType, string szOldPin, string szNewPin)
        {
            var resultCode = Native_SKF_ChangePIN(hApplication, ulPINType.AsUInt32(), szOldPin, szNewPin, out var pulRetryCount);

            return SKFResultHelper.FromResultCode(resultCode).WithData(pulRetryCount.AsInt32());
        }

        public SKFResult<int> SKF_VerifyPIN(IntPtr hApplication, int ulPINType, string szPin)
        {
            var resultCode = Native_SKF_VerifyPIN(hApplication, ulPINType.AsUInt32(), szPin, out var pulRetryCount);

            return SKFResultHelper.FromResultCode(resultCode).WithData(pulRetryCount.AsInt32());
        }

        #region Native Methods
        protected abstract uint Native_SKF_EnumDev(bool bPresent, IntPtr szNameList, ref uint pulSize);

        protected abstract uint Native_SKF_GetDevState(string szDevName, out uint pulDevState);

        protected abstract uint Native_SKF_ConnectDev(string szName, out IntPtr phDev);

        protected abstract uint Native_SKF_GetDevInfo(IntPtr hDev, IntPtr pDevInfo);

        protected abstract uint Native_SKF_DisConnectDev(IntPtr hDev);

        protected abstract uint Native_SKF_EnumApplication(IntPtr hDev, IntPtr szAppNameList, ref uint pulSize);

        protected abstract uint Native_SKF_OpenApplication(IntPtr hDev, string szAppName, out IntPtr phApplication);
        
        protected abstract uint Native_SKF_CloseApplication(IntPtr hApplication);

        protected abstract uint Native_SKF_EnumContainer(IntPtr hApplication, IntPtr szContainerNameList, ref uint pulSize);
        
        protected abstract uint Native_SKF_OpenContainer(IntPtr hApplication, string szContainerName, out IntPtr phContainer);
        
        protected abstract uint Native_SKF_CloseContainer(IntPtr hContainer);

        protected abstract uint Native_SKF_ExportCertificate(IntPtr hContainer, bool bSignFlag, byte[] pbCert, ref uint pulCertLen);
        
        protected abstract uint Native_SKF_WaitForDevEvent(StringBuilder szDevName, ref uint pulDevNameLen, out uint pulEvent);
        
        protected abstract uint Native_SKF_CancelWaitForDevEvent();

        protected abstract uint Native_SKF_ChangePIN(IntPtr hApplication, uint ulPINType, string szOldPin, string szNewPin, out uint pulRetryCount);

        protected abstract uint Native_SKF_VerifyPIN(IntPtr hApplication, uint ulPINType, string szPin, out uint pulRetryCount);
        #endregion

        #region Helper Functions

        #region SKF_EnumAction
        protected delegate uint Func_SKF_EnumSize(ref uint pulSize);

        protected delegate uint Func_SKF_EnumResult(IntPtr szNamesPtr, ref uint pulSize);

        protected static SKFResult<string[]> SKF_EnumAction(Func_SKF_EnumSize enumSizeFunc, Func_SKF_EnumResult enumResultFunc)
        {
            uint pulSize = 0;

            var resultCode = enumSizeFunc.Invoke(ref pulSize);
            var skfResult = SKFResultHelper.FromResultCode(resultCode);

            if (!skfResult.IsSuccess())
                return skfResult.ToResult<string[]>();

            if (pulSize < 1)
                return skfResult.ToResult<string[]>();

            var szNamesPtr = Marshal.AllocHGlobal(pulSize.AsInt32());
            resultCode = enumResultFunc.Invoke(szNamesPtr, ref pulSize);

            var szNames = Marshal.PtrToStringAnsi(szNamesPtr, pulSize.AsInt32());
            Marshal.FreeHGlobal(szNamesPtr);
            var szNameArr = szNames.TrimEnd('\0').Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

            return SKFResultHelper.FromResultCode(resultCode).WithData(szNameArr);
        }
        #endregion

        #endregion
    }
}
