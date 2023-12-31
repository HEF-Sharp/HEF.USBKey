﻿using HEF.USBKey.Common;
using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEF.USBKey.Services.SKF
{
    public abstract class USBKeyService_SKF_Base_LoopMonitor : USBKeyService_SKF_Base
    {
        private const int DevicesMonitorIntervalSeconds = 2;

        private IList<SKF_PresentDevice> _prevPresentDevices = new List<SKF_PresentDevice>();

        internal USBKeyService_SKF_Base_LoopMonitor(IUSBKeyProvider_SKF usbKeyProvider_SKF)
            : base(usbKeyProvider_SKF)
        { }

        #region MonitorDeviceEvent
        protected override async Task ProcessingWaitDeviceEvent(Action<SKF_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var currentPresentDevices = GetPresentDevices();

                var deviceInOutEvents = BuildDeviceInOutEventsByComparePresentDevices(_prevPresentDevices, currentPresentDevices);

                foreach (var deviceInOutEvent in deviceInOutEvents)
                {
                    deviceEventAction?.Invoke(deviceInOutEvent);
                }

                _prevPresentDevices = currentPresentDevices.ToList();   //本次在线设备 作为 下次检测的Prev在线设备

                await Task.Delay(DevicesMonitorIntervalSeconds * 1000);
            }
        }

        private IEnumerable<SKF_DeviceInOutEvent> BuildDeviceInOutEventsByComparePresentDevices(
            IEnumerable<SKF_PresentDevice> prevPresentDevices, IEnumerable<SKF_PresentDevice> currentPresentDevices)
        {
            prevPresentDevices ??= Enumerable.Empty<SKF_PresentDevice>();
            currentPresentDevices ??= Enumerable.Empty<SKF_PresentDevice>();

            var prevPresentDeviceNames = prevPresentDevices.Select(m => m.DeviceName).ToArray();
            var currentPresentDeviceNames = currentPresentDevices.Select(m => m.DeviceName).ToArray();

            var plugInDeviceNames = currentPresentDeviceNames.Except(prevPresentDeviceNames).ToArray();
            var pullOutDeviceNames = prevPresentDeviceNames.Except(currentPresentDeviceNames).ToArray();

            foreach (var pullOutDeviceName in pullOutDeviceNames)
            {
                yield return new SKF_DeviceInOutEvent
                {
                    ProviderName = Provider.ProviderName,
                    DeviceName = pullOutDeviceName,
                    EventType = (int)DeviceEventTypes.PullOut
                };
            }

            foreach (var plugInDeviceName in plugInDeviceNames)
            {
                yield return new SKF_DeviceInOutEvent
                {
                    ProviderName = Provider.ProviderName,
                    DeviceName = plugInDeviceName,
                    EventType = (int)DeviceEventTypes.PlugIn
                };
            }
        }

        protected override void CancelWaitDeviceEvent()
        {
            //Nothing to do
        }
        #endregion
    }
}
