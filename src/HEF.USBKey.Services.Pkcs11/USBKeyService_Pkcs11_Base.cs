using HEF.USBKey.Common;
using HEF.USBKey.Interop.Pkcs11;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEF.USBKey.Services.Pkcs11
{
    public abstract class USBKeyService_Pkcs11_Base
    {
        private Task _monitorSlotEventTask;

        internal USBKeyService_Pkcs11_Base(IUSBKeyProvider_Pkcs11 usbKeyProvider_Pkcs11)
        {
            Provider = usbKeyProvider_Pkcs11 ?? throw new ArgumentNullException(nameof(usbKeyProvider_Pkcs11));
        }

        public IUSBKeyProvider_Pkcs11 Provider { get; }

        public IEnumerable<ISlot> GetPresentSlotList()
        {
            return Provider.GetSlotList(true);
        }

        public ISlot GetPresentSlotById(ulong slotId)
        {
            return GetPresentSlotList().SingleOrDefault(slot => slot.SlotId == slotId);
        }

        public IEnumerable<Pkcs11_Certificate> ExportCertificates(ulong slotId)
        {
            var slot = GetPresentSlotById(slotId);

            if (slot != null)
            {
                using (var session = slot.OpenSession(SessionType.ReadOnly))
                {
                    var searchTemplate = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509)
                    };

                    foreach (IObjectHandle certHandle in session.FindAllObjects(searchTemplate))
                    {
                        List<IObjectAttribute> objectAttributes = session.GetAttributeValue(certHandle,
                            new List<CKA>() { CKA.CKA_ID, CKA.CKA_LABEL, CKA.CKA_SIGN, CKA.CKA_VALUE });

                        string ckaId = objectAttributes[0].GetValueAsString();
                        string ckaLabel = objectAttributes[1].GetValueAsString();
                        bool ckaSign = objectAttributes[2].CannotBeRead ? false : objectAttributes[2].GetValueAsBool();
                        byte[] ckaValue = objectAttributes[3].GetValueAsByteArray();

                        var numberIndex = ckaId.IndexOf('#');
                        if (numberIndex >= 0)
                            ckaId = ckaId.Substring(0, numberIndex);  //截断末尾 #序号

                        yield return new Pkcs11_Certificate
                        {
                            Id = ckaId,
                            Label = ckaLabel,
                            ForSign = ckaSign,
                            CertBytes = ckaValue
                        };
                    }
                }
            }
        }

        public IEnumerable<Pkcs11_Certificate_X509> ExportX509Certificates(ulong slotId)
        {
            var slotCerts = ExportCertificates(slotId);

            foreach (var slotCert in slotCerts)
            {
                yield return slotCert.BuildX509Certificate(Provider);
            }
        }

        public bool ChangeTokenPIN(ulong slotId, string oldPin, string newPin)
        {
            var slot = GetPresentSlotById(slotId);

            if (slot is null)
                return false;

            using (var session = slot.OpenSession(SessionType.ReadWrite))
            {
                try
                {
                    session.SetPin(oldPin, newPin);
                    return true;
                }
                catch (Pkcs11Exception)
                {
                    return false;
                }
            }
        }

        public bool VerifyTokenPIN(ulong slotId, string pin)
        {
            var slot = GetPresentSlotById(slotId);

            if (slot is null)
                return false;

            using (var session = slot.OpenSession(SessionType.ReadOnly))
            {
                var loginSuccess = false;
                try
                {
                    session.Login(CKU.CKU_USER, pin);
                    loginSuccess = true;
                }
                catch (Pkcs11Exception ex)
                {
                    if (ex.RV == CKR.CKR_PIN_INCORRECT)
                        return false;
                }
                finally
                {
                    if (loginSuccess)
                        session.Logout();
                }

                return true;  //登录返回值不是 CKR_PIN_INCORRECT 均为校验通过
            }
        }

        public void StartMonitorSlotEvent(Action<Pkcs11_SlotInOutEvent> slotEventAction, CancellationToken cancellationToken)
        {
            if (_monitorSlotEventTask != null && _monitorSlotEventTask.Status == TaskStatus.Running)
                return;   //监听Slot事件 任务正在运行 则不做处理

            _monitorSlotEventTask = Task.Factory.StartNew(() => ProcessingWaitSlotEvent(slotEventAction, cancellationToken), TaskCreationOptions.LongRunning);           
        }

        #region MonitorSlotEvent
        protected virtual Task ProcessingWaitSlotEvent(Action<Pkcs11_SlotInOutEvent> slotEventAction, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = Provider.WaitForSlotEvent(out var slot);

                if (result)
                {
                    //先调用GetSlotInfo可以起到刷新的作用，保证后面GetPresentSlot能获取到 实际存在设备插入的Slot
                    var slotInfo = slot.GetSlotInfo();

                    var matchPresentSlot = GetPresentSlotById(slot.SlotId);

                    var slotEvent = new Pkcs11_SlotInOutEvent
                    {
                        ProviderName = Provider.ProviderName,
                        SlotId = slot.SlotId,
                        InOutEventType = matchPresentSlot != null ? DeviceEventTypes.PlugIn : DeviceEventTypes.PullOut
                    };

                    slotEventAction?.Invoke(slotEvent);
                }
            }

            return Task.FromResult(0);
        }
        #endregion
    }
}
