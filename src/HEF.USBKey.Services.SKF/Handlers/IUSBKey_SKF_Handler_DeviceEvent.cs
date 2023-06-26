namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKey_SKF_Handler_DeviceEvent
    {
        void Handle_DeviceInOutEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService);
    }
}