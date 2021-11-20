using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAct : MonoBehaviour
{
    
    public void OnPress()
    {
        NetworkClient.localClient.SendMessage(MessageHeaders.CLIENT_ACTION);
    }

}
