using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;


namespace Client
{
    class Program
    {
        //pk,sk from http://web.chacuo.net/netrsakeypair 1024bit PKCS Base64
        static string password = "HAi1I6I6";
        //SHA1 from https://www.convertstring.com/zh_CN/Hash/SHA1
        static string hashPK = "A9F08526BA2C3D9D09DB446E1EF423AD6201CE5A";


        //based on https://www.cnblogs.com/resplendent/p/12817563.html
        static UdpClient udpClient;

           static Thread t1 = new Thread(SendMsg1);
        static Thread t2 = new Thread(ReciveMsg1);
        static void Main(string[] args)
        {
            udpClient = new UdpClient(50000);       // 当前客户端使用的端口
            udpClient.Connect("127.0.0.1", 61000); // 与服务器建立连接
            Console.WriteLine("Client is ready......");

            Console.WriteLine("please input name. (Server must be ready)");
            var name = Console.ReadLine().ToString();           // 获取控制台字符串

            #region 开启线程保持通讯

            t1.Start(name);

            t2.Start();

            #endregion
        }
        static void SendMsg1(object msg)
        {

                byte[] sendBytes = Encoding.UTF8.GetBytes(msg.ToString()); // 将消息编码成字符串数组
                udpClient.Send(sendBytes, sendBytes.Length);       // 发送数据报

        }
       static int mark = 0;
        static void ReciveMsg1()
        {
            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 61000); // 远程端点，即发送消息方的端点
            while (true)
            {
                if (mark == 0)
                {
                    byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                    string returnData = Encoding.UTF8.GetString(receiveBytes);      // 解析字节数组，得到原消息
                    Console.WriteLine("get pk，NA");
                    Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());
                    string[] getAry = returnData.Split(new char[] { ',' });

                    string thisHashPK = Hash(getAry[0]);
                    Console.WriteLine("hash pk:" + thisHashPK);

                    Console.WriteLine("match pk");
                    if (thisHashPK != hashPK)
                    { Console.WriteLine("pk is wrong"); }

                    string PKNA = Hash(password) + getAry[1];
                    string OTP = Hash(PKNA);
                    Console.WriteLine("OTP:" + OTP);

                    byte[] data = ConvertFromStringToHex(OTP);
                    string base64 = Convert.ToBase64String(data);
                    Console.WriteLine("RSA plainText:" + base64);


       
                    string publicXml = getAry[0];
                    string encryptedContent = string.Empty;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        //rsa.FromXmlString(getAry[0]);
                        rsa.FromXmlString(publicXml);
                        byte[] encryptedData = rsa.Encrypt(Convert.FromBase64String(base64), RSAEncryptionPadding.OaepSHA1);
                        encryptedContent = Convert.ToBase64String(encryptedData);

                    }
                    Console.WriteLine("RSA crypteText:" + encryptedContent);





                    byte[] sendBytes = Encoding.UTF8.GetBytes(encryptedContent); // 将消息编码成字符串数组
                    udpClient.Send(sendBytes, sendBytes.Length);       // 发送数据报

                    mark = 1;
                }
                if (mark == 1)
                {
                    byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                    string returnData = Encoding.UTF8.GetString(receiveBytes);      // 解析字节数组，得到原消息
                    Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());

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
        }








        /// <summary>
        /// 发送消息
        /// </summary>
        static void SendMsg()
        {
            while (true)
            {
                var msg = Console.ReadLine().ToString();        　　 // 获取控制台字符串
                byte[] sendBytes = Encoding.UTF8.GetBytes(msg); // 将消息编码成字符串数组
                udpClient.Send(sendBytes, sendBytes.Length);       // 发送数据报
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        static void ReciveMsg()
        {
            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 61000); // 远程端点，即发送消息方的端点
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                string returnData = Encoding.UTF8.GetString(receiveBytes);      // 解析字节数组，得到原消息
                Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());
            }

        }
    }
}
