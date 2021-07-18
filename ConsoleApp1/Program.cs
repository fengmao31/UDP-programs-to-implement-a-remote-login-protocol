using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static string pk = @"<RSAKeyValue><Modulus>33ae2HAt4hDTimX0LxkcMHfIOnDWF4OMtk37FBW79WWhzqoVZ4z5eQOlHbT/9qPf9LrVRX+f10QyBzkU4BxoClsQvtsIo/j5w4BSKTBcX2fuWcznxRp1ZV1KrUZubL2vBjbx/58Ia7fjisLeM5Zz42l7eCh+hTb3i0W641fx84s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        static string sk = @"<RSAKeyValue><Modulus>33ae2HAt4hDTimX0LxkcMHfIOnDWF4OMtk37FBW79WWhzqoVZ4z5eQOlHbT/9qPf9LrVRX+f10QyBzkU4BxoClsQvtsIo/j5w4BSKTBcX2fuWcznxRp1ZV1KrUZubL2vBjbx/58Ia7fjisLeM5Zz42l7eCh+hTb3i0W641fx84s=</Modulus><Exponent>AQAB</Exponent><P>9g3Z4mg9rKDk+iO6bdc6m/hTAUNXBPYL59XnezqRuo4QoFtW9hREPIhS98vmj/AmS8ifLD2/28YoYp1IVUONww==</P><Q>6H8AiYIH80pnRMTxr7pYU3NTWEHnNWJ0P3zFFxL3uZOkPHIvuAIeJlr/rYkJ1b73iRnJEDle5BnalRum2/8+mQ==</Q><DP>d+H5xdkaK5Dc348m1zulX7bW4men70/eLx/GQAEiXa24Jhk6vnzTXlbhbcBX3suYLRZbj1eqse7TYHDXfEuEtw==</DP><DQ>EtHdAESHUjlOnLF4guoJpk+qinVjOt4KXO1DoveFF/+MhtwTYsjBGge5tJloM2Yu8Wkl7mtGdB5npDRF0H8b2Q==</DQ><InverseQ>wpvQ2Up4QkaIlNjBTom29N9JdLaplOr0YqcPRYk2Qr4xhLbdpMPM7C8FDu+UujqShGsTaWLguArauwvLafAqFw==</InverseQ><D>QW7EeG/wn3950c6kv53EqVSJpsMfjWcRrtgKqwjqLqMBzDf1aIrZCdxNXvN+98Nocoq6pE0IisoHNCI8wJrQw5tiK2N2Kv48xCK9Nshn5ZNsAgvJT+ET1PIGmn4kGymQXb/daiRzWtQu6j+Lz8bZ91jZsNXWHOTGWT0uSUWCiCE=</D></RSAKeyValue>";
        //static string OTP = "2C6zP4eE2kLj3ilnKxcxciO/h0U=";
        static string OTP = "7029919FC65B38986BEFF95C0724AB1863612FF3";
        static void Main(string[] args)
        {
            byte[] data = ConvertFromStringToHex(OTP);
            string base64 = Convert.ToBase64String(data);
            Console.WriteLine("RSA plainText:" + base64);

            string publicXml = pk;
            string encryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //rsa.FromXmlString(getAry[0]);
                rsa.FromXmlString(publicXml);
                byte[] encryptedData = rsa.Encrypt(Convert.FromBase64String(base64), RSAEncryptionPadding.OaepSHA1);
                encryptedContent = Convert.ToBase64String(encryptedData);
                
            }
            Console.WriteLine("RSA crypteText:" + encryptedContent);


            string privateXml = @"<RSAKeyValue><Modulus>33ae2HAt4hDTimX0LxkcMHfIOnDWF4OMtk37FBW79WWhzqoVZ4z5eQOlHbT/9qPf9LrVRX+f10QyBzkU4BxoClsQvtsIo/j5w4BSKTBcX2fuWcznxRp1ZV1KrUZubL2vBjbx/58Ia7fjisLeM5Zz42l7eCh+hTb3i0W641fx84s=</Modulus><Exponent>AQAB</Exponent><P>9g3Z4mg9rKDk+iO6bdc6m/hTAUNXBPYL59XnezqRuo4QoFtW9hREPIhS98vmj/AmS8ifLD2/28YoYp1IVUONww==</P><Q>6H8AiYIH80pnRMTxr7pYU3NTWEHnNWJ0P3zFFxL3uZOkPHIvuAIeJlr/rYkJ1b73iRnJEDle5BnalRum2/8+mQ==</Q><DP>d+H5xdkaK5Dc348m1zulX7bW4men70/eLx/GQAEiXa24Jhk6vnzTXlbhbcBX3suYLRZbj1eqse7TYHDXfEuEtw==</DP><DQ>EtHdAESHUjlOnLF4guoJpk+qinVjOt4KXO1DoveFF/+MhtwTYsjBGge5tJloM2Yu8Wkl7mtGdB5npDRF0H8b2Q==</DQ><InverseQ>wpvQ2Up4QkaIlNjBTom29N9JdLaplOr0YqcPRYk2Qr4xhLbdpMPM7C8FDu+UujqShGsTaWLguArauwvLafAqFw==</InverseQ><D>QW7EeG/wn3950c6kv53EqVSJpsMfjWcRrtgKqwjqLqMBzDf1aIrZCdxNXvN+98Nocoq6pE0IisoHNCI8wJrQw5tiK2N2Kv48xCK9Nshn5ZNsAgvJT+ET1PIGmn4kGymQXb/daiRzWtQu6j+Lz8bZ91jZsNXWHOTGWT0uSUWCiCE=</D></RSAKeyValue>";
            string content = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //rsa.FromXmlString(getAry[0]);
                rsa.FromXmlString(privateXml);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedContent), RSAEncryptionPadding.OaepSHA1);
                content = Convert.ToBase64String(decryptedData);
            }
            Console.WriteLine("RSA plainText:" + content);
        }

        public static byte[] ConvertFromStringToHex(string inputHex)
        {
            inputHex = inputHex.Replace("-", "");

            byte[] resultantArray = new byte[inputHex.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }
    }
}
