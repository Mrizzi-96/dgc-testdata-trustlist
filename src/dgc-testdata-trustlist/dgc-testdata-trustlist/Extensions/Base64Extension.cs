﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dgc_testdata_trustlist.Extensions
{
    public static class Base64Extension
    {
        public static string ToBase64Url(this byte[] data)
        {
            return Convert.ToBase64String(data)
                .Replace("+", "-")
                .Replace("/", "_");
        }
        public static byte[] FromBase64Url(this string input)
        {

            return Convert.FromBase64String(
                input.Replace("-", "+")
                .Replace("_", "/"));
        }
    }
}
