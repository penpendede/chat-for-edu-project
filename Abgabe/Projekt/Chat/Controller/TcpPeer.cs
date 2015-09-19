using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Controller
{

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
        /// This is the case if a remote host started a connection
        /// </summary>
        /// <param name="client">said client</param>
        public TcpPeer(TcpClient client)
        {
            //UnhandledMessages = new Queue<string>();

            // get the ip of the other end
            _ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            // the port the remote side is listening on is not clear here, because we 
            // will only know on which port it is sending - which is of no use for us

            _client = client;
        }

        /// <summary>
        /// New TcpPeer for a given ip and port
        /// This is the case if the local host started the connection
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

            // encode the string into a byte array and write it to the stream
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
                    // read the incoming data into a byte array of fitting size
                    byte[] buffer = new byte[_client.Available];

                    _client.GetStream().Read(buffer, 0, _client.Available);

                    // decode the message
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
