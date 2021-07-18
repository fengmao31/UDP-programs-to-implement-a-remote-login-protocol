using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        //random from https://suijimimashengcheng.bmcx.com/
        //static string password = "HAi1I6I6";
        static string hashPassword = "A326A454A8DAC12A91C3006871F29E01839A6FEC";

        //pk,sk from http://web.chacuo.net/netrsakeypair 1024bit PKCS Base64
       static string pk = @"<RSAKeyValue><Modulus>33ae2HAt4hDTimX0LxkcMHfIOnDWF4OMtk37FBW79WWhzqoVZ4z5eQOlHbT/9qPf9LrVRX+f10QyBzkU4BxoClsQvtsIo/j5w4BSKTBcX2fuWcznxRp1ZV1KrUZubL2vBjbx/58Ia7fjisLeM5Zz42l7eCh+hTb3i0W641fx84s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        static string sk = @"<RSAKeyValue><Modulus>33ae2HAt4hDTimX0LxkcMHfIOnDWF4OMtk37FBW79WWhzqoVZ4z5eQOlHbT/9qPf9LrVRX+f10QyBzkU4BxoClsQvtsIo/j5w4BSKTBcX2fuWcznxRp1ZV1KrUZubL2vBjbx/58Ia7fjisLeM5Zz42l7eCh+hTb3i0W641fx84s=</Modulus><Exponent>AQAB</Exponent><P>9g3Z4mg9rKDk+iO6bdc6m/hTAUNXBPYL59XnezqRuo4QoFtW9hREPIhS98vmj/AmS8ifLD2/28YoYp1IVUONww==</P><Q>6H8AiYIH80pnRMTxr7pYU3NTWEHnNWJ0P3zFFxL3uZOkPHIvuAIeJlr/rYkJ1b73iRnJEDle5BnalRum2/8+mQ==</Q><DP>d+H5xdkaK5Dc348m1zulX7bW4men70/eLx/GQAEiXa24Jhk6vnzTXlbhbcBX3suYLRZbj1eqse7TYHDXfEuEtw==</DP><DQ>EtHdAESHUjlOnLF4guoJpk+qinVjOt4KXO1DoveFF/+MhtwTYsjBGge5tJloM2Yu8Wkl7mtGdB5npDRF0H8b2Q==</DQ><InverseQ>wpvQ2Up4QkaIlNjBTom29N9JdLaplOr0YqcPRYk2Qr4xhLbdpMPM7C8FDu+UujqShGsTaWLguArauwvLafAqFw==</InverseQ><D>QW7EeG/wn3950c6kv53EqVSJpsMfjWcRrtgKqwjqLqMBzDf1aIrZCdxNXvN+98Nocoq6pE0IisoHNCI8wJrQw5tiK2N2Kv48xCK9Nshn5ZNsAgvJT+ET1PIGmn4kGymQXb/daiRzWtQu6j+Lz8bZ91jZsNXWHOTGWT0uSUWCiCE=</D></RSAKeyValue>";

        //SHA1 from https://www.convertstring.com/zh_CN/Hash/SHA1
        static string hashPK = "A9F08526BA2C3D9D09DB446E1EF423AD6201CE5A";


        //based on https://www.cnblogs.com/resplendent/p/12817563.html
        static UdpClient udpServer;
        static void Main(string[] args)
        {
            udpServer = new UdpClient(61000);       // 当前服务器使用的端口
            udpServer.Connect("127.0.0.1", 50000); // 与客户端建立连接
            Console.WriteLine("Server is ready......");
   


            #region 开启线程保持通讯

            var t1 = new Thread(ReciveMsg1);
            t1.Start();
            #endregion
        }



        //https://stackoverflow.com/questions/32932679/using-rngcryptoserviceprovider-to-generate-random-string
        public static string RandomString(int length)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = new StringBuilder(length);
            using (var rng = new RNGCryptoServiceProvider())
            {
                int count = (int)Math.Ceiling(Math.Log(alphabet.Length, 2) / 8.0);
     
                int offset = BitConverter.IsLittleEndian ? 0 : sizeof(uint) - count;
                int max = (int)(Math.Pow(2, count * 8) / alphabet.Length) * alphabet.Length;
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (res.Length < length)
                {
                    rng.GetBytes(uintBuffer, offset, count);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    if (num < max)
                    {
                        res.Append(alphabet[(int)(num % alphabet.Length)]);
                    }
                }
            }

            return res.ToString();
        }


        static void SendMsg1(object msg)
        {
    
               
                byte[] sendBytes = Encoding.UTF8.GetBytes(msg.ToString()); // 将消息编码成字符串数组
                udpServer.Send(sendBytes, sendBytes.Length);      // 发送数据报

        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// 
        static void ReciveMsg()
        {

            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 50000); // 远程端点，即发送消息方的端点
            while (true)
            {
                byte[] receiveBytes = udpServer.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                string returnData = Encoding.UTF8.GetString(receiveBytes);     // 解析字节数组，得到原消息
                Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());
            }

        }
        static int mark = 0;
        static Thread t2 = new Thread(SendMsg1);
        static string NA;
        static void ReciveMsg1()
        {

            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 50000); // 远程端点，即发送消息方的端点
            while (true)
            {
                if (mark == 0)
                {
                    byte[] receiveBytes = udpServer.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                    string returnData = Encoding.UTF8.GetString(receiveBytes);     // 解析字节数组，得到原消息
                    Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());

                    string name = returnData;

                    NA = RandomString(16);     // 解析字节数组，得到原消息
                    Console.WriteLine("generate NA:" + NA);

                    string PKNA = pk + "," + NA;
                    Console.WriteLine("send pk,NA to Bob:" + PKNA);

                    byte[] sendBytes = Encoding.UTF8.GetBytes(PKNA); // 将消息编码成字符串数组
                    udpServer.Send(sendBytes, sendBytes.Length);       // 发送数据报

                    mark = 1;
                }
                if (mark == 1)
                {
                    byte[] receiveBytes = udpServer.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                    string returnData11 = Encoding.UTF8.GetString(receiveBytes);     // 解析字节数组，得到原消息
                    Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData11);


                    string privateXml = sk;
                    string content = string.Empty;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(privateXml);
                      byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(returnData11), RSAEncryptionPadding.OaepSHA1);
                        content = Convert.ToBase64String(decryptedData);
                    }
                    Console.WriteLine("Bob RSA plainText OTP:" + content);

                    //string privateXml = sk;
                    //string content = string.Empty;
                    //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    //{
                    //    //rsa.FromXmlString(getAry[0]);
                    //    rsa.FromXmlString(privateXml);
                    //    byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(returnData11), RSAEncryptionPadding.OaepSHA1);
                    //    content = Convert.ToBase64String(decryptedData);
                    //}
                    //Console.WriteLine("Bob RSA plainText OTP:" + content);


                    string PKNA = hashPassword + NA;
                    string OTP = Hash(PKNA);

                    byte[] data = ConvertFromStringToHex(OTP);
                    string base64 = Convert.ToBase64String(data);
                    Console.WriteLine("Alice RSA plainText OTP:" + base64);

                    if(content==base64)
                    {
                        Console.WriteLine("success");
                        t2.Start("success");

                    }
                    else 
                    {
                        Console.WriteLine("fail");
                        t2.Start("fail");
                    }
                }

            }

        }
        //https://maheshkumar.wordpress.com/2014/04/15/how-to-convert-hex-to-base64-in-c/
        static byte[] ConvertFromStringToHex(string inputHex)
        {
            inputHex = inputHex.Replace("-", "");

            byte[] resultantArray = new byte[inputHex.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }
        //https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
        static string Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash))).Replace("-", string.Empty);
            }

        }
        /// <summary>
        /// 发送消息
        /// </summary>
        static void SendMsg()
        {
            while (true)
            {
                var msg = Console.ReadLine().ToString();                // 获取控制台字符串
                byte[] sendBytes = Encoding.UTF8.GetBytes(msg); // 将消息编码成字符串数组
                udpServer.Send(sendBytes, sendBytes.Length);      // 发送数据报
            }
        }
    }

}