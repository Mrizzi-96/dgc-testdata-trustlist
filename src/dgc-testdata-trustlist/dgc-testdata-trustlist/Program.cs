using dgc_testdata_trustlist.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dgc_testdata_trustlist
{
    class Program
    {
        static void Main(string[] args)
        {
            //istantiate trustlist & other variables
            DSC_TL trustList = new DSC_TL();
            trustList.DscTrustList = new Dictionary<string, DscTrust>();

            // path to your assets folder
            var path = @"..\..\..\..\..\..\assets\dgc-testdata";
            var certRegex = new Regex(@"""CERTIFICATE""\s*:\s*""([^""]*)""");
            IEnumerable<string> files;

            // find json files
            try
            {
                files = Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to read files inside assets directory", e);
            }
            if (files.Count().Equals(0))
            {
                throw new Exception($"Error: no files were found inside directory: {path}");
            }
            // loop thorugh files
            foreach (var file in files)
            {

                // skip excluded folders
                if (file.Contains(@"\test\") || file.Contains(@"\common\"))
                {
                    continue;
                }

                // load file contents
                var content = string.Empty;
                try
                {
                    content = File.ReadAllText(file);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unable to read contents of file {file}", ex);
                }

                // extract X509 data
                var m = certRegex.Match(content);
                if (m.Success)
                {
                    var cert = m.Groups[1].Value;
                    var jwk = new Key();
                    try
                    {
                        // create Key from certificate
                        var byteData = Encoding.ASCII.GetBytes(cert);
                        jwk = Key.LoadFromX509(byteData);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error: unable to load certificate from file {file}", e);
                    }

                    // extract country code from filename
                    var cc = file.Split("\\")[8];

                    // check if cc already exists
                    if (!trustList.DscTrustList.ContainsKey(cc))
                    {
                        // if not, create a new one.
                        trustList.DscTrustList.Add(cc, new DscTrust());
                    }

                    // get the country DSC list (maybe it's empty, since could be just created)
                    var countryDscList = new DscTrust();
                    if (trustList.DscTrustList.TryGetValue(cc, out countryDscList))
                    {
                        // prepare a list of key, preserving existing ones
                        var keys = new List<Key>();
                        if (countryDscList.Keys != null)
                        {
                            keys.AddRange(countryDscList.Keys);
                        }

                        // append the new key
                        keys.Add(jwk);
                        countryDscList.Keys = keys.ToArray();
                    }
                    else
                    {
                        // cannot get the country DSC list! argh, something went wrong
                        throw new Exception($"Cannot find nor create DSC trust list for country {cc}");
                    }
                }
            }

            // set issuer
            trustList.Iss = "dgc-testdata";

            // set issue date
            var now = DateTimeOffset.Now;
            trustList.Iat = now.ToUnixTimeSeconds();

            // set expiration date
            trustList.Exp = new DateTimeOffset(new DateTime(2022,4,16)).ToUnixTimeSeconds();

            // save trustlist into a json file
            var jsonTL = DSC_TLSerialize.ToJson(trustList);
            File.WriteAllText("cached_trust_list.json", jsonTL);
            Console.WriteLine("All test files parsed. Trustlist built successfully.");
        }
    }
}
