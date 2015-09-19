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
        // the delegate which will be called if a new connection comes in
        public TcpPeerManagerOnPeerConnect OnConnect;

        // a list of all connected peers
        private List<TcpPeer> _peers;

        // the thread the tcpListener runs in
        private Thread _listenerThread;

        // the tcpListener itself
        private TcpListener _listener;

        // the own used port
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
        /// <returns>local IP as string (or "0.0.0.0" if none is found)</returns>
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
        /// The listener loops which runs in a thread to not block the programm
        /// </summary>
        private void _listenerThreadMethod()
        {
            try
            {
                while (true) // this "forever" actually means "unless interrupted"
                {
                    // wait for a new connection, create a peer and add it to the list of peers
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
                if (e.SocketErrorCode != SocketError.Interrupted) // namely the one that occurs when the AcceptTcpClient method is iterrupted
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

}
