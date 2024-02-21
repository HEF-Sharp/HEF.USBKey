using HEF.USBKey.Common;
using System;
using System.Linq;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Pkcs11_LocalCertSync : IUSBKeyService_LocalCertSync
    {
        public USBKeyService_Pkcs11_LocalCertSync(IUSBKeyService_Pkcs11_Compose composeUSBKeyPkcs11Service,
            IUSBKeyService_LocalCertStore usbKeyLocalCertStoreService)
        {
            ComposeUSBKeyPkcs11Service = composeUSBKeyPkcs11Service ?? throw new ArgumentException(nameof(composeUSBKeyPkcs11Service));

            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));

            USBKeyLocalCertSlotEventHandler = new USBKey_Pkcs11_Handler_SlotEvent_LocalCertSync(usbKeyLocalCertStoreService);
        }

        protected IUSBKeyService_Pkcs11_Compose ComposeUSBKeyPkcs11Service { get; }

        protected IUSBKeyService_LocalCertStore USBKeyLocalCertStoreService { get; }

        protected IUSBKey_Pkcs11_Handler_SlotEvent USBKeyLocalCertSlotEventHandler { get; }

        public void StartSyncUSBKeysCertsToLocalCurrentUser()
        {
            var presentSlots = ComposeUSBKeyPkcs11Service.GetPresentSlotList();

            foreach (var presentSlot in presentSlots)
            {
                var slotX509Certs = ComposeUSBKeyPkcs11Service.ExportCertificates(presentSlot.ProviderName, presentSlot.Slot.SlotId);

                USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(presentSlot.ProviderName, presentSlot.Slot.SlotId.ToString(), slotX509Certs.ToArray());
            }

            ComposeUSBKeyPkcs11Service.AttachSlotEventHandlers(USBKeyLocalCertSlotEventHandler);
            ComposeUSBKeyPkcs11Service.StartMonitorSlotEvent();
        }

        public void CancelSyncUSBKeysCertsToLocalCurrentUser()
        {
            ComposeUSBKeyPkcs11Service.CancelMonitorSlotEvent();

            USBKeyLocalCertStoreService.RemoveAllDeviceCertsFromLocalCurrentUser();
        }
    }
}
