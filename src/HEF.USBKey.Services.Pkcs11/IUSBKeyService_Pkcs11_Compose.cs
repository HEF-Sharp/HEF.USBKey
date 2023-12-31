﻿using System.Collections.Generic;

namespace HEF.USBKey.Services.Pkcs11
{
    public interface IUSBKeyService_Pkcs11_Compose
    {
        IEnumerable<Pkcs11_PresentSlot> GetPresentSlotList();

        IEnumerable<Pkcs11_Certificate_X509> ExportCertificates(string providerName, ulong slotId);
    }
}
