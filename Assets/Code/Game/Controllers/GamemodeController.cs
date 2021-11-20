using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeController
{
    
    //The component that the player uses
    public abstract Type PlayerComponent { get; }

    //Method called to spawn players
    public abstract void SpawnPlayers();

}
