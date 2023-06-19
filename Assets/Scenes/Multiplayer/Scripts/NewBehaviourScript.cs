using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class CadastrarUsuario : MonoBehaviour
{


    private const string postUrl = "http://localhost:3000/register"; // Replace with your actual API endpoint URL


     public Button cadastrarButton;
     public TextMeshProUGUI emailField;
     public TextMeshProUGUI usernameField;
     public TextMeshProUGUI passwordField;




    public void chamarPorra(){
        
        StartCoroutine(SendPostRequest());
    }

    public IEnumerator SendPostRequest()
    {
        Debug.Log("Mande noticias do mundo de lá, diz quem fica");
        // Create a dictionary to hold your JSON data
        Dictionary<string, string> jsonData = new Dictionary<string, string>();

        jsonData.Add("email", emailField.text);
        jsonData.Add("username",  usernameField.text);
        jsonData.Add("password",  passwordField.text);

        // Convert the dictionary to a JSON string
        string json = JsonUtility.ToJson(jsonData);

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
        }
        else
        {
            Debug.LogError("Error sending POST request: " + request.error);
        }
    }

        public void Start(){

        
       
    }
}