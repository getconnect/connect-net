using System;
using ConnectSdk.Querying;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLCrypto;

namespace ConnectSdk.Security
{
    public static class FilteredKeyGeneration
    {
        public static string GenerateFilteredKey(string keyJson, string masterKey)
        {
            var aesProvider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var masterKeyBytes = masterKey.AsBytes();
            var queryJsonBytes = keyJson.AsBytes();

            var masterSymmetricKey = aesProvider.CreateSymmetricKey(masterKeyBytes);
            var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(16);
            var encryptedQueryJson = WinRTCrypto.CryptographicEngine.Encrypt(masterSymmetricKey, queryJsonBytes, iv);

            return $"{iv.AsHex()}-{encryptedQueryJson.AsHex()}";
        }

        public static string GenerateFilteredKey<TResult>(this IQuery<TResult> query, string masterKey, KeySettings setting)
        {
            var queryJson = query.ToString();
            var keyJObject = JObject.Parse(queryJson);
            keyJObject["canPush"] = setting.CanPush;
            keyJObject["canQuery"] = setting.CanQuery;
            return GenerateFilteredKey(keyJObject.ToString(Formatting.None), masterKey);
        }

        private static byte[] AsBytes(this string @string)
        {
            return System.Text.Encoding.UTF8.GetBytes(@string);
        }

        private static string AsHex(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}