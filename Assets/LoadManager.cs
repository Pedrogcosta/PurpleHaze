using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

public class LoadManager : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(this);
    }

    public SocketIOUnity socket;
    // Start is called before the first frame update
    void Start()
    {
        string token = PlayerPrefs.GetString("token", null);
        if (token == null && token == "") {
            SceneManager.LoadScene("ConnectError");
        }

        var uri = new Uri("http://localhost:3000");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Auth = new Dictionary<string, string>
                {
                    {"token", token }
                }
            ,
            EIO = 4
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("connect: " + e);
            UnityThread.executeInUpdate(() => {
                SceneManager.LoadScene("MultiplayerMenu");
            });
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
            SceneManager.LoadScene("ConnectError");
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"Reconnecting: attempt = {e}");
        };
        socket.OnError += (sender, e) =>
        {
            Debug.Log($"Error = {e}");
        };
        socket.On("connect_error", (e) =>
        {
            Debug.Log("connect error: " + e);
        });
        ////

        Debug.Log("Connecting...");
        socket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
