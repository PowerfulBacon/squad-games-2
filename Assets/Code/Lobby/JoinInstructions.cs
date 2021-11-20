using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinInstructions : MonoBehaviour
{

    //The message to display
    //%GAME_LINK will be replaced with the game link
    //%IP_ADDRESS will be replaced with the IP address
    private const string MESSAGE = "Join the squad games!\nGo to <b>%GAME_LINK</b> and connect to:\n<b>%IP_ADDRESS</b>\nWhile connected to the same WiFi";
    
    //Link to the game page
    private const string GAME_LINK = "powerfulbacon.itch.io/squad-games-2";
    
    //The text mesh we are linked to
    [SerializeField]
    private TextMeshProUGUI textMesh;

    /// <summary>
    /// Sets up the message
    /// </summary>
    public void SetJoinIp(string ipText)
    {
        textMesh.text = MESSAGE.Replace("%GAME_LINK", GAME_LINK).Replace("%IP_ADDRESS", ipText);
    }

}
