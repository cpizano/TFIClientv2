﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static MapHandler mapHandler;

    private const int serverSend = 81;
    private const int serverHandle = 61;

    public static void Welcome(Packet _packet)
    {
        int _serverSend = _packet.ReadInt();
        int _serverHandle = _packet.ReadInt();
        int _id = _packet.ReadInt();
        int mapVersion = _packet.ReadInt();
        int mapLayerCount = _packet.ReadInt();
        int mapRowCount = _packet.ReadInt();
        int mapColumnCount = _packet.ReadInt();

        if ((serverSend != _serverSend) ||
            (serverHandle != _serverHandle) ||
            (MapHandler.mapVersion != mapVersion))
        {
            ClientSend.SessionEnd($"wrong version {serverSend} {serverHandle} {mapVersion}");
            return;
        }

        Client.instance.myId = _id;

        mapHandler = new MapHandler(mapLayerCount, mapRowCount, mapColumnCount);

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

        GameManager.players[_id].Move(_position);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerQuit(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _reason = _packet.ReadInt();

        GameManager.instance.DeSpawnPlayer(_id, _reason);
    }

}
