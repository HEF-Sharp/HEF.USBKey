using HEF.USBKey.Interop.SKF;
using System;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_SKF_ACT_DeviceConnection : IDisposable
    {
        private readonly SKFResult<IntPtr> _connectResult;

        private readonly bool _connectSuccess = false;
        private readonly IntPtr _hDevice = IntPtr.Zero;

        private bool _disposed = false;

        public USBKey_SKF_ACT_DeviceConnection(IUSBKeyProvider_SKF usbKeyProvider_SKF, string deviceName)
        {
            USBKeyProvider_SKF = usbKeyProvider_SKF ?? throw new ArgumentNullException(nameof(usbKeyProvider_SKF));

            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentNullException(nameof(deviceName));

            _connectResult = USBKeyProvider_SKF.SKF_ConnectDevice(deviceName);
            if (_connectResult.IsSuccess())
            {
                _connectSuccess = true;
                _hDevice = _connectResult.Data;
            }
        }

        protected IUSBKeyProvider_SKF USBKeyProvider_SKF { get; }

        public SKFResult<IntPtr> ConnectResult => _connectResult;

        public bool Connected => _connectSuccess;

        public IntPtr HDevice => _hDevice;

        #region IDisposable
        private void DisConnect()
        {
            USBKeyProvider_SKF.SKF_DisConnectDevice(_hDevice);
        }

        public void Dispose()
        {
            if (!_connectSuccess)
                return;

            if (_disposed)
                return;

            DisConnect();
            _disposed = true;
        }
        #endregion
    }
}
