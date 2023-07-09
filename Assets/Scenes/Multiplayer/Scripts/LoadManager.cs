using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

public class LoadManager : MonoBehaviour
{
    void Start()
    {
        SocketManager.Connect(() => {
            SceneManager.LoadScene("MultiplayerMenu");
        });
    }
}
