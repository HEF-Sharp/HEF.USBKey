using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Pkcs11_Compose : IUSBKeyService_Pkcs11_Compose
    {
        private readonly IDictionary<string, IUSBKeyService_Pkcs11> _usbKeyPkcs11ServiceDict;

        private CancellationTokenSource _monitorSlotEventCts;

        private readonly List<IUSBKey_Pkcs11_Handler_SlotEvent> _usbKeyPkcs11SlotEventHandlers;

        #region Constructor
        public USBKeyService_Pkcs11_Compose(IEnumerable<IUSBKeyService_Pkcs11> usbKeyPkcs11Services)
        {
            if (usbKeyPkcs11Services == null || !usbKeyPkcs11Services.Any())
                throw new ArgumentNullException(nameof(usbKeyPkcs11Services));

            _usbKeyPkcs11ServiceDict = new Dictionary<string, IUSBKeyService_Pkcs11>();
            InitUSBKeyPkcs11Services(usbKeyPkcs11Services);

            _usbKeyPkcs11SlotEventHandlers = new List<IUSBKey_Pkcs11_Handler_SlotEvent>();
        }

        private void InitUSBKeyPkcs11Services(IEnumerable<IUSBKeyService_Pkcs11> usbKeyPkcs11Services)
        {
            foreach (var usbKeyPkcs11Service in usbKeyPkcs11Services)
            {
                AddOrUpdateUSBKeyPkcs11Service(usbKeyPkcs11Service);
            }
        }

        private void AddOrUpdateUSBKeyPkcs11Service(IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            var providerName = usbKeyPkcs11Service.Provider.ProviderName;

            if (_usbKeyPkcs11ServiceDict.ContainsKey(providerName))
            {
                _usbKeyPkcs11ServiceDict[providerName] = usbKeyPkcs11Service;
                return;
            }

            _usbKeyPkcs11ServiceDict.Add(providerName, usbKeyPkcs11Service);
        }
        #endregion

        public IEnumerable<Pkcs11_PresentSlot> GetPresentSlotList()
        {
            foreach (var usbKeyPkcs11Service in _usbKeyPkcs11ServiceDict.Values)
            {
                var presentSlots = usbKeyPkcs11Service.GetPresentSlotList();

                foreach (var presentSlot in presentSlots)
                {
                    yield return new Pkcs11_PresentSlot
                    {
                        ProviderName = usbKeyPkcs11Service.Provider.ProviderName,
                        Slot = presentSlot
                    };
                }
            }
        }

        public IEnumerable<Pkcs11_Certificate_X509> ExportCertificates(string providerName, ulong slotId)
        {
            var usbKeyPkcs11Service = GetMatchUSBKeyPkcs11Service(providerName);

            var slotCerts = usbKeyPkcs11Service.ExportCertificates(slotId);

            foreach (var slotCert in slotCerts)
            {
                yield return slotCert.BuildX509Certificate(usbKeyPkcs11Service);
            }
        }

        public bool ChangeTokenPIN(string providerName, ulong slotId, string oldPin, string newPin)
        {
            var usbKeyPkcs11Service = GetMatchUSBKeyPkcs11Service(providerName);

            return usbKeyPkcs11Service.ChangeTokenPIN(slotId, oldPin, newPin);
        }

        public bool VerifyTokenPIN(string providerName, ulong slotId, string pin)
        {
            var usbKeyPkcs11Service = GetMatchUSBKeyPkcs11Service(providerName);

            return usbKeyPkcs11Service.VerifyTokenPIN(slotId, pin);
        }

        public void StartMonitorSlotEvent()
        {
            if (_monitorSlotEventCts != null && !_monitorSlotEventCts.IsCancellationRequested)
                return;   //监听Slot事件 正在进行，则不做处理

            _monitorSlotEventCts = new CancellationTokenSource();

            var slotInOutEventHandle = BuildSlotInOutEventHandle();

            foreach (var usbKeyPkcs11Service in _usbKeyPkcs11ServiceDict.Values)
            {
                usbKeyPkcs11Service.StartMonitorSlotEvent(slotInOutEventHandle, _monitorSlotEventCts.Token);
            }
        }

        public void AttachSlotEventHandlers(params IUSBKey_Pkcs11_Handler_SlotEvent[] usbKeySlotEventHandlers)
        {
            if (usbKeySlotEventHandlers == null || !usbKeySlotEventHandlers.Any())
                return;

            _usbKeyPkcs11SlotEventHandlers.AddRange(usbKeySlotEventHandlers);
        }

        public void CancelMonitorSlotEvent()
        {
            if (_monitorSlotEventCts != null && !_monitorSlotEventCts.IsCancellationRequested)
                _monitorSlotEventCts.Cancel();
        }

        #region Helper Functions
        protected IUSBKeyService_Pkcs11 GetMatchUSBKeyPkcs11Service(string providerName)
        {
            if (_usbKeyPkcs11ServiceDict.TryGetValue(providerName, out var matchUSBKeyPkcs11Service))
            {
                return matchUSBKeyPkcs11Service;
            }

            throw new InvalidOperationException("not found target provider name usbKey pkcs11 service");
        }
        #endregion

        #region Handle SlotInOutEvent
        protected Action<Pkcs11_SlotInOutEvent> BuildSlotInOutEventHandle()
        {
            return slotInOutEvent =>
            {
                var usbKeyPkcs11Service = GetMatchUSBKeyPkcs11Service(slotInOutEvent.ProviderName);

                foreach (var usbKeySlotEventHandler in _usbKeyPkcs11SlotEventHandlers)
                {
                    usbKeySlotEventHandler.Handle_SlotInOutEvent(slotInOutEvent, usbKeyPkcs11Service);
                }
            };
        }
        #endregion
    }
}
