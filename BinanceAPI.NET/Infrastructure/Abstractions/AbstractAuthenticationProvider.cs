using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    /// <summary>
    /// Base class for the authentication provider that holds credentials that will be used to sign any HTTP request if injected into the client.
    /// </summary>
    public abstract class AuthenticationProvider
    {
        public ApiCredentials ApiCredentials { get; }
        protected byte[] secretBytes;
        public AuthenticationProvider(ApiCredentials apiCredentials)
        {
            if (ApiCredentials!.ApiSecret == null) throw new ArgumentException("No ApiSecret is set. Please set an api secret key first");
            ApiCredentials = apiCredentials;
            secretBytes = Encoding.UTF8.GetBytes(apiCredentials.ApiSecret);
        }

        /// <summary>
        /// The method that authenticates a request and returns it back.
        /// </summary>
        /// <param name="request">The request to be authenticated.</param>
        /// <returns></returns>
        public abstract IRequest AuthenticateRequest(IRequest request);

        /// <summary>
        /// Method used to compute SHA256 string of a given string.
        /// </summary>
        /// <param name="data">The string to be hashed.</param>
        /// <param name="outputType">Base64 or HEX</param>
        /// <returns></returns>
        protected static string GetSha256String(string data, SignOutputType? outputType = null)
        {
            using var encryptor = SHA256.Create();
            var resultBytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(data));
            return outputType == SignOutputType.Base64 ? BytesToBase64String(resultBytes) : BytesToHexString(resultBytes);
        }
        /// <summary>
        /// Method used to compute SHA256 bytes of a given string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected static byte[] GetSha256Bytes(string data)
        {
            using var encoder = SHA256.Create();
            return encoder.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Method used to convert bytes to HEX string.
        /// </summary>
        /// <param name="resultBytes"></param>
        /// <returns></returns>
        protected static string BytesToHexString(byte[] resultBytes)
        {
            var result = string.Empty;
            foreach (byte b in resultBytes)
            {
                result += b.ToString("X2");
            }
            return result;
        }
        /// <summary>
        /// Method used to convert bytes to Base64 string.
        /// </summary>
        /// <param name="resultBytes"></param>
        /// <returns></returns>
        protected static string BytesToBase64String(byte[] resultBytes)
        {
            return Convert.ToBase64String(resultBytes);
        }



    }
}
