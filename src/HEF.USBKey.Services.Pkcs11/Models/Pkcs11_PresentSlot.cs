using Net.Pkcs11Interop.HighLevelAPI;

namespace HEF.USBKey.Services.Pkcs11
{
    public class Pkcs11_PresentSlot
    {
        public string ProviderName { get; set; }

        public ISlot Slot { get; set; }
    }
}
