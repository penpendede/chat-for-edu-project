using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Controller
{
    public delegate void TcpPeerManagerOnPeerConnect(TcpPeer peer);

    public enum TcpPeerStatus
    {
        OK,
        NOT_CONNECTED
    };

    public class TcpPeerManager : IDisposable
    {
        public TcpPeerManagerOnPeerConnect OnConnect;

        private List<TcpPeer> _peers;

        private Thread _listenerThread;

        private TcpListener _listener;

        private int _ownPort;

        public TcpPeerManager(int ownPort)
        {
            _ownPort = ownPort;

            _peers = new List<TcpPeer>();
            _listen();
        }

        public TcpPeer GetPeer(string ip, int remotePort = -1)
        {
            remotePort = (remotePort < 0) ? _ownPort : remotePort;

            TcpPeer peer = new TcpPeer(ip, Convert.ToInt32(remotePort));
            _peers.Add(peer);

            return peer;
        }

        public string GetLocalIpAddress()
        {
            //IPAddress[] addressList = Dns.Resolve("localhost").AddressList;
            //return ((IPEndPoint)_listener.Server.LocalEndPoint).Address.ToString();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "0.0.0.0";
        }

        private void _listenerThreadMethod()
        {
            try
            {
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    //string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    TcpPeer peer = new TcpPeer(client);
                    _peers.Add(peer);

                    if (OnConnect != null)
                    {
                        OnConnect(peer);

                    }
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.Interrupted)
                {
                    throw e;
                }
            }
        }

        private void _listen()
        {
            _listener = new TcpListener(IPAddress.Any, _ownPort);

            _listener.Start();

            _listenerThread = new Thread(new ThreadStart(_listenerThreadMethod));

            _listenerThread.Start();
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (TcpPeer p in _peers)
                {
                    p.Dispose();
                }

                _peers = null;

                _listener.Stop();
                _disposed = true;
            }
        }
    }

    public delegate void TcpPeerOnMessage(string msg);

    public class TcpPeer : IDisposable
    {
        public TcpPeerOnMessage MessageReceive;

        //public Queue<string> UnhandledMessages;

        private TcpClient _client;

        private Thread _listenerThread;
        private bool _endThread;

        private string _ip;
        private int _port;

        public TcpPeer(TcpClient client)
        {
            //UnhandledMessages = new Queue<string>();

            _ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            _client = client;

            _listen();
        }

        public TcpPeer(string ip, int port)
        {
            //UnhandledMessages = new Queue<string>();

            _ip = ip;
            _port = port;

            _client = new TcpClient();

            _listen();
        }

        public bool IsConnected()
        {
            return (_client != null) && _client.Connected;
        }

        public TcpPeerStatus SendMessage(string msg)
        {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect(_ip, _port);
                }
                catch
                {
                    return TcpPeerStatus.NOT_CONNECTED;
                }
            }
            byte[] encoded = Encoding.UTF8.GetBytes(msg);

            _client.GetStream().Write(encoded, 0, encoded.Length);

            return TcpPeerStatus.OK;
        }

        private void _listenerThreadMethod()
        {
            while (!_endThread)
            {
                if (_client.Connected && _client.Available > 0)
                {
                    byte[] buffer = new byte[_client.Available];

                    _client.GetStream().Read(buffer, 0, _client.Available);

                    string msg = Encoding.UTF8.GetString(buffer);

                    if (msg != "")
                    {
                        if (MessageReceive != null)
                        {
                            MessageReceive(msg);
                        }
                        else
                        {
                            throw new Exception("bäääh"); // UnhandledMessages.Enqueue(msg);
                        }
                    }
                }
            }
        }

        private void _listen()
        {
            _listenerThread = new Thread(new ThreadStart(_listenerThreadMethod));

            _listenerThread.Start();
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                _endThread = true;

                _client.Close();
            }
        }
    }

}
