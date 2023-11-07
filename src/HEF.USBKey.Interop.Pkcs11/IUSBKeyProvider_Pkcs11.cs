using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.Collections.Generic;

namespace HEF.USBKey.Interop.Pkcs11
{
    public interface IUSBKeyProvider_Pkcs11 : IDisposable
    {
        string ProviderName { get; }

        string CspProviderName { get; }

        ILibraryInfo GetLibraryInfo();

        IEnumerable<ISlot> GetSlotList(bool present);

        ISlot GetSlotById(ulong slotId);

        bool WaitForSlotEvent(out ISlot slot);
    }
}
