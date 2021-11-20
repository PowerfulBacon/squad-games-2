using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMaster : MonoBehaviour
{

    public static NetworkMaster networkMaster;
    
    public NetworkHost host;

    private void Start()
    {
        if(networkMaster != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        networkMaster = this;
    }

    private void OnApplicationQuit()
    {
        host.Shutdown();
    }

}
