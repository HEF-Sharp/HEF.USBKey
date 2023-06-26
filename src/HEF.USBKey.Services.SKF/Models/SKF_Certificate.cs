using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.SKF
{
    public class SKF_Certificate
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

    public class SKF_Certificate_X509 : SKF_Certificate
    {
        /// <summary>
        /// X509证书
        /// </summary>
        public X509Certificate2 X509Cert { get; set; }
    }
}
