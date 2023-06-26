namespace HEF.USBKey.Interop.SKF
{
    public class SKFResult
    {
        public uint Code { get; set; }
    }

    public class SKFResult<TData> : SKFResult
    {
        public TData Data { get; set; }
    }
}
