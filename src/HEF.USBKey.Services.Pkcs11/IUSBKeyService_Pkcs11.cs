using HEF.USBKey.Interop.Pkcs11;
using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HEF.USBKey.Services.Pkcs11
{
    public interface IUSBKeyService_Pkcs11
    {
        IUSBKeyProvider_Pkcs11 Provider { get; }

        IEnumerable<ISlot> GetPresentSlotList();

        ISlot GetPresentSlotById(ulong slotId);

        IEnumerable<Pkcs11_Certificate> ExportCertificates(ulong slotId);
        
        void StartMonitorSlotEvent(Action<Pkcs11_SlotInOutEvent> slotEventAction, CancellationToken cancellationToken);
    }
}
