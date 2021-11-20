using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{

    //Player list text mesh
    [SerializeField]
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        //UpdatePlayerList(new List<ConnectedClient>());
    }

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
        textMesh.text = finalMessage;
    }

}
