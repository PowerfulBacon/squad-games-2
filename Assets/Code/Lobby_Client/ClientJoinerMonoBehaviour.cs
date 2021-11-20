using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TMPro;
using UnityEngine;

public class ClientJoinerMonoBehaviour : MonoBehaviour
{

    //The username field
    public TMP_InputField usernameField;
    //The IP address field
    public TMP_InputField ipAddressField;

    //Error message box
    public TextMeshProUGUI errorTextMesh;

    public void JoinGame()
    {
        //Check the username field is valid
        if(usernameField.text.Length < 3)
        {
            DisplayError("Username must be at least 3 characters!");
            return;
        }
        //Check the IP address field has something in it
        if(ipAddressField.text.Length == 0)
        {
            DisplayError("No IP address entered!");
            return;
        }
        //Try to parse IP
        IPAddress parsedIpAddress;
        if(!IPAddress.TryParse(ipAddressField.text, out parsedIpAddress))
        {
            DisplayError("Invalid IP address format!");
            return;
        }
        //Clear error message
        DisplayError("");
        //Attempt to connect
        try
        {
            NetworkClient.localClient = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            NetworkClient.localClient.AttemptConnect(parsedIpAddress, usernameField.text, ConnectedSuccessfully, FailedToConnect);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            DisplayError(e.Message);
        }
    }

    private void FailedToConnect()
    {
        DisplayError("Failed to connect to server successfully!");
    }

    private void ConnectedSuccessfully()
    {
        DisplayError("Successfully failed to fail to connect to the server!");
    }

    //Weird way to display error messages: TMPs cannot be editted from threads.
    private void DisplayError(string error)
    {
        errorMessageWraper = error;
    }

    private string errorMessageWraper = null;

    private void Update()
    {
        if(errorMessageWraper != null)
        {
            errorTextMesh.text = errorMessageWraper;
            errorMessageWraper = null;
        }
    }

}
