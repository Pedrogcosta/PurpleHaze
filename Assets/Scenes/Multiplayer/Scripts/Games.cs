using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Games : MonoBehaviour
{
    public TMP_Text gamesText;
    
    [Serializable]
    public class Game {
        public string name;
    }

    void Start() {
       SocketManager.Socket.OnUnityThread("games", (response) => {
           var games = response.GetValue<Game[]>();
           gamesText.text = string.Join("\n", Array.ConvertAll(games, game => game.name));
       });
    }
}
