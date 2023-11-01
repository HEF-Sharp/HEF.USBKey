using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.Pkcs11
{
    public class Pkcs11_Certificate
    {
        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否签名证书
        /// </summary>
        public bool ForSign { get; set; }

        /// <summary>
        /// 证书Bytes
        /// </summary>
        public byte[] CertBytes { get; set; }
    }

    public class Pkcs11_Certificate_X509 : Pkcs11_Certificate
    {
        /// <summary>
        /// X509证书
        /// </summary>
        public X509Certificate2 X509Cert { get; set; }
    }
}
