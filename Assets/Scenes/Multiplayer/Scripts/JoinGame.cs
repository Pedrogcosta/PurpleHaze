using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinGame : MonoBehaviour
{
     public TMP_InputField nameField;
     public TMP_InputField passwordField;
     public TMP_Text error;

     [Serializable]
     private class Game {
          public string name;
          public string password;
     }

     [Serializable]
     private class Result {
          public string result;
          public string reason;
     }

    public void Join()
    {
         Game game = new Game();
         game.name = nameField.text;
         game.password = passwordField.text;

        SocketManager.Socket.Emit("joinGame", response => {
             var result = response.GetValue<Result>();
             
             UnityThread.executeInUpdate(() => {
                  if (result.result == "joined") {
                       CreateGame.created = false;
                       GameRoom.gameName = game.name;
                       SceneManager.LoadScene("GameRoom");
                  } else if (result.result == "error") {
                       error.text = result.reason;
                  }
             });
        }, game);
    }
}
