using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Common
{
    public class USBKey_Certificate
    {
        /// <summary>
        /// 是否签名证书
        /// </summary>
        public bool ForSign { get; set; }

        /// <summary>
        /// 证书Bytes
        /// </summary>
        public byte[] CertBytes { get; set; }
    }

    public class USBKey_Certificate_X509 : USBKey_Certificate
    {
        /// <summary>
        /// X509证书
        /// </summary>
        public X509Certificate2 X509Cert { get; set; }
    }
}
