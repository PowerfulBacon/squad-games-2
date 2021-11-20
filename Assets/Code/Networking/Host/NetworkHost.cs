using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkHost : NetworkBase
{

    //Callback for client connected
    public delegate void ClientConnectedDelegate(NetworkHost host, ConnectedClient connectedClient);
    public ClientConnectedDelegate onClientConnect;

    //Dictionary containing the clients connected to this host
    //The key is the ip address of the client
    public Dictionary<string, ConnectedClient> connectedClients { get; protected set; } = new Dictionary<string, ConnectedClient>();

    public NetworkHost(int port) : base(port)
    {
        identifier = "Server";
    }

    protected override bool MessageReceieved(IPAddress address, int port, string header, string message)
    {
        string addressAsString = $"{address.ToString()}:{port}";
        //Check if the client is considered connected by the server.
        //Removing a client from the list of connected clients essentially kicks them from the game
        //TODO: Convert "JOIN" to a constant
        if(header != MessageHeaders.JOIN_REQUEST && !connectedClients.ContainsKey(addressAsString))
        {
            Debug.LogError("Received message from non-connected client at {address}, ignoring.");
            return false;
        }
        //Do something depending on what we receieved from the client.
        switch(header)
        {
            case MessageHeaders.JOIN_REQUEST:
                Debug.Log($"Validating join request from {address}:{port}");
                //Accept the join request
                connectedClients.Add(addressAsString, new ConnectedClient(address, port));
                connectedClients[addressAsString].username = message.Substring(5);
                connectedClients[addressAsString].SendMessage(udpClient, MessageHeaders.JOIN_ACCEPTED);
                onClientConnect?.Invoke(this, connectedClients[addressAsString]);
                Debug.Log($"Client {address} successfully connected");
                break;
            default:
                Debug.LogError($"Unrecognised message header: {header}.");
                break;
        }
        //Message was fine
        return true;
    }
}
