using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static _CommunicationPLC._CustomEventHandler;

namespace _CommunicationPLC
{
    public class Communication_Instace : ICommunication_Instace
    {
        public string IP { get; set; }
        public Enum_Communcation.PLCType Cpu { get; set; }

        public bool IsAvailable {
            get
            {
                Ping ping = new Ping();
                PingReply result = ping.Send(IP);
                return result != null && result.Status == IPStatus.Success;
            }
        }

        public int port { get; set; } = 502;

        public Communication_Instace(Enum_Communcation.PLCType cpuType)
        {
            this.Cpu = cpuType;
           
        }

        public Result<Socket> Connecetion(string ip, int TimeOut)
        {
            this.IP = ip;
            Result<Socket> result = new Result<Socket>();
            ManualResetEvent connectionDone = new ManualResetEvent(false);
            StateObject state = new StateObject();
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            OperTimeOut connectionTimerOut = new OperTimeOut()
            {
                WorkSocket = socket,
                DelayTime = TimeOut
            };
            Task.Factory.StartNew(t=> { ThreadPoolCheckTimeOut(t); }, connectionTimerOut);
            try
            {
                state.WaitDone = connectionDone;
                state.WorkSocket = socket;
                socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port),new AsyncCallback(ConnCallBack),state);
            }
            catch (Exception ex)
            {
                connectionTimerOut.IsSuccessful = true;
                socket.Close();
                connectionDone.Close();
                result.Message = ex.ToString();
                return result;
            }
            connectionDone.WaitOne();
            connectionDone.Close();
            connectionTimerOut.IsSuccessful = true;
            //如果异步对象发生错误
            if (state.IsError)
            {
                if (socket != null) { socket.Close(); }
                return result;
            }
            result.Content = socket;
            result.IsSuccess = true;
            state.Clear();
            state = null;
            return result;

        }

        public Result<byte[]> Receive(Socket socket, int Length)
        {
            throw new NotImplementedException();
        }

        public Result Send(Socket socket, byte[] data)
        {
            if (data == null) return Result.CreateSuccessResult();
            Result result = new Result();
            ManualResetEvent sendDone = new ManualResetEvent(false);
            StateObject state = new StateObject(data.Length);

            try
            {
                state.WaitDone = sendDone;
                state.WorkSocket = socket;
                state.buffer = data;
                socket.BeginSend(state.buffer,state.AlreadyDealLength,state.DataLength-state.AlreadyDealLength,SocketFlags.None,new AsyncCallback(SendCallBack),state);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (socket != null) { socket.Close(); }
                sendDone.Close();
                return result;
            }
            sendDone.WaitOne();
            sendDone.Close();
            if (state.IsError)
            {
                socket.Close();
                result.Message = state.ErrerMsg;
                return result;
            }
            state.Clear();
            state = null;
            result.IsSuccess = true;
            return result;
        }
     
        protected void ThreadPoolCheckTimeOut(object obj)
        {
            if (obj is OperTimeOut timeout)
            {
                while (!timeout.IsSuccessful)
                {
                    Thread.Sleep(100);
                    if ((DateTime.Now - timeout.StartTime).TotalMilliseconds > timeout.DelayTime)
                    {
                        // 连接超时或是验证超时
                        if (!timeout.IsSuccessful)
                        {
                            timeout.Operator?.Invoke();
                            timeout.WorkSocket?.Close();
                        }
                        break;
                    }
                }
            }
        }
       
        private void ConnCallBack(IAsyncResult ir)
        {
            StateObject st = ir.AsyncState as StateObject;
            try
            {
                if (st != null)
                {
                    Socket socket = st.WorkSocket;
                    socket.EndConnect(ir);
                    st.WaitDone.Set();
                }
                else
                {
                    st.WaitDone.Set();
                    st.WaitDone.Close();
                    st.WorkSocket.Close();
                }
            }
            catch (Exception ex)
            {
                st.IsError = true;
                st.ErrerMsg = ex.ToString();
                st.WaitDone.Set();
            }
            
        }
      
        private void SendCallBack(IAsyncResult ir)
        {
            StateObject st = ir.AsyncState as StateObject;
            try
            {
                if (st != null)
                {
                    Socket socket = st.WorkSocket;
                    int byteSend = socket.EndSend(ir);
                    st.AlreadyDealLength += byteSend;
                    //如果已经接收的数据小于发送的字节数组长度
                    //那么就继续发送接收
                    if (st.AlreadyDealLength < st.DataLength)
                    {
                        socket.BeginSend(st.buffer, st.AlreadyDealLength, st.DataLength - st.AlreadyDealLength, SocketFlags.None, new AsyncCallback(SendCallBack), st);
                    }
                    else
                    {
                        //本次接受完成就标识正在阻塞线程继续执行
                        st.WaitDone.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                st.ErrerMsg = ex.ToString();
                st.IsError = true;
                st.WaitDone.Set();
            }
        }


    }
}
