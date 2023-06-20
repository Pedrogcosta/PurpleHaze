using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MenuScene()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void GameScene(){
        SceneManager.LoadScene("Game");
    }

    public void PlazaScene(){
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void RegisterScene(){
        SceneManager.LoadScene("RegisterScene");
    }

    public void LoginScene(){
        SceneManager.LoadScene("LoginScene");
    }

}
