using SendClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    public class GuidSocket
    {
        public string name;
        public Socket socket;
        public GuidSocket() { }
        public GuidSocket(string name, Socket socket)
        {
            this.name = name;
            this.socket = socket;
        }
    }

    class Program
    {
        public static int Port = 8009;

        static Dictionary<String, GuidSocket> skAll = new Dictionary<string, GuidSocket>() { };

        static void Main(string[] args)
        {
            try
            {
                LoadResoureDll.RegistDLL();
                Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, Port);
                sk.Bind(iep);

                //设置同时连接个数
                sk.Listen(10);

                Thread thread = new Thread(Listen)
                {
                    IsBackground = true
                };
                thread.Start(sk);
                Console.WriteLine("服务启动成功");
                Console.WriteLine("当前服务器IP: " + GetLocalIP() + " 运行IP: " + IPAddress.Any + ", 端口" + Port);

            }
            catch (Exception ex)
            {
                Console.WriteLine("服务启动失败，" + ex.Message);
            }
            Console.Read();
        }

        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="o"></param>
        static void Listen(object o)
        {
            Socket serverSocket = o as Socket;
            while (true)
            {
                try
                {
                    //等待连接并且创建一个负责通讯的socket
                    Socket con = serverSocket.Accept();

                    //获取连接的PcName
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    con.ReceiveTimeout = 1000;
                    int effective = con.Receive(buffer);
                    string json = string.Empty;
                    if (effective > 0)
                    {
                        json = Encoding.UTF8.GetString(buffer, 0, effective);
                    }
                    PcMsg pm = PcMsg.DeserializeJosn(json);
                    if (pm == null || pm.notice)
                    {
                        //con.Close();
                        continue;
                    }
                    string pcname = pm.msg;
                    //将该Pc加入到连接列表中
                    String guid = Guid.NewGuid().ToString();
                    skAll.Add(guid, new GuidSocket(pcname, con));
                    Updatepc();

                    //获取链接的IP地址
                    String sendIpoint = con.RemoteEndPoint.ToString();
                    Console.WriteLine("已连接客户端：" + pcname + ", 地址: " + sendIpoint);

                    //开启一个新线程不停接收消息
                    Thread thread = new Thread(Recive)
                    {
                        IsBackground = true
                    };
                    thread.Start(guid);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Listen错误：" + ex.Message);

                    //服务端强制关闭连接
                    if (ex is SocketException skex && skex.ErrorCode == 10054)
                    {
                        //return;
                    }
                }
            }
        }

        [Obsolete]
        private static void SocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                //获取连接的PcName
                byte[] buffer = e.Buffer;
                int effective = buffer.Length;
                string json = string.Empty;
                if (effective > 0)
                {
                    json = Encoding.UTF8.GetString(buffer, 0, effective);
                }
                PcMsg pm = PcMsg.DeserializeJosn(json);
                if (pm == null || pm.notice)
                {
                    return;
                }
                string pcname = pm.msg;
                //将该Pc加入到连接列表中
                String guid = Guid.NewGuid().ToString();
                skAll.Add(guid, new GuidSocket(pcname, e.AcceptSocket));
                Updatepc();

                //获取链接的IP地址
                String sendIpoint = e.AcceptSocket.RemoteEndPoint.ToString();
                Console.WriteLine("已连接客户端：" + pcname + ", 地址: " + sendIpoint);

                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive)
                {
                    IsBackground = true
                };
                thread.Start(guid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Listen错误：" + ex.Message);

                //服务端强制关闭连接
                if (ex is SocketException skex && skex.ErrorCode == 10054)
                {
                    //return;
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        static void Recive(object o)
        {
            String guid = o as String;
            while (true)
            {
                try
                {
                    //获取发送过来的消息容器
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    skAll[guid].socket.ReceiveTimeout = 0;
                    int effective = skAll[guid].socket.Receive(buffer);
                    //有效字节为0则跳过
                    if (effective == 0)
                    {
                        break;
                    }
                    String json = Encoding.UTF8.GetString(buffer, 0, effective);
                    PcMsg pm = PcMsg.DeserializeJosn(json);
                    if (pm == null)
                    {
                        Console.WriteLine("用户非法访问: " + skAll[guid].socket.RemoteEndPoint);
                        skAll[guid].socket.Close();
                        skAll.Remove(guid);
                        break;
                    }
                    if (pm.notice)
                    {
                        string nexuser = string.Empty;
                        pm.sendpcname = skAll[guid].name;
                        foreach (PcName pnTemp in pm.pcname)
                        {
                            skAll[pnTemp.guid].socket.Send(Encoding.UTF8.GetBytes(pm.SerializeObject()));
                            nexuser += (string.IsNullOrEmpty(nexuser) ? "" : "、") + pnTemp.name;
                        }
                        Console.WriteLine("用户" + skAll[guid].name + "向" + nexuser + " 发送了消息：" + pm.msg);
                    }
                    else Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    //服务端强制关闭连接
                    if (ex is SocketException skex && skex.ErrorCode == 10054)
                    {
                        Console.WriteLine("Recive 客户端: " + skAll[guid].name + ", 地址: " + skAll[guid].socket.RemoteEndPoint + "已经退出" + ", " + ex.Message);
                    }
                    else
                    {
                        Console.WriteLine("Recive 错误：" + skAll[guid].socket.RemoteEndPoint + ", " + ex.Message);
                    }

                    skAll.Remove(guid);
                    Updatepc();
                    return;
                }
            }
        }

        /// <summary>
        /// 通知所有客户端更改当前链接人数
        /// </summary>
        /// <param name="guid"></param>
        private static void Updatepc()
        {
            List<PcMsg> lpm = new List<PcMsg>() { };
            PcMsg pcm = new PcMsg
            {
                notice = false
            };
            foreach (KeyValuePair<String, GuidSocket> skTemp in skAll)
            {
                pcm.pcname.Add(new PcName(skTemp.Key, skTemp.Value.name));
            }

            //并将该连接列表通知到所有客户端
            foreach (KeyValuePair<String, GuidSocket> skTemp in skAll)
            {
                skTemp.Value.socket.Send(Encoding.UTF8.GetBytes(pcm.SerializeObject()));
            }
        }

        /// <summary>  
        /// 获取当前使用的IP  
        /// </summary>  
        /// <returns></returns>  
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "0.0.0.0";
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }
        }

    }
}
