using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json;

public class SocketManager
{
    private static SocketIOUnity socket;
    public static SocketIOUnity Socket {
        get {
            if (socket == null) {
                Connect();
            }
            return socket;
        }
    }

    [Serializable]
    public class User {
        public string username;
        public string email;
    }

    public static User user;

    public static void Connect(Action callback = null) {
        string token = PlayerPrefs.GetString("token", null);
        if (token == null && token == "") {
            SceneManager.LoadScene("LoginScene");
        }

        var parts = token.Split('.');
        if (parts.Length > 2)
        {
            var decode = parts[1];
            var padLength = 4 - decode.Length % 4;
            if (padLength < 4)
            {
                decode += new string('=', padLength);
            }
            var bytes = System.Convert.FromBase64String(decode);
            var userInfo = System.Text.ASCIIEncoding.ASCII.GetString(bytes);

            user = JsonConvert.DeserializeObject<User>(userInfo);
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
            if (callback != null) {
                UnityThread.executeInUpdate(callback);
            }
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
            UnityThread.executeInUpdate(() => {
                SceneManager.LoadScene("TitleScreen");
            });
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"Reconnecting: attempt = {e}");
        };
        socket.OnError += (sender, e) =>
        {
            Debug.Log($"Error = {e}");

            UnityThread.executeInUpdate(() => {
                PlayerPrefs.DeleteKey("token");
                SceneManager.LoadScene("LoginScene");
            });
        };
        socket.OnUnityThread("startGame", (response) => {
            SceneManager.LoadScene("MultiplayerGame");
        });

        Debug.Log("Connecting...");
        socket.Connect();
    }
}
