
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkClient : NetworkBase
{

    public static NetworkClient localClient = null;

    public delegate void OnConnectionSuccess();
    public delegate void OnConnectionFailure();

    //Are we actually connected to a server?
    public bool isConnected { get; private set; } = false;

    //The end point of the server
    private IPEndPoint serverEndPoint;

    //The serve rport
    private int server_port;

    //Are we connecting?
    private bool isConnecting = false;

    public NetworkClient(int server_port) : base()
    {
        this.server_port = server_port;
        identifier = "Client";
    }

    //Attempt Connect Callbacks
    private OnConnectionSuccess connectedCallback;
    private OnConnectionFailure connectionFailureCallback;

    /// <summary>
    /// Attempt connection to the server.
    /// </summary>
    public void AttemptConnect(IPAddress serverAddress, string username,
            OnConnectionSuccess successCallback, OnConnectionFailure failureCallback)
    {
        if(isConnected || isConnecting)
        {
            failureCallback?.Invoke();
            return;
        }
        //We are connecting
        isConnecting = true;
        connectedCallback = successCallback;
        connectionFailureCallback = failureCallback;
        //Start the timeout
        Thread timeoutThread = new Thread(TimeoutConnection);
        timeoutThread.Start();
        //Get the port from the client
        udpClient.Connect(serverAddress, server_port);
        client_port = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
        Debug.Log($"Client port: {client_port}");
        //Create the listening thread and start it
        listeningThread = new Thread(ListenLoop);
        listeningThread.Start();
        //Create the IP end point of the server
        serverEndPoint = new IPEndPoint(serverAddress, server_port);
        //Encode the message
        byte[] usernameByteArray = Encoding.UTF8.GetBytes($"{MessageHeaders.JOIN_REQUEST} {username}");
        //Send the server a message saying we want to connect
        udpClient.Send(usernameByteArray, usernameByteArray.Length);
        //Attempting connection
        Debug.Log("Attempting connection...");
    }

    private void TimeoutConnection()
    {
        //Sleep for 5 seconds
        Thread.Sleep(5000);
        //Check if we are connected
        if(isConnected)
        {
            return;
        }
        //Timeout
        isConnecting = false;
        connectionFailureCallback?.Invoke();
    }

    protected override bool MessageReceieved(IPAddress address, int port, string header, string message)
    {
        //Check if the message is coming from the server for security
        //(Anyone can message the UDP client)
        if(!serverEndPoint.Address.Equals(address))
        {
            Debug.LogError($"Received a message from {address}:{port}, but they are not registered as our server ({serverEndPoint.Address}). Ignoring.");
            return false;
        }
        //Handle the message
        switch(header)
        {
            case MessageHeaders.JOIN_ACCEPTED:
                //:D Not rejected
                isConnected = true;
                connectedCallback?.Invoke();
                isConnecting = false;
                Debug.Log("Connected to server.");
                break;
            default:
                Debug.LogError($"Unrecognised message header: {header}.");
                break;
        }
        return true;
    }
}
