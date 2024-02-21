using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HEF.USBKey.Interop.Pkcs11
{
    public abstract class USBKeyProvider_Pkcs11_Base
    {
        internal USBKeyProvider_Pkcs11_Base(string libraryRelativePath, string cspProvideName)
        {
            if (string.IsNullOrWhiteSpace(libraryRelativePath))
                throw new ArgumentNullException(nameof(libraryRelativePath));

            if (string.IsNullOrWhiteSpace(cspProvideName))
                throw new ArgumentNullException(cspProvideName);

            Pkcs11LibraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryRelativePath);
            CspProviderName = cspProvideName;

            InteropFactories = new Pkcs11InteropFactories();

            Pkcs11Library = InteropFactories.Pkcs11LibraryFactory.LoadPkcs11Library(
                InteropFactories, Pkcs11LibraryPath, AppType.MultiThreaded);
        }

        protected string Pkcs11LibraryPath { get; }

        public string CspProviderName { get; }

        protected Pkcs11InteropFactories InteropFactories { get; }

        protected IPkcs11Library Pkcs11Library { get; }

        public ILibraryInfo GetLibraryInfo() => Pkcs11Library.GetInfo();

        public IEnumerable<ISlot> GetSlotList(bool present)
        {
            var slotsType = present ? SlotsType.WithTokenPresent : SlotsType.WithOrWithoutTokenPresent;

            foreach(var slot in Pkcs11Library.GetSlotList(slotsType))
                yield return slot;
        }

        public ISlot GetSlotById(ulong slotId)
        {
            return GetSlotList(false).SingleOrDefault(slot => slot.SlotId == slotId);
        }

        public bool WaitForSlotEvent(out ISlot slot)
        {
            Pkcs11Library.WaitForSlotEvent(WaitType.Blocking, out var eventOccured, out slot);

            return eventOccured;
        }

        public void Dispose()
        {
            Pkcs11Library.Dispose();
        }
    }
}
