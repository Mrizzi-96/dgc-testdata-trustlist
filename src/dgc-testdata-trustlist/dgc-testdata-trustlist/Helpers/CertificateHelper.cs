using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dgc_testdata_trustlist.Helpers
{
    public static class CertificateHelper
    {
        public static string GetCountryFromCertificateRawData(byte[] rawData)
        {
            if (rawData.Length <= 0) return null;
            var certificate = new X509Certificate2(rawData, string.Empty, X509KeyStorageFlags.Exportable);

            // 
            var issuer = certificate.Issuer;

            // parse string O=BMSGPK, C=AT, CN=AT DGC CSCA 1
            var partsRegex = new Regex("([A-Z]+)=([^,]+)");
            var mm = partsRegex.Matches(issuer);

            foreach (Match m in mm)
            {
                if (m.Success && m.Groups.Count==3 && m.Groups[1].Value== "C")
                {
                    return m.Groups[2].Value;
                }
            }

            return string.Empty;

        }
    }
}
