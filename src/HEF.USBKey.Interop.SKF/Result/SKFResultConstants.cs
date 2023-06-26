namespace HEF.USBKey.Interop.SKF
{
    public class SKFResultConstants
    {
        public static SKFResult Success = new SKFResult { Code = SKFResultCodes.SAR_OK };
    }

    public class SKFResultCodes
    {
        public const uint SAR_OK = 0x00000000;

        public const uint SAR_Fail = 0x0A000001;

        public const uint SAR_UNKNOWNERR = 0x0A000002;   //异常错误

        public const uint SAR_NOTSUPPORTYETERR = 0x0A000003;   //不支持的服务

        public const uint SAR_FILEERR = 0x0A000004;   //文件操作错误

        public const uint SAR_INVALIDHANDLEERR = 0x0A000005;   //无效的句柄

        public const uint SAR_INVALIDPARAMERR = 0x0A00BBBB;   //无效的参数
    }
}
