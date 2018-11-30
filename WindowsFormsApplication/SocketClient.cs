using _CommunicationPLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class SocketClient : Form
    {
        public SocketClient()
        {
            cs = new Communication_Instace(Enum_Communcation.PLCType.S7200);
            InitializeComponent();
        }
        Socket socket = null;
        Communication_Instace cs = null;
        private void button2_Click(object sender, EventArgs e)
        {
            string ip = "127.0.0.1";
          
            socket = cs.Connecetion(ip, 5000).Content;
            bool c = cs.IsAvailable;
        }

    

        private void button1_Click(object sender, EventArgs e)
        {
         //   modbus协议  测试modbus地址2537 读取1个字
            byte[] temp = new byte[]
            {
                0x00,
                0x04,
                0x00,
                0x00,
                0x00,
                0x06,//数据长度
                0x01,//站号
                0x03,//功能码
                0x09,//起始地址Hi
                0xE9,//起始地址Lo
                0x00,//读取字节偏移地址
                0x01 //读取偏移地址lo
            };
            cs.Send(socket, temp);


        }
    }
}
