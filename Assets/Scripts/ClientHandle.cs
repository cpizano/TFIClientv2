using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    private const int serverSend = 66;
    private const int serverHandle = 61; 
    public static void Welcome(Packet _packet)
    {
        int _serverSend = _packet.ReadInt();
        int _serverHandle = _packet.ReadInt();
        int _id = _packet.ReadInt();

        if ((serverSend != _serverSend) || (serverHandle != _serverHandle)) 
        {
            ClientSend.SessionEnd($"wrong version {serverSend} {serverHandle}");
            return;
        }

        Client.instance.myId = _id;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(Client.instance.tcp.socket.Client);

        Debug.Log($"Server welcomes as player {_id}");
    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        Debug.Log($"Spawn player: {_id} pos {_position} rot {_rotation}");
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
}
