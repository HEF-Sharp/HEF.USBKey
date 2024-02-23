namespace HEF.USBKey.Common
{
    public interface IUSBKey_Handler_DeviceEvent
    {
        void Handle_DeviceInOutEvent(USBKey_DeviceInOutEvent deviceInOutEvent, IUSBKeyService usbKeyService);
    }
}
