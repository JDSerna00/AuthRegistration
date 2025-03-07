using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    string username;
    string password;
    string url = "https://sid-restapi.onrender.com";
    public string Token { get; set; }
    private void Start()
    {
        Token = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay token");
        }
        else
        {
            StartCoroutine("GetProfile");
        }
    }

    public void Login()
    {
        Credentials credentials = new Credentials();
        credentials.username = GameObject.Find("InputFieldUsername")
            .GetComponent<TMP_InputField>().text;
        credentials.password = GameObject.Find("InputFieldPassword")
            .GetComponent<TMP_InputField>().text;

        string postDataJson = JsonUtility.ToJson(credentials);

        StartCoroutine(LoginPost(postDataJson));
    }

    IEnumerator LoginPost(string postDataJson)
    {
        string path = "/api/auth/login";
        UnityWebRequest request = UnityWebRequest.Put(url + path, postDataJson);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                string json = request.downloadHandler.text;

                AuthResponse response = JsonUtility.FromJson<AuthResponse>(json);
                Token = response.token;
                PlayerPrefs.SetString("token", Token);

                GameObject.Find("StartMenu").SetActive(false);
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetString("username", response.usuario.username);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

    public void Register()
    {
        Credentials credentials = new Credentials();
        credentials.username = GameObject.Find("InputFieldUsername")
            .GetComponent<TMP_InputField>().text;
        credentials.password = GameObject.Find("InputFieldPassword")
            .GetComponent<TMP_InputField>().text;

        string postDataJson = JsonUtility.ToJson(credentials);

        StartCoroutine(RegisterPost(postDataJson));

    }
    IEnumerator GetPerfil()
    {
        string path = "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Get(url + path);
        www.SetRequestHeader("x-token", Token);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                string json = www.downloadHandler.text;
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(json);
                GameObject.Find("PanelAuth").SetActive(false);
            }
            else
            {
                Debug.Log("Token Vencido... redirecionar a Login");
            }
        }
    }

    IEnumerator RegisterPost(string postDataJson)
    {
        string path = "/api/usuarios";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + path, postDataJson);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Debug.Log(request.downloadHandler.ToString());
                StartCoroutine(LoginPost(postDataJson));
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }
}
[System.Serializable]
public class Credentials
{
    public string username;
    public string password;
}
[System.Serializable]
public class AuthResponse
{
    public UserModel usuario;
    public string token;
}
[System.Serializable]
public class UserModel
{
    public string _id;
    public string username;
    public string estado;
    public DataUser data;
}
[System.Serializable]
public class DataUser
{
    public int score;
}
