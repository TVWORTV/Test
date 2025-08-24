using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class Initializator : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject buttonObject;
    string playerName;

    public void OnJoinButton()
    {
        // Assign name
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            playerName = $"Player{Random.Range(1000, 9999)}";
        }
        else
        {
            playerName = nameInput.text;
        }

        NetworkManager manager = NetworkManager.singleton;
        manager.networkAddress = "localhost";
        nameInput.gameObject.SetActive(false);
        buttonObject.gameObject.SetActive(false);
        StartCoroutine(TryConnect(manager));
    }

    IEnumerator TryConnect(NetworkManager manager)
    {
        manager.StartClient();

        float timer = 3f; 
        while (timer > 0f && !NetworkClient.isConnected && !NetworkServer.active)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            manager.StartHost();
        }

        Player.localPlayerName = playerName;
    }
}
