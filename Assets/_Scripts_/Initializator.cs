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

        // Try to connect to localhost
        NetworkManager manager = NetworkManager.singleton;
        manager.networkAddress = "localhost";
        nameInput.gameObject.SetActive(false);
        buttonObject.gameObject.SetActive(false);
        StartCoroutine(TryConnect(manager));
    }

    IEnumerator TryConnect(NetworkManager manager)
    {
        manager.StartClient();

        float timer = 3f; // wait 3 seconds for connection
        while (timer > 0f && !NetworkClient.isConnected && !NetworkServer.active)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // If failed to connect, start as host
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            manager.StartHost();
        }

        // Save player name so Player prefab can read it
        Player.localPlayerName = playerName;
    }
}
