namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKeyService_SKF_LocalCertSync
    {
        void StartSyncUSBKeysCertsToLocalCurrentUser();

        void CancelSyncUSBKeysCertsToLocalCurrentUser();
    }
}
