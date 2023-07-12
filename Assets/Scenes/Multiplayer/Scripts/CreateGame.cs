using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json.Linq;

public class CreateGame : MonoBehaviour
{
     public TMP_InputField nameField;
     public TMP_InputField passwordField;
     public TMP_Text error;
     public Toggle canJoinAfter;
     public static bool created = false;

     [Serializable]
     private class Game {
          public string name;
          public string password;
          public bool canEnterAfterStart;
     }

     [Serializable]
     private class Result {
          public string result;
          public string reason;
     }

    public void Create() {
         Game game = new Game();
         game.name = nameField.text;
         game.password = passwordField.text;
         game.canEnterAfterStart = canJoinAfter.isOn;

        SocketManager.Socket.Emit("createGame", response => {
             var result = response.GetValue<Result>();
             Debug.Log(result.result);
             
             UnityThread.executeInUpdate(() => {
                  if (result.result == "created") {
                       created = true;
                       GameRoom.gameName = game.name;
                       SceneManager.LoadScene("GameRoom");
                  } else {
                       Debug.Log(result.reason);
                       error.text = result.reason;
                  }
             });
        }, game);
    }
}
