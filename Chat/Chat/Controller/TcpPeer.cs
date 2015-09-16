using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Controller
{
    /// <summary>
    /// Delegate for peer connect handlers
    /// </summary>
    /// <param name="peer">peer who connects</param>
    public delegate void TcpPeerManagerOnPeerConnect(TcpPeer peer);

    // The TcpPeerStatus can either be okay or not connected
    public enum TcpPeerStatus
    {
        OK,
        NOT_CONNECTED
    };

    /// <summary>
    /// Manages the TCP connections
    /// </summary>
    public class TcpPeerManager : IDisposable
    {
        public TcpPeerManagerOnPeerConnect OnConnect;

        private List<TcpPeer> _peers;

        private Thread _listenerThread;

        private TcpListener _listener;

        private int _ownPort;

        /// <summary>
        /// Initialize and start listening
        /// </summary>
        /// <param name="ownPort">port to listen on</param>
        public TcpPeerManager(int ownPort)
        {
            _ownPort = ownPort;

            _peers = new List<TcpPeer>();
            _listen();
        }

        /// <summary>
        /// get a new TcpPeer for a given IP and an optional remote port
        /// </summary>
        /// <param name="ip">The Ip for the TcpPeer (string representation like "192.168.41.11") </param>
        /// <param name="remotePort">an optional port (if missing remotePort is used)</param>
        /// <returns>a new peer of type TcpPeer</returns>
        public TcpPeer GetPeer(string ip, int remotePort = -1)
        {
            remotePort = (remotePort < 0) ? _ownPort : remotePort;

            TcpPeer peer = new TcpPeer(ip, Convert.ToInt32(remotePort));
            _peers.Add(peer);

            return peer;
        }

        /// <summary>
        /// Find the local IP address
        /// </summary>
        /// <returns>local IP as string (or "0.0.0.0" if none is found</returns>
        public string GetLocalIpAddress()
        {

            var host = Dns.GetHostEntry(Dns.GetHostName());

            // iterate over all IP addresses found
            foreach (var ip in host.AddressList)
            {
                // if AddressFamily is InterNetwork assume we have the right IP
                // this may be wrong if the local machine has more than one NIC but then again which user machine has?
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "0.0.0.0";
        }

        /// <summary>
        /// Todo
        /// </summary>
        private void _listenerThreadMethod()
        {
            try
            {
                while (true) // this "forever" actually means "unless interrupted"
                {
                    // make a new peer and add it to the list of peers
                    TcpClient client = _listener.AcceptTcpClient();
                    //string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    TcpPeer peer = new TcpPeer(client);
                    _peers.Add(peer);

                    if (OnConnect != null)
                    {
                        OnConnect(peer); // when available run OnConnect for the new peer
                    }
                }
            }
            catch (SocketException e) // a certain SocketExeption is in order here
            {
                if (e.SocketErrorCode != SocketError.Interrupted) // namely the one that occurs when a connection is iterrupted
                {
                    throw e; // in all other cases throw on...
                }
            }
        }

        /// <summary>
        /// Start listening, for internal use
        /// </summary>
        private void _listen()
        {
            _listener = new TcpListener(IPAddress.Any, _ownPort);

            _listener.Start();

            _listenerThread = new Thread(new ThreadStart(_listenerThreadMethod)); // create listener thread

            _listenerThread.Start(); // start it
        }

        private bool _disposed = false;

        /// <summary>
        /// Dispose listeners if there are still listeners to be disposed.
        /// </summary>
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

    /// <summary>
    /// Delegate for message handlers
    /// </summary>
    /// <param name="msg">message to handle</param>
    public delegate void TcpPeerOnMessage(string msg);

    /// <summary>
    /// A single peer
    /// </summary>
    public class TcpPeer : IDisposable
    {
        public TcpPeerOnMessage MessageReceive;

        //public Queue<string> UnhandledMessages;

        private TcpClient _client;

        private Thread _listenerThread;
        private bool _endThread;

        private string _ip;
        private int _port;

        /// <summary>
        /// New TcpPeer for a given client
        /// </summary>
        /// <param name="client">said client</param>
        public TcpPeer(TcpClient client)
        {
            //UnhandledMessages = new Queue<string>();

            _ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            _client = client;
        }

        /// <summary>
        /// New TcpPeer for a given ip and port
        /// </summary>
        /// <param name="ip">said ip</param>
        /// <param name="port">said port</param>
        public TcpPeer(string ip, int port)
        {
            //UnhandledMessages = new Queue<string>();

            _ip = ip;
            _port = port;

            _client = new TcpClient();
        }

        /// <summary>
        /// Checks if connection exists
        /// </summary>
        /// <returns>truth value of "client exists and is connected"</returns>
        public bool IsConnected()
        {
            return (_client != null) && _client.Connected;
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="msg">message to be sent</param>
        /// <returns>status value, NOT_CONNECTED or OK</returns>
        public TcpPeerStatus SendMessage(string msg)
        {
            // if not connected
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect(_ip, _port); // try to connect
                }
                catch
                {
                    return TcpPeerStatus.NOT_CONNECTED; // on failure return NOT_CONNECTED
                }
            }

            byte[] encoded = Encoding.UTF8.GetBytes(msg);

            _client.GetStream().Write(encoded, 0, encoded.Length);

            return TcpPeerStatus.OK;
        }

        /// <summary>
        /// this is being executed in the listener thread
        /// </summary>
        private void _listenerThreadMethod()
        {
            while (!_endThread) // as long as thread isn't marked as ended
            {
                if (_client.Connected && _client.Available > 0) // if connected and data available
                {
                    byte[] buffer = new byte[_client.Available];

                    _client.GetStream().Read(buffer, 0, _client.Available);

                    string msg = Encoding.UTF8.GetString(buffer);

                    if (msg != "") 
                    {
                        if (MessageReceive != null)
                        {
                            MessageReceive(msg); // For non-empty msg call MessageReceive (if it is available)
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Start listening
        /// </summary>
        public void StartListening()
        {
            // If no listener thread is present create one and start it
            if (_listenerThread == null)
            {
                _listenerThread = new Thread(new ThreadStart(_listenerThreadMethod));

                _listenerThread.Start();
            }
        }

        private bool _disposed = false;

        /// <summary>
        /// Dispose this TcpPeer
        /// </summary>
        public void Dispose()
        {
            // if not already disposed mark thread as having reached its end and close the client
            if (!_disposed)
            {
                _endThread = true;

                _client.Close();
            }
        }
    }

}
