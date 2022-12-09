using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public abstract class AuthenticationProvider
    {
        public ApiCredentials ApiCredentials { get; }
        protected byte[] secretBytes;
        public AuthenticationProvider(ApiCredentials apiCredentials)
        {
            if (ApiCredentials!.ApiSecret == null) throw new ArgumentException("No ApiScret is set. Please set an api secret key first");
            ApiCredentials = apiCredentials;
            secretBytes = Encoding.UTF8.GetBytes(apiCredentials.ApiSecret);
        }

        public abstract IRequest AuthenticateRequest(IRequest request);

        protected static string GetSha256String(string data, SignOutputTypes? outputType = null)
        {
            using var encryptor = SHA256.Create();
            var resultBytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(data));
            return outputType == SignOutputTypes.Base64 ? BytesToBase64String(resultBytes) : BytesToHexString(resultBytes);
        }
        protected static byte[] GetSha256Bytes(string data)
        {
            using var encoder = SHA256.Create();
            return encoder.ComputeHash(Encoding.UTF8.GetBytes(data));
        }


        protected static string BytesToHexString(byte[] resultBytes)
        {
            var result = string.Empty;
            foreach (byte b in resultBytes)
            {
                result += b.ToString("X2");
            }
            return result;
        }

        protected static string BytesToBase64String(byte[] resultBytes)
        {
            return Convert.ToBase64String(resultBytes);
        }



    }
}
