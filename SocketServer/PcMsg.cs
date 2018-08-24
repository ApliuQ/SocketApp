using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendClass
{
    public class PcMsg
    {
        /// <summary>
        /// 是否是发送的聊天消息
        /// </summary>
        public bool notice = true;
        /// <summary>
        /// 信息发送人
        /// </summary>
        public String sendpcname = string.Empty;
        public List<PcName> pcname = new List<PcName>() { };
        public string msg = string.Empty;

        public override String ToString()
        {
            return SerializeObject();
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <param name="Json"></param>
        public static PcMsg DeserializeJosn(string json)
        {
            PcMsg pm = null;
            try
            {
                pm = (PcMsg)JsonConvert.DeserializeObject(json, typeof(PcMsg));
            }
            catch (Exception)
            {
            }
            return pm;
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="Json"></param>
        public string SerializeObject()
        {
            string Json = JsonConvert.SerializeObject(this);
            return Json;
        }

        public DataTable GetPcNameDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("guid");
            dt.Columns.Add("name");
            foreach (PcName pnTemp in pcname)
            {
                DataRow dr = dt.NewRow();
                dr["guid"] = pnTemp.guid;
                dr["name"] = pnTemp.name;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public class PcName
    {
        /// <summary>
        /// 服务端连接列表中的唯一标识
        /// </summary>
        public string guid = string.Empty;
        public string name = string.Empty;
        public PcName() { }
        public PcName(String guid, String name)
        {
            this.guid = guid;
            this.name = name;
        }
    }
}
