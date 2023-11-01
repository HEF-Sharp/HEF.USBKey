using HEF.USBKey.Interop.Pkcs11;
using Net.Pkcs11Interop.HighLevelAPI;
using System.Collections.Generic;

namespace HEF.USBKey.Services.Pkcs11
{
    public interface IUSBKeyService_Pkcs11
    {
        IUSBKeyProvider_Pkcs11 Provider { get; }

        IEnumerable<ISlot> GetPresentSlotList();

        ISlot GetPresentSlotById(ulong slotId);

        IEnumerable<Pkcs11_Certificate> ExportCertificates(ulong slotId);
    }
}
