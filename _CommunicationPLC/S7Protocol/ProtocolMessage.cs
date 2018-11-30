using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _CommunicationPLC
{
   public static  class ProtocolMessage
    {
        /// <summary>
        /// 第一次发送报文
        /// </summary>
        public static byte[] FirstSend { get
            {
                return new byte[]
                {
                    3,
                    0,
                    0,
                    22,
                    17,
                    224,
                    0,
                    0,
                    0,
                    46,
                    0,
                    193,
                    2,
                    1,
                    0,
                    194,
                    2,
                    3,
                    0,
                    192,
                    1,
                    9
                };
            }
        }

        /// <summary>
        /// 第二次发送报文
        /// </summary>
        public static byte[] SencondSend { get
            {
                return new byte[]
                {
                    3,
                    0,
                    0,
                    25,
                    2,
                    240,
                    128,
                    50,
                    1,
                    0,
                    0,
                    255,
                    255,
                    0,
                    8,
                    0,
                    0,
                    240,
                    0,
                    0,
                    3,
                    0,
                    3,
                    1,
                    0
                };
            } }
    }
   
}
