using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using SocketIOClient;
using TMPro;

public class Chat : MonoBehaviour
{
    public TextMeshProUGUI chat;
    public TMP_InputField field;

     [Serializable]
     public class ChatMessage
     {
         public string username;
         public string message;
     }

    void Start()
    {
        SocketManager.Socket.OnUnityThread("chat", (response) => {
            var chatMessage = response.GetValue<ChatMessage>();
            chat.text += '\n' + chatMessage.username + ": " + chatMessage.message;
        });
    }

    public void SendChatMessage() {
        SocketManager.Socket.Emit("chat", field.text);
        field.text = "";
        field.Select();
    }
}
