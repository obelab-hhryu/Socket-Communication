using Newtonsoft.Json;
using SocketModel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Server
    {
        public event EventHandler LogGenerated;
        public event EventHandler ClientConnected;
        public event EventHandler GameResultReceived;

        private Socket _sendSocket = null;

        public Server(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            try
            {
                var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);
                listener.Listen(5);

                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            }
            catch (Exception e)
            {
                string message = $"[TCP] Accept exeception {e.Message}";
                LogGenerated?.Invoke(this, new LogGeneratedEventArgs(message));
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            if (ar.AsyncState is Socket listener)
            {
                var handler = listener.EndAccept(ar);

                var stateObj = new StateObject();
                stateObj.workSocket = handler;

                handler.BeginReceive(stateObj.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObj);
                ClientConnected?.Invoke(this, new ClientConnectedEventArgs(handler));
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (ar.AsyncState is StateObject stateObj)
            {
                var handler = stateObj.workSocket;

                try
                {
                    if (handler.Connected == false)
                    {
                        // disconnected
                    }
                    else
                    {
                        int bytesRead = handler.EndReceive(ar);
                        if (bytesRead < StateObject.BUFFER_SIZE)
                        {
                            _sendSocket = handler;

                            string str = Encoding.UTF8.GetString(stateObj.buffer, 0, bytesRead);
                            stateObj.JsonStr.Append(str);

                            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Formatting = Formatting.Indented };

                            var receivedData = JsonConvert.DeserializeObject<SocketData>(stateObj.JsonStr.ToString());
                            GameResultReceived?.Invoke(this, receivedData);
                        }
                        else
                        {
                            string str = Encoding.UTF8.GetString(stateObj.buffer, 0, bytesRead);
                            stateObj.JsonStr.Append(str);

                            stateObj.buffer = new byte[1024];
                            handler.BeginReceive(stateObj.buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObj);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogGenerated?.Invoke(this, new LogGeneratedEventArgs($"[TCP/ReadCallback] {e.Message}"));
                }
            }
        }

        public bool Send(byte[] data)
        {
            if (_sendSocket != null)
            {
                _sendSocket.Send(data);
                return true;
            }

            return false;
        }
    }

    class ClientConnectedEventArgs : EventArgs
    {
        public ClientConnectedEventArgs(Socket socket)
        {
            Socket = socket;
        }
        public Socket Socket { get; }
    }

    class LogGeneratedEventArgs : EventArgs
    {
        public LogGeneratedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class StateObject
    {
        public const int BUFFER_SIZE = 1024;
        public byte[] buffer = new byte[BUFFER_SIZE];

        public StringBuilder JsonStr = new StringBuilder();
        public Socket workSocket = null;
    }
}