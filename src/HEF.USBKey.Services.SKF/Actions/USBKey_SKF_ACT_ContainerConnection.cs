using HEF.USBKey.Interop.SKF;
using System;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_SKF_ACT_ContainerConnection : IDisposable
    {
        private readonly SKFResult<IntPtr> _openResult;

        private readonly bool _openSuccess = false;
        private readonly IntPtr _hContainer = IntPtr.Zero;

        private bool _disposed = false;

        public USBKey_SKF_ACT_ContainerConnection(IUSBKeyProvider_SKF usbKeyProvider_SKF, IntPtr hApplication, string containerName)
        {
            USBKeyProvider_SKF = usbKeyProvider_SKF ?? throw new ArgumentNullException(nameof(usbKeyProvider_SKF));

            if (hApplication == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hApplication));

            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentNullException(nameof(containerName));

            _openResult = USBKeyProvider_SKF.SKF_OpenContainer(hApplication, containerName);
            if (_openResult.IsSuccess())
            {
                _openSuccess = true;
                _hContainer = _openResult.Data;
            }
        }

        protected IUSBKeyProvider_SKF USBKeyProvider_SKF { get; }

        public SKFResult<IntPtr> OpenResult => _openResult;

        public bool Opened => _openSuccess;

        public IntPtr HContainer => _hContainer;

        #region IDisposable
        private void Close()
        {
            USBKeyProvider_SKF.SKF_CloseContainer(_hContainer);
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
