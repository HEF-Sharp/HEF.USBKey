using System;

namespace HEF.USBKey.Interop.SKF
{
    public static class SKFResultHelper
    {
        public static SKFResult FromResultCode(uint resultCode)
        {
            if (resultCode < 0)
                throw new ArgumentOutOfRangeException(nameof(resultCode));

            return new SKFResult { Code = resultCode };
        }
    }
}
