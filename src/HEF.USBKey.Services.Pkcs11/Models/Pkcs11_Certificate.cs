using HEF.USBKey.Common;

namespace HEF.USBKey.Services.Pkcs11
{
    public class Pkcs11_Certificate : USBKey_Certificate
    {
        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Label { get; set; }
    }

    public class Pkcs11_Certificate_X509 : USBKey_Certificate_X509
    {
        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Csp ContainerName
        /// </summary>
        public string Label { get; set; }
    }
}
