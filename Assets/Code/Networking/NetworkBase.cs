using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NetworkBase
{

    protected string identifier { get; set; }

    //The default port for the host and client
    //They are different so we can have 2 UDP clients on 1 machine for unit testing.
    public const int DEFAULT_SERVER_PORT = 27005;

    //The length of message headers
    public const int MESSAGE_HEADER_LENGTH = 4;

    //The network host's UDP listener
    public UdpClient udpClient { get;  protected set; } = null;

    //Port of the client
    protected int client_port { get; set; }

    //Is the network host running?
    public bool running { get; private set; } = true;

    //Number of messages receieved
    public int messagesReceived { get; private set; } = 0;

    //The thread that handles listening
    protected Thread listeningThread;

    public NetworkBase()
    {
        //Create the client
        udpClient = new UdpClient();
    }
    
    public NetworkBase(int port)
    {
        //Set the port
        client_port = port;
        //Create the server's listener
        udpClient = new UdpClient(client_port);
        //Create the listening thread and start it
        listeningThread = new Thread(ListenLoop);
        listeningThread.Start();
    }

    /// <summary>
    /// Shuts down the listening thread
    /// </summary>
    public void Shutdown()
    {
        running = false;
        udpClient.Close();
    }

    /// <summary>
    /// The listening loop thread
    /// Handles getting messages only. Needs to constantly listen and not be blocked by anything
    /// or messages will be lost (networking will destroy a small amount, we don't want to destroy a bunch).
    /// </summary>
    protected void ListenLoop()
    {
        //Log it
        Debug.Log($"[{identifier}] listening thread initiated on port {client_port}.");
        //Listen for messages
        //TODO: Receieve is blocking so if the server shuts down we never leave this
        while (running)
        {
            try
            {
                //Create the base IP end point
                //This will be modified by the listeners recieve method.
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, client_port);
                //Receieve the message
                byte[] recievedBytes = udpClient.Receive(ref ipEndPoint);
                //Handle message asynchronously (intensive messages will not block other messages from doing things)
                //This means that we can receieve 2 messages at about the same time and we won't lose one of them like in squad games 1.
                Task.Run(() => HandleMessage(ipEndPoint.Address, ipEndPoint.Port, recievedBytes));
            }
            catch(Exception e)
            {
                Debug.LogError($"=======[{identifier}]========");
                Debug.LogError(e);
                Debug.LogError(e.StackTrace);
            }
        }
        //Indicate that the listening thread has ended
        Debug.Log($"[{identifier}] listening thread closed.");
    }
    
    /// <summary>
    /// Handle incomming messages
    /// Done on a seperate thread.
    /// Actual actions are taken in the MessageReceieved abstract method.
    /// </summary>
    private void HandleMessage(IPAddress address, int port, byte[] message)
    {
        //Log the message
        //Debug.Log($"[{identifier}] Receieved message from {address} (Message {messagesReceived}). Current thread ID: {Thread.CurrentThread.ManagedThreadId}");
        //If the server isn't running, the message is now redundant
        if(!running) return;
        //Convert the byte array to string
        string messageString = Encoding.UTF8.GetString(message);
        //Split the message into its header part only
        string messageHeader = messageString.Substring(0, MESSAGE_HEADER_LENGTH);
        //Handle the message receieved
        if(MessageReceieved(address, port, messageHeader, messageString))
        {
            //Count the messages receieved and log the message
            messagesReceived ++;
            Debug.Log($"[{identifier}] Receieved message ({messageHeader}) from {address} (Message {messagesReceived}). Current thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    protected abstract bool MessageReceieved(IPAddress address, int port, string header, string message);

}
