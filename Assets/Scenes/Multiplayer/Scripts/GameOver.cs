using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;

public class GameOver : MonoBehaviour
{
    public static SocketIOResponse response;

    public async void Retry()
    {
        await response.CallbackAsync("restart");
        SceneManager.UnloadSceneAsync("GameOver");
    }

    // Update is called once per frame
    public async void Exit()
    {
        await response.CallbackAsync("leave");
        SceneManager.LoadScene("MultiplayerMenu");
    }
}
