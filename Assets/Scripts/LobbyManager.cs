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

    public void ConnectToServer()
    {
        //startMenu.SetActive(false);
        //userNameField.interactable = false;

        Client.instance.ConnectToServer(userNameField.text, serverAddress.text);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
