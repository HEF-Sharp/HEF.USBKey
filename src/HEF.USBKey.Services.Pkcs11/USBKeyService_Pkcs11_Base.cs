using HEF.USBKey.Interop.Pkcs11;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HEF.USBKey.Services.Pkcs11
{
    public abstract class USBKeyService_Pkcs11_Base
    {
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

                        ckaId = ckaId.Substring(0, ckaId.Length - 2);  //截断末尾 #序号

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
    }
}
