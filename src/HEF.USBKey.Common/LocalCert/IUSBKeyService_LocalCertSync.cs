namespace HEF.USBKey.Common
{
    public interface IUSBKeyService_LocalCertSync
    {
        void StartSyncUSBKeysCertsToLocalCurrentUser();

        void CancelSyncUSBKeysCertsToLocalCurrentUser();
    }
}
