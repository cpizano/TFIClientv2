using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField userNameField;
    public TMP_InputField serverAddress;

    public void Start()
    {
        userNameField.text = LoadPlayerName();
        serverAddress.text = LoadServerAddress();
    }

    public void ConnectToServer()
    {
        userNameField.interactable = false;
        serverAddress.interactable = false;
        SavePlayerName(userNameField.text);
        SaveServerAddress(serverAddress.text);

        Client.instance.ConnectToServer(userNameField.text, serverAddress.text);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void SavePlayerName(string name)
    {
        PlayerPrefs.SetString("playername", name);
    }
    private void SaveServerAddress(string name)
    {
        PlayerPrefs.SetString("serveraddr", name);
    }

    private string LoadPlayerName()
    {
        return PlayerPrefs.GetString("playername", "your-name-here");
    }
    private string LoadServerAddress()
    {
        return PlayerPrefs.GetString("serveraddr", "127.0.0.1");
    }

}
