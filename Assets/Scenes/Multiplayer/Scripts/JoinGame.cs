using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void IniciarGame()
    {
        SceneManager.LoadScene("JoinGame");
    }
}
