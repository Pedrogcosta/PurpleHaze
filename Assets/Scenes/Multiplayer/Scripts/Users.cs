using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Users : MonoBehaviour
{
    public TMP_Text usersText;
    void Start()
    {
        SocketManager.Socket.OnUnityThread("users", (response) => {
            var users = response.GetValue<string[]>();
            usersText.text = string.Join("\n", users);
        });
    }
}
