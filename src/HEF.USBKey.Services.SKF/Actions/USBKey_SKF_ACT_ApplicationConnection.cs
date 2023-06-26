using HEF.USBKey.Interop.SKF;
using System;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_SKF_ACT_ApplicationConnection : IDisposable
    {
        private readonly SKFResult<IntPtr> _openResult;

        private readonly bool _openSuccess = false;
        private readonly IntPtr _hApplication = IntPtr.Zero;

        private bool _disposed = false;

        public USBKey_SKF_ACT_ApplicationConnection(IUSBKeyProvider_SKF usbKeyProvider_SKF, IntPtr hDevice, string appName)
        {
            USBKeyProvider_SKF = usbKeyProvider_SKF ?? throw new ArgumentNullException(nameof(usbKeyProvider_SKF));

            if (hDevice == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hDevice));
        
            if (string.IsNullOrWhiteSpace(appName))
                throw new ArgumentNullException(nameof(appName));

            _openResult = USBKeyProvider_SKF.SKF_OpenApplication(hDevice, appName);
            if (_openResult.IsSuccess())
            {
                _openSuccess = true;
                _hApplication = _openResult.Data;
            }
        }

        protected IUSBKeyProvider_SKF USBKeyProvider_SKF { get; }

        public SKFResult<IntPtr> OpenResult => _openResult;

        public bool Opened => _openSuccess;

        public IntPtr HApplication => _hApplication;

        #region IDisposable
        private void Close()
        {
            USBKeyProvider_SKF.SKF_CloseApplication(_hApplication);
        }

        public void Dispose()
        {
            if (!_openSuccess)
                return;

            if (_disposed)
                return;

            Close();
            _disposed = true;
        }
        #endregion
    }
}
