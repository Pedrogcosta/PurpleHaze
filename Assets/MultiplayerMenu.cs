using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class MultiplayerMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I'm in multiplayer menu");
        GameObject socketManager = GameObject.Find("SocketManager");
        if (socketManager != null) {
            SocketIOUnity socket = socketManager.GetComponent<LoadManager>().socket;
            socket.On("chat", (e) => {
                Debug.Log("chat: " + e);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
