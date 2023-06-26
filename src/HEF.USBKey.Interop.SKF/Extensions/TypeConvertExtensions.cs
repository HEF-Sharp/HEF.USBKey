using System;
using System.Text;

namespace HEF.USBKey.Interop.SKF
{
    public static class TypeConvertExtensions
    {
        public static int AsInt32(this uint value)
        {
            return Convert.ToInt32(value);
        }

        public static uint AsUInt32(this int value)
        {
            return Convert.ToUInt32(value);
        }

        public static string ToASCIIString(this byte[] strBytes)
        {
            if (strBytes is null || strBytes.Length == 0)
                throw new ArgumentNullException(nameof(strBytes));

            return Encoding.ASCII.GetString(strBytes);
        }
    }
}
