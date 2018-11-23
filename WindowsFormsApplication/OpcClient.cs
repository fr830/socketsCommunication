using ClientConnOpcServer;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class OpcClient : Form
    {
        public OpcClient()
        {
            InitializeComponent();
        }

        List<string> GetPointBF = new List<string>()
        {
            "BF1.Input.PFlag",
            "BF1.DB.Pre",
            "BF1.DB.PreMax",
            "BF1.DB.PreMin",
            "BF1.Input.WFlag",
            "BF1.DB.Wei",
            "BF1.DB.WeiMax",
            "BF1.DB.WeiMin"
        };
        string strHostIP, strHostName;
        _ClientConnOpcServer opc = null;
        Result<OPCServer> result = null;
        private void button1_Click(object sender, EventArgs e)
        {
           
                opc = new _ClientConnOpcServer(SynchronizationContext.Current);
              //  string[] a = opc.GetLoccalIpAndHostName();
                opc.TelegrammRecieved += opc_TelegrammRecieved;
                result = opc.CreateConnection("KepWare.KEPServerEX.V6", "10.3.10.100", 3);
                if (!result.IsSuccess) { MessageBox.Show("连接失败!"); }
                IList<string> list = ConfigPoint();
                opc.AddConnectionPoint(GetPointBF, result.Content, 200);
       
            
        }

        void opc_TelegrammRecieved(Dictionary<string, string> resultList)
        {
            textBox1.Text = "";
            foreach (KeyValuePair<string, string> pair in resultList)
            {
                RefeshTextBox("键为:" + pair.Key + " 的值为：" + pair.Value, textBox1);
            }

        }
        private void RefeshTextBox(string msg, TextBox t)
        {

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, TextBox>(RefeshTextBox), msg, t);
                return;
            }
            t.AppendText(msg + Environment.NewLine);
        }

        public IList<string> ConfigPoint()
        {
            IList<string> list = new List<string>();
            //list.Add("模拟器示例.函数.Ramp1");
            //list.Add("模拟器示例.函数.Ramp2");
            //list.Add("模拟器示例.函数.Ramp3");
            //list.Add("模拟器示例.函数.Ramp4");
            //list.Add("模拟器示例.函数.Random1");

            list.Add("BF1.Input.PFlag");
            list.Add("BF1.DB.Pre");
            list.Add("BF1.DB.PreMax");
            list.Add("BF1.DB.PreMin");
            list.Add("BF1.Input.WFlag");
            list.Add("BF1.DB.Wei");
            list.Add("BF1.DB.WeiMax");
            list.Add("BF1.DB.WeiMin");
            //list.Add("模拟器示例.函数.Random1");
            return list;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            opc.CloseOpcServer(result.Content);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

    }
}
