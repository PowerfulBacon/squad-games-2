using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// A client connected to the server
/// </summary>
public class ConnectedClient
{

    //Static random used by all clients
    private static System.Random random = new System.Random();

    //Called when the generic action button is pressed
    public delegate void OnActionPressed();
    public OnActionPressed actionButtonPressed;

    //The end point of the client, where messages will be sent to
    public IPEndPoint ipEndPoint { get; private set; }

    //Username of the client
    public string username { get; set; } = "Username";

    //Colour of the client
    public Color colour { get; private set; } = new Color(0, 0, 0);

    public ConnectedClient(IPAddress address, int port)
    {
        //Get an IP end point
        ipEndPoint = new IPEndPoint(address, port);
        //Get a random colour
        SetRandomColour();
        Debug.Log($"New client registered at {address}:{port}");
    }

    /// <summary>
    /// Sets a random colour
    /// </summary>
    public void SetRandomColour()
    {
        colour = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1.0f);
    }

    /// <summary>
    /// Send a message to the client
    /// </summary>
    public void SendMessage(UdpClient serverClient, string header, string data = "")
    {
        Debug.Log($"Sending {header} message to client");
        //Convert the string to bytes
        byte[] messageAsBytes = Encoding.UTF8.GetBytes($"{header} {data}");
        //Send it to the client's end point
        serverClient.Send(messageAsBytes, messageAsBytes.Length, ipEndPoint);
    }

}
