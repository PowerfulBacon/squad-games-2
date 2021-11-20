using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Gamemode
{
    
    [Tooltip("Name of the gamemode")]
    public string name;

    [Tooltip("Max players required for the gamemode")]
    public int maxPlayers = 1000000;

    [Tooltip("Min players required for the gamemode")]
    public int minPlayers = 0;

    [Tooltip("List of scenes that can be loaded")]
    public List<string> sceneList = new List<string>();

}
