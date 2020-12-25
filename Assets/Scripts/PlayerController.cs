using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;
    private int scale = 0;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerManager.SetZoom(0);
    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            scale = (scale + 1) % 4;
            playerManager.SetZoom(scale);
        }
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[4];
        int _sendData = 0;

        if (Input.GetKey(KeyCode.W))
        {
            _inputs[0] = true;
            _sendData++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _inputs[1] = true;
            _sendData++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _inputs[2] = true;
            _sendData++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _inputs[3] = true;
            _sendData++;
        }

        if (_sendData > 0)
        {
            ClientSend.PlayerMovement(_inputs);
        }
    }
}
