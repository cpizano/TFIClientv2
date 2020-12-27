using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    private const int serverSend = 102;
    private const int serverHandle = 61;

    public static void Welcome(Packet _packet)
    {
        int _serverSend = _packet.ReadInt();
        int _serverHandle = _packet.ReadInt();
        int _id = _packet.ReadInt();
        int pixels_per_unit = _packet.ReadInt();
        int mapVersion = _packet.ReadInt();
        int mapLayerCount = _packet.ReadInt();
        int mapRowCount = _packet.ReadInt();
        int mapColumnCount = _packet.ReadInt();

        // TODO: sync the pixels per unit with Unity framework.
        if ((serverSend != _serverSend) ||
            (serverHandle != _serverHandle) ||
            (2 != mapVersion) || pixels_per_unit != 32)
        {
            ClientSend.SessionEnd($"wrong version {serverSend} {serverHandle} {mapVersion}");
            return;
        }

        Client.instance.myId = _id;
        GameManager.instance.InitTileMap(mapLayerCount, mapRowCount, mapColumnCount);

        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(Client.instance.tcp.socket.Client);

        Debug.Log($"Server welcomes as player {_id}");
    }

    public static void MapLayerRow(Packet _packet)
    {
        int layer = _packet.ReadInt();
        int row = _packet.ReadInt();
        int row_len = _packet.ReadInt();
        var cells = new short[row_len];
        for (int ix = 0; ix < row_len; ix++)
        {
            cells[ix] = _packet.ReadShort();
        }
        GameManager.instance.SetTiles(layer, row, cells);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        var _position = _packet.ReadVector2();
        var _z_level = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _z_level, _rotation);
        Debug.Log($"Spawn player: {_id} pos {_position} rot {_rotation}");
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        var _position = _packet.ReadVector2();
        int z_level = _packet.ReadInt();

        GameManager.players[_id].Move(_position, z_level);
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
