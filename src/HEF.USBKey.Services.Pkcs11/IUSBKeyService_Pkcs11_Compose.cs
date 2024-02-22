using System.Collections.Generic;

namespace HEF.USBKey.Services.Pkcs11
{
    public interface IUSBKeyService_Pkcs11_Compose
    {
        IEnumerable<Pkcs11_PresentSlot> GetPresentSlotList();

        IEnumerable<Pkcs11_Certificate_X509> ExportCertificates(string providerName, ulong slotId);

        bool ChangeTokenPIN(string providerName, ulong slotId, string oldPin, string newPin);

        bool VerifyTokenPIN(string providerName, ulong slotId, string pin);

        void StartMonitorSlotEvent();

        void AttachSlotEventHandlers(params IUSBKey_Pkcs11_Handler_SlotEvent[] usbKeySlotEventHandlers);

        void CancelMonitorSlotEvent();
    }
}
