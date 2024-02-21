namespace HEF.USBKey.Services.Pkcs11
{
    public interface IUSBKey_Pkcs11_Handler_SlotEvent
    {
        void Handle_SlotInOutEvent(Pkcs11_SlotInOutEvent slotInOutEvent, IUSBKeyService_Pkcs11 usbKeyPkcs11Service);
    }
}
