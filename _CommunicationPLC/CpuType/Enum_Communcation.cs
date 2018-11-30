using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _CommunicationPLC
{
    public static class Enum_Communcation
    {
        /// <summary>
        /// 操作西门子plc CPU种类
        /// </summary>
        public enum PLCType
        {
            S7200 = 0,
            S7300 = 10,
            S7400 = 20,
            S71200 = 30,
        }
        /// <summary>
        /// modbus功能码
        /// </summary>
        public enum OperModbus
        {
            ReadCoilStatus = 0x01,//读取线圈当前状态
            ReadInputStatus = 0x02,//读取输入状态
            ReadHoldingRegister = 0x03,//读取保持寄存器
            ReadInputRegister = 0x0,//读取输入寄存器
        }
    }
}
