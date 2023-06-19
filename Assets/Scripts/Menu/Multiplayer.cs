using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Multiplayer : MonoBehaviour
{
    public void IniciarGame()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }
}
