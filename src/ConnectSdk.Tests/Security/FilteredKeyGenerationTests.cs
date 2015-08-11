using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using ConnectSdk.Config;
using ConnectSdk.Querying;
using ConnectSdk.Security;
using Xunit;

namespace ConnectSdk.Tests.Security
{
    public class FilteredKeyGenerationTests
    {
        public class WhenFilteredKeyIsGenerated
        {
            [Fact]
            public void It_should_encrypt_filter_query()
            {
                var queryToEncrypt = Connect.FilteredKeyQuery()
                    .Where("Prop", "MyProp");
                var keyJson = "{\"filters\":{\"Prop\":{\"eq\":\"MyProp\"}},\"canPush\":false,\"canQuery\":true}";
                var masterKey = "2fMSlDSOGtMWH50wffnCscgGMcJGMQ0s";
                var filteredKey = queryToEncrypt.GenerateFilteredKey(masterKey, new KeySettings(false, true));
                var decryptedQueryJson = Decrypter.Decrypt(masterKey, filteredKey);

                Assert.Equal(keyJson, decryptedQueryJson);
            }

            [Fact]
            public void It_should_encrypt_query()
            {
                var queryToEncrypt = Connect.FilteredKeyQuery();
                var keyJson = "{\"filters\":null,\"canPush\":false,\"canQuery\":true}";
                var masterKey = "2fMSlDSOGtMWH50wffnCscgGMcJGMQ0s";
                var filteredKey = queryToEncrypt.GenerateFilteredKey(masterKey, new KeySettings(false, true));
                var decryptedQueryJson = Decrypter.Decrypt(masterKey, filteredKey);

                Assert.Equal(keyJson, decryptedQueryJson);
            }
        }
    }

    internal static class Decrypter
    {
        public static string Decrypt(string masterKey, string key)
        {
            using (var encryption = GetEncryption())
            {
                var parts = key.Split('-');
                var iv = parts[0].FromHexString();
                var data = parts[1].FromHexString();

                encryption.IV = iv;
                encryption.Key = masterKey.ToBytes();

                using (var decryptor = encryption.CreateDecryptor())
                {
                    var decrypted = decryptor.TransformFinalBlock(data, 0, data.Length);
                    var token = Encoding.UTF8.GetString(decrypted);

                    return token;
                }
            }
        }

        private static SymmetricAlgorithm GetEncryption()
        {
            return new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
        }
    }

    internal static class HexExtensions
    {
        public static byte[] FromHexString(this object value)
        {
            return SoapHexBinary.Parse(value.ToString()).Value;
        }
    }

    internal static class StringExtensions
    {
        public static byte[] ToBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
