using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace _CommunicationPLC
{
   public  interface ICommunication_Instace
    {
        /// <summary>
        /// 网口地址
        /// </summary>
        string IP { get; set; }

        /// <summary>
        /// cpu类型
        /// </summary>
        Enum_Communcation.PLCType Cpu { get; set; }

        /// <summary>
        /// 检测网络连接状态
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// 打开连接
        /// </summary>
        Result<Socket> Connecetion(string ip,int TimeOut);

        Result Send(Socket socket, byte[] data);

        Result<byte[]> Receive(Socket socket,int Length);
    }
}
