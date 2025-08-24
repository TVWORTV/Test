using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 5f;
    CamFollow follower;

    [SerializeField] TMP_Text nameDisplay;
    [SerializeField] GameObject cubePrefab; // assign in Inspector

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    public static string localPlayerName;

    void Start()
    {
        if (isLocalPlayer)
        {
            SetCamera();
            CmdSetName(localPlayerName);
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Tell server to broadcast a message
            CmdSendMessage("Player " + playerName + " pressed SPACE!");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Tell server to spawn a cube
            CmdSpawnCube();
        }
    }

    void SetCamera()
    {
        follower = Camera.main.GetComponent<CamFollow>();
        if (follower != null)
            follower.CharSpawned(transform);
    }

    [Command]
    void CmdSetName(string newName)
    {
        playerName = newName;
    }

    // === Space key handling ===
    [Command]
    void CmdSendMessage(string msg)
    {
        RpcReceiveMessage(msg);
    }

    [ClientRpc]
    void RpcReceiveMessage(string msg)
    {
        Debug.Log(msg);
    }

    // === Cube spawning ===
    [Command]
    void CmdSpawnCube()
    {
        GameObject cube = Instantiate(cubePrefab, transform.position + transform.forward * 2f, Quaternion.identity);
        NetworkServer.Spawn(cube);
    }

    // Runs on clients whenever playerName changes
    void OnNameChanged(string oldName, string newName)
    {
        nameDisplay.text = newName;
    }
}
