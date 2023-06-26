using HEF.USBKey.Interop.SKF;
using System;

namespace HEF.USBKey.Services.SKF
{
    public static class USBKeySKFActionsExtensions
    {
        public static USBKey_SKF_ACT_DeviceConnection ACT_DeviceConnection(
            this IUSBKeyProvider_SKF usbKeyProvider_SKF, string deviceName)
        {
            return new USBKey_SKF_ACT_DeviceConnection(usbKeyProvider_SKF, deviceName);
        }

        public static USBKey_SKF_ACT_ApplicationConnection ACT_ApplicationConnection(
            this IUSBKeyProvider_SKF usbKeyProvider_SKF, IntPtr hDevice, string appName)
        {
            return new USBKey_SKF_ACT_ApplicationConnection(usbKeyProvider_SKF, hDevice, appName);
        }

        public static USBKey_SKF_ACT_ContainerConnection ACT_ContainerConnection(
            this IUSBKeyProvider_SKF usbKeyProvider_SKF, IntPtr hApplication, string containerName)
        {
            return new USBKey_SKF_ACT_ContainerConnection(usbKeyProvider_SKF, hApplication, containerName);
        }
    }
}
