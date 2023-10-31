using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.IO;

namespace HEF.USBKey.Interop.Pkcs11
{
    public abstract class USBKeyProvider_Pkcs11_Base
    {
        internal USBKeyProvider_Pkcs11_Base(string libraryRelativePath)
        {
            if (string.IsNullOrWhiteSpace(libraryRelativePath))
                throw new ArgumentNullException(nameof(libraryRelativePath));

            Pkcs11LibraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryRelativePath);

            InteropFactories = new Pkcs11InteropFactories();

            Pkcs11Library = InteropFactories.Pkcs11LibraryFactory.LoadPkcs11Library(
                InteropFactories, Pkcs11LibraryPath, AppType.MultiThreaded);
        }

        protected string Pkcs11LibraryPath { get; }

        protected Pkcs11InteropFactories InteropFactories { get; }

        protected IPkcs11Library Pkcs11Library { get; }

        public void Dispose()
        {
            Pkcs11Library.Dispose();
        }
    }
}
