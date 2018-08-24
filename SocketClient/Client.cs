using SendClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Client : Form
    {
        private Socket socketClient;
        private Thread thread;
        private String serverip = "192.168.13.64";

        public Client()
        {
            InitializeComponent();
            btnsend.Enabled = false;
            sendtext.Enabled = false;
            btnstop.Enabled = false;
            pcname.Text = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        private void Btnconnect_Click(object sender, EventArgs e)
        {
            try
            {
                serverip = tbserverip.Text.Trim();
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketClient.Connect(serverip, 8009);
                PcMsg pm = new PcMsg
                {
                    sendpcname = pcname.Text,
                    msg = pcname.Text,
                    notice = false
                };
                socketClient.Send(Encoding.UTF8.GetBytes(pm.SerializeObject()));
                TextBox.CheckForIllegalCrossThreadCalls = false;

                thread = new Thread(Recive)
                {
                    IsBackground = true
                };
                thread.Start(socketClient);

                btnsend.Enabled = true;
                sendtext.Enabled = true;
                btnstop.Enabled = true;
                btnconnect.Enabled = false;
                pcname.Enabled = false;
                msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：连接服务成功" + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + ex.Message + System.Environment.NewLine);
            }
        }

        private void Btnstop_Click(object sender, EventArgs e)
        {
            pclist.DataSource = null;
            thread.Abort();
            socketClient.Close();
            btnsend.Enabled = false;
            sendtext.Enabled = false;
            btnstop.Enabled = false;
            btnconnect.Enabled = true;
            pcname.Enabled = true;
        }

        private void Btnsend_Click(object sender, EventArgs e)
        {
            string strsend = sendtext.Text.Trim();
            if (string.IsNullOrEmpty(strsend) || pclist.CheckedItems.Count <= 0)
            {
                MessageBox.Show("消息为空或未选择发送人");
                return;
            }
            PcMsg pm = new PcMsg
            {
                sendpcname = pcname.Text,
                msg = strsend
            };
            for (int i = 0; i < pclist.CheckedItems.Count; i++)
            {
                DataRowView drv = pclist.CheckedItems[i] as DataRowView;
                pm.pcname.Add(new PcName(drv["guid"].ToString(), drv["name"].ToString()));
            }
            byte[] buffter = Encoding.UTF8.GetBytes(pm.SerializeObject());
            try
            {
                int temp = socketClient.Send(buffter);
                //msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + pm.sendpcname + " 说: " + strsend + System.Environment.NewLine);
                sendtext.Clear();
            }
            catch (Exception ex)
            {
                msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + ex.Message + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        public void Recive(object o)
        {
            var send = o as Socket;
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[1024 * 1024 * 2];
                try
                {
                    var effective = send.Receive(buffer);
                    if (effective == 0)
                    {
                        break;
                    }
                    string json = Encoding.UTF8.GetString(buffer, 0, effective);
                    PcMsg pm = PcMsg.DeserializeJosn(json);
                    if (pm.notice)
                    {
                        this.msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + pm.sendpcname + ":" + pm.msg + System.Environment.NewLine);
                    }
                    else
                    {
                        this.pclist.DataSource = pm.GetPcNameDt();
                        this.pclist.DisplayMember = "name";
                        this.pclist.ValueMember = "guid";
                    }
                }
                catch (Exception ex)
                {
                    this.msgtext.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + ex.Message + System.Environment.NewLine);
                    //服务端强制关闭连接
                    if (ex is SocketException skex && skex.ErrorCode == 10054)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Object DeserializeWithBinary(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);

            stream.Close();

            return obj;
        }
    }
}
