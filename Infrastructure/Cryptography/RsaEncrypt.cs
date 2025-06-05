
using Infrastructure.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Cryptography;

public class RsaEncrypt
{
    public string RsaPublicKey { get; set; }

    public RsaEncrypt(string rsa)
    {
        RsaPublicKey = rsa;
    }

    public byte[] RSAEncryptMethod(byte[] byteEncrypt, string publicXml, bool isOAEP)
    {
        byte[] encryptedData;
        if (isOAEP)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                RsaServiceProviderExtensions.FromXmlString(rsa, publicXml);
                encryptedData = rsa.Encrypt(byteEncrypt, isOAEP);
            }
        }
        else
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                RsaServiceProviderExtensions.FromXmlString(rsa, publicXml);
                encryptedData = rsa.Encrypt(byteEncrypt, isOAEP);
            }
        }

        return encryptedData;
    }
    public string Encrypt(string toEncryptData, bool oaep = false, bool isUnicode = false)
    {
        if (toEncryptData.IsNullOrEmpty())
            return string.Empty;

        if (isUnicode)
        {
            UnicodeEncoding encoding = new();
            return Convert.ToBase64String(RSAEncryptMethod(encoding.GetBytes(toEncryptData), RsaPublicKey, oaep));
        }
        else
        {
            return Convert.ToBase64String(RSAEncryptMethod(Encoding.UTF8.GetBytes(toEncryptData), RsaPublicKey, oaep));
        }
    }
}