using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// Represents an all purpose network controller that helps with connecting to servers, retrieving data, and sending data.
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// Delegate function that represents an action done on connection.
        /// </summary>
        /// <param name="ss"></param>
        public delegate void NetworkAction(SocketState ss);

        /// <summary>
        /// This class holds all the necessary state to represent a socket connection
        /// Note that all of its fields are public because we are using it like a "struct"
        /// It is a simple collection of fields
        /// </summary>
        public class SocketState
        {
            public Socket theSocket;
            public int ID;
            public NetworkAction ClientCallback;

            // This is the buffer where we will receive data from the socket
            public byte[] messageBuffer = new byte[1024];

            // This is a larger (growable) buffer, in case a single receive does not contain the full message.
            public StringBuilder sb = new StringBuilder();

            /// <summary>
            /// Creates a SocketState object with the specified ID and socket.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="id"></param>
            public SocketState(Socket s, int id)
            {
                theSocket = s;
                ID = id;
                ClientCallback = null;
            }

            /// <summary>
            /// Creates a SocketState object with the specified ID, socket, and callback function for the instantiator.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="id"></param>
            /// <param name="_actionCallback"></param>
            public SocketState(Socket s, int id, NetworkAction _actionCallback)
            {
                theSocket = s;
                ID = id;
                ClientCallback = _actionCallback;
            }
        }

        public class ServerListenerState
        {
            public TcpListener ServerListener { get; }
            public NetworkAction ServerCallback { get; }

            /// <summary>
            /// Creates a SocketState object with the specified ID, socket, and callback function for the instantiator.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="id"></param>
            /// <param name="_actionCallback"></param>
            public ServerListenerState(TcpListener tcpl, NetworkAction actionCallback)
            {
                ServerListener = tcpl;
                ServerCallback = actionCallback;
            }
        }

        /// <summary>
        /// Represents essential networking functions for client/server architecture to work properly.
        /// </summary>
        public class Networking
        {
            public const int DEFAULT_PORT = 11000;

            /// <summary>
            /// Creates a Socket object for the given host string
            /// </summary>
            /// <param name="hostName">The host name or IP address</param>
            /// <param name="socket">The created Socket</param>
            /// <param name="ipAddress">The created IPAddress</param>
            public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
            {
                ipAddress = IPAddress.None;
                socket = null;
                try
                {
                    // Establish the remote endpoint for the socket.
                    IPHostEntry ipHostInfo;

                    // Determine if the server address is a URL or an IP
                    try
                    {
                        ipHostInfo = Dns.GetHostEntry(hostName);
                        bool foundIPV4 = false;
                        foreach (IPAddress addr in ipHostInfo.AddressList)
                            if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                            {
                                foundIPV4 = true;
                                ipAddress = addr;
                                break;
                            }
                        // Didn't find any IPV4 addresses
                        if (!foundIPV4)
                        {
                            System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                            throw new ArgumentException("Invalid address");
                        }
                    }
                    catch (Exception)
                    {
                        // see if host name is actually an ipaddress, i.e., 155.99.123.456
                        System.Diagnostics.Debug.WriteLine("using IP");
                        ipAddress = IPAddress.Parse(hostName);
                    }

                    // Create a TCP/IP socket.
                    socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                    // Disable Nagle's algorithm - can speed things up for tiny messages, 
                    // such as for a game
                    socket.NoDelay = true;

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                    throw new ArgumentException("Invalid address");
                }
            }

            /// <summary>
            /// Connects to a server using the specified address. Also assigns the passed in callback function to the socket for later use.
            /// Returns the socket.
            /// </summary>
            /// <param name="callbackFunction"></param>
            /// <param name="hostname"></param>
            /// <returns></returns>
            public static Socket ConnectToServer(NetworkAction callbackFunction, string hostname)
            {
                System.Diagnostics.Debug.WriteLine("connecting  to " + hostname);

                // Create a TCP/IP socket.
                Socket socket;
                IPAddress ipAddress;

                MakeSocket(hostname, out socket, out ipAddress);

                SocketState state = new SocketState(socket, -1);
                state.ClientCallback = callbackFunction;

                state.theSocket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, state);

                return state.theSocket;
            }

            /// <summary>
            /// Verifies initial connection and notifies the client. Automatically called on successful connection.
            /// </summary>
            /// <param name="stateAsArObject"></param>
            private static void ConnectedCallback(IAsyncResult stateAsArObject)
            {
                SocketState ss = (SocketState)stateAsArObject.AsyncState;

                try
                {
                    ss.theSocket.EndConnect(stateAsArObject);
                    ss.ClientCallback(ss);
                    GetData(ss);
                }
                catch (Exception e)
                {
                    ss.theSocket.Close();
                    Console.WriteLine(e.ToString());
                    ss.ClientCallback(ss);
                }
            }

            /// <summary>
            /// Requests more data from the server/client assigned to the socket state object.
            /// </summary>
            /// <param name="state"></param>
            public static void GetData(SocketState state)
            {
                // Continue the "event loop" that was started on line 96.
                // Start listening for more parts of a message, or more new messages
                state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }

            /// <summary>
            /// Automatically called when data is received. Makes sure there is data, and notifies the client/server that data has arrived.
            /// </summary>
            /// <param name="stateAsArObject"></param>
            private static void ReceiveCallback(IAsyncResult stateAsArObject)
            {
                try
                {
                    SocketState ss = (SocketState)stateAsArObject.AsyncState;

                    int bytesRead = ss.theSocket.EndReceive(stateAsArObject);

                    // If the socket is still open
                    if (bytesRead > 0)
                    {
                        string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                        // Append the received data to the growable buffer.
                        // It may be an incomplete message, so we need to start building it up piece by piece
                        ss.sb.Append(theMessage);
                        ss.ClientCallback(ss);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION: " + e.Message);
                }
            }

            /// <summary>
            /// Starts a send request to the server, sending the passed in data.
            /// </summary>
            /// <param name="socket"></param>
            /// <param name="data"></param>
            public static void SendSocket(Socket socket, string data)
            {
                try
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    socket.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None, SendCallback, socket);
                }
                catch (SocketException e)
                {
                    using (var deadSock = socket)
                    {
                        deadSock.Disconnect(false);
                        deadSock.Shutdown(SocketShutdown.Both);
                        deadSock.Close();
                    }
                }
                catch (Exception e)
                {

                }
            }

            /// <summary>
            /// Automatically called when the send is successful. Simply ends the request.
            /// </summary>
            /// <param name="ar"></param>
            private static void SendCallback(IAsyncResult ar)
            {
                Socket socket = (Socket)ar.AsyncState;

                socket.EndSend(ar);
            }

            /// <summary>
            /// Initiates a loop for a server that wishes to accept new clients.
            /// </summary>
            /// <param name="serverCallback"></param>
            public static void ServerAwaitingClientLoop(NetworkAction serverCallback)
            {
                Console.WriteLine("Server initialized, waiting for first client.");
                TcpListener tcpListener = new TcpListener(IPAddress.Any, DEFAULT_PORT);
                try
                {
                    tcpListener.Start();

                    ServerListenerState sls = new ServerListenerState(tcpListener, serverCallback);

                    tcpListener.BeginAcceptSocket(AcceptNewClient, sls);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            /// <summary>
            /// OS calls this function upon client contact with the server. Calls the server's callback function then begins the client loop again.
            /// </summary>
            /// <param name="ar"></param>
            private static void AcceptNewClient(IAsyncResult ar)
            {
                Console.WriteLine("A new player has joined the server.");

                ServerListenerState sls = (ServerListenerState)ar.AsyncState;

                TcpListener tcpListener = sls.ServerListener;

                Socket socket = tcpListener.EndAcceptSocket(ar);
                socket.NoDelay = true;

                SocketState ss = new SocketState(socket, 0);
                ss.ClientCallback = sls.ServerCallback;
                ss.theSocket = socket;
                ss.ClientCallback(ss);

                sls.ServerListener.BeginAcceptSocket(AcceptNewClient, sls);
            }
        }
    }
}
