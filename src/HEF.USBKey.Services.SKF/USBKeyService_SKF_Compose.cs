﻿using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_Compose : IUSBKeyService_SKF_Compose
    {
        private readonly IDictionary<string, IUSBKeyService_SKF> _usbKeySKFServiceDict;

        private CancellationTokenSource _monitorDeviceEventCts;

        private readonly List<IUSBKey_SKF_Handler_DeviceEvent> _usbKeySKFDeviceEventHandlers;

        #region Constructor
        public USBKeyService_SKF_Compose(IEnumerable<IUSBKeyService_SKF> usbKeySKFServices)
        {
            if (usbKeySKFServices == null || !usbKeySKFServices.Any())
                throw new ArgumentNullException(nameof(usbKeySKFServices));

            _usbKeySKFServiceDict = new Dictionary<string, IUSBKeyService_SKF>();
            InitUSBKeySKFServices(usbKeySKFServices);

            _usbKeySKFDeviceEventHandlers = new List<IUSBKey_SKF_Handler_DeviceEvent>();
        }

        private void InitUSBKeySKFServices(IEnumerable<IUSBKeyService_SKF> usbKeySKFServices)
        {
            foreach (var usbKeySKFService in usbKeySKFServices)
            {
                AddOrUpdateUSBKeySKFService(usbKeySKFService);
            }
        }

        private void AddOrUpdateUSBKeySKFService(IUSBKeyService_SKF usbKeySKFService)
        {
            var providerName = usbKeySKFService.Provider.ProviderName;

            if (_usbKeySKFServiceDict.ContainsKey(providerName))
            {
                _usbKeySKFServiceDict[providerName] = usbKeySKFService;
                return;
            }

            _usbKeySKFServiceDict.Add(providerName, usbKeySKFService);
        }
        #endregion

        public IEnumerable<SKF_PresentDevice> GetPresentDevices()
        {
            foreach (var usbKeySKFService in _usbKeySKFServiceDict.Values)
            {
                var presentDevices = usbKeySKFService.GetPresentDevices();

                foreach (var presentDevice in presentDevices)
                {
                    yield return presentDevice;
                }
            }
        }

        public IEnumerable<SKF_Certificate_X509> ExportDeviceCertificates(string providerName, string deviceName)
        {
            var usbKeySKFService = GetMatchUSBKeySKFService(providerName);

            return usbKeySKFService.ExportDeviceX509Certificates(deviceName);
        }

        public SKFResult<int> ChangeDeviceDefaultAppPIN(string providerName, string deviceName, string oldPin, string newPin)
        {
            var usbKeySKFService = GetMatchUSBKeySKFService(providerName);

            return usbKeySKFService.ChangeDeviceDefaultAppPIN(deviceName, oldPin, newPin);
        }

        public SKFResult<int> ChangeDeviceAppPIN(string providerName, string deviceName, string appName, string oldPin, string newPin)
        {
            var usbKeySKFService = GetMatchUSBKeySKFService(providerName);

            return usbKeySKFService.ChangeDeviceAppPIN(deviceName, appName, oldPin, newPin);
        }

        public SKFResult<int> VerifyDeviceDefaultAppPIN(string providerName, string deviceName, string pin)
        {
            var usbKeySKFService = GetMatchUSBKeySKFService(providerName);

            return usbKeySKFService.VerifyDeviceDefaultAppPIN(deviceName, pin);
        }

        public SKFResult<int> VerifyDeviceAppPIN(string providerName, string deviceName, string appName, string pin)
        {
            var usbKeySKFService = GetMatchUSBKeySKFService(providerName);

            return usbKeySKFService.VerifyDeviceAppPIN(deviceName, appName, pin);
        }

        public void StartMonitorDeviceEvent()
        {
            if (_monitorDeviceEventCts != null && !_monitorDeviceEventCts.IsCancellationRequested)
                return;   //监听设备事件 正在进行，则不做处理

            _monitorDeviceEventCts = new CancellationTokenSource();

            var deviceInOutEventHandle = BuildDeviceInOutEventHandle();

            foreach (var usbKeySKFService in _usbKeySKFServiceDict.Values)
            {
                usbKeySKFService.StartMonitorDeviceEvent(deviceInOutEventHandle, _monitorDeviceEventCts.Token);
            }
        }

        public void AttachDeviceEventHandlers(params IUSBKey_SKF_Handler_DeviceEvent[] usbKeyDeviceEventHandlers)
        {
            if (usbKeyDeviceEventHandlers == null || !usbKeyDeviceEventHandlers.Any())
                return;

            _usbKeySKFDeviceEventHandlers.AddRange(usbKeyDeviceEventHandlers);
        }

        public void CancelMonitorDeviceEvent()
        {
            if (_monitorDeviceEventCts != null && !_monitorDeviceEventCts.IsCancellationRequested)
                _monitorDeviceEventCts.Cancel();
        }

        #region Helper Functions
        protected IUSBKeyService_SKF GetMatchUSBKeySKFService(string providerName)
        {
            if (_usbKeySKFServiceDict.TryGetValue(providerName, out var matchUSBKeySKFService))
            {
                return matchUSBKeySKFService;
            }

            throw new InvalidOperationException("not found target provider name usbKey skf service");
        }
        #endregion

        #region Handle DeviceInOutEvent
        protected Action<SKF_DeviceInOutEvent> BuildDeviceInOutEventHandle()
        {
            return deviceInOutEvent =>
            {
                var usbKeySKFService = GetMatchUSBKeySKFService(deviceInOutEvent.ProviderName);

                foreach (var usbKeyDeviceEventHandler in _usbKeySKFDeviceEventHandlers)
                {
                    usbKeyDeviceEventHandler.Handle_DeviceInOutEvent(deviceInOutEvent, usbKeySKFService);
                }
            };
        }
        #endregion
    }
}
