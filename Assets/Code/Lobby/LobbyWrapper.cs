using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class LobbyWrapper : MonoBehaviour
{

//Adds in a local client to connect to the server for testing purposes.
#if UNITY_EDITOR
    public const bool TESTING_MODE = true;
#else
    public const bool TESTING_MODE = false;
#endif

    //Join instructions object
    public JoinInstructions joinInstructions;
    //Player list object
    public PlayerList playerList;

    private NetworkHost host;

    // Start is called before the first frame update
    void Start()
    {
        //Start the server
        host = new NetworkHost(NetworkBase.DEFAULT_SERVER_PORT);
        //Set up the network master
        //This is a little dodgy and could be improved by just having a static singleton.
        new GameObject("Network Master").AddComponent<NetworkMaster>().host = host;
        //Try and get the local IP address
        try
        {
            joinInstructions.SetJoinIp(GetLocalIPAddress());
        }
        catch(Exception e)
        {
            joinInstructions.SetJoinIp("Networking Error: No network adaptor found.");
        }
        //Add in the callback for client connections
        host.onClientConnect += playerList.UpdatePlayerList;
        //Started server
        Debug.Log("Server started");
        //Connect a debugging client
        if(TESTING_MODE)
        {
            ConnectLocalClient();
        }
    }

    /// <summary>
    /// Creates a fake client and connects them to the server
    /// </summary>
    private void ConnectLocalClient()
    {
        //Create 5 clients
        int clients = 10;
        for(int i = 0; i < clients; i ++)
        {
            //Create and connect the client
            NetworkClient client = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            client.AttemptConnect(IPAddress.Parse(GetLocalIPAddress()), $"Test Client {i}", null, null);
        }
    }

    /// <summary>
    /// Get local IP address
    /// https://stackoverflow.com/questions/6803073/get-local-ip-address
    /// </summary>
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

}
