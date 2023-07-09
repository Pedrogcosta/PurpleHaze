using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour
{
    public void Leave() {
        SocketManager.Socket.Emit("leaveGame");
        CreateGame.created = false;
        SceneManager.LoadScene("MultiplayerMenu");
    }
}
