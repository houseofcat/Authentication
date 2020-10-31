using System;
using System.Security.Cryptography.X509Certificates;

namespace Utils
{
    public static class X509
    {
        public static X509Certificate2 GetCertificateByIssuer(string issuer)
        {
            using var certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly);

            var certCollection = certStore.Certificates.Find(X509FindType.FindByIssuerName, issuer, false);
            if (certCollection.Count < 1) { throw new InvalidOperationException($"Issuer ({issuer}) was not found."); }

            return certCollection[0];
        }
    }
}
