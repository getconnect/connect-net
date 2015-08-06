using System;
using ConnectSdk.Querying;
using PCLCrypto;

namespace ConnectSdk.Security
{
    public static class FilteredKeyGeneration
    {
        public static string GenerateFilteredKey<TResult>(this IQuery<TResult> query, string masterKey)
        {
            var aesProvider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var masterKeyBytes = System.Text.Encoding.UTF8.GetBytes(masterKey);
            var queryJsonBytes = System.Text.Encoding.UTF8.GetBytes(query.ToString());

            var masterSymmetricKey = aesProvider.CreateSymmetricKey(masterKeyBytes);
            var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(16);
            var encryptedQueryJson = WinRTCrypto.CryptographicEngine.Encrypt(masterSymmetricKey, queryJsonBytes, iv);

            return $"{BitConverter.ToString(iv).Replace("-", "")}-{BitConverter.ToString(encryptedQueryJson).Replace("-", "")}";
        }
    }
}