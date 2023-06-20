using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class CadastrarUsuario : MonoBehaviour
{

    private const string postUrl = "http://localhost:3000/register"; // Replace with your actual API endpoint URL

     public TextMeshProUGUI emailField;
     public TextMeshProUGUI usernameField;
     public TextMeshProUGUI passwordField;

     [Serializable]
     public class Usuario
     {
         public string email;
         public string username;
         public string password;
     }

    public void Cadastrar()
    {
        StartCoroutine(SendPostRequest());
    }

    public IEnumerator SendPostRequest()
    {
        // Create a dictionary to hold your JSON data
        Usuario usuario = new Usuario();
        usuario.email = emailField.text;
        usuario.username = usernameField.text;
        usuario.password = passwordField.text;

        // Convert the dictionary to a JSON string
        string json = JsonUtility.ToJson(usuario);

        // Create a new UnityWebRequest
        UnityWebRequest request = UnityWebRequest.PostWwwForm(postUrl, "POST");

        // Set the content type to application/json
        request.SetRequestHeader("Content-Type", "application/json");

        // Attach the JSON data to the request
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request
        yield return request.SendWebRequest();

        // Check for any errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST request sent successfully");
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            Debug.LogError("Error sending POST request: " + request.error);
        }
    }
}
