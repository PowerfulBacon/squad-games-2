using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    //Are we dead?
    //If we are dead, onActionPressed should be ignored
    protected bool dead = false;
    
    //Attached client
    public ConnectedClient Client { get; private set; }

    /// <summary>
    /// Called when the client presses their action button
    /// </summary>
    public abstract void OnActionPressed();

    /// <summary>
    /// Called when the player dies
    /// </summary>
    public void Death()
    {
        //Prevent OnActionPressed from being called
        dead = true;
        //Relay death to the client
        Client.Death();
    }

}
