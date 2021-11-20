using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{

    //Player list text mesh
    [SerializeField]
    private TextMeshProUGUI textMesh;

    private string newMessage;

    /// <summary>
    /// Updates the playerlist
    /// </summary>
    public void UpdatePlayerList(NetworkHost host, ConnectedClient connectedClient)
    {
        Debug.Log($"Updating player list. Length: {host.connectedClients.Count}");
        string finalMessage = "";
        //Iterate through all clients
        foreach(ConnectedClient client in host.connectedClients.Values)
        {
            finalMessage = $"{finalMessage}\t<color=#{ColorUtility.ToHtmlStringRGB(client.colour)}>>{client.username}<</color>";
        }
        //Set the text of the message
        newMessage = finalMessage;
    }

    private void Update()
    {
        if(newMessage == null) return;
        textMesh.text = newMessage;
    }

}
