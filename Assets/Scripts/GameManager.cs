using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public Canvas canvas;
    public Tilemap tilemap;

    private Tile[] tiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("GameManager already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tiles = Resources.FindObjectsOfTypeAll<Tile>();
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        bool _local;

        if (_id == Client.instance.myId)
        {
            // Local player gets the camera.
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            _local = true;

        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            _local = false;
        }

        Camera _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        var label = canvas.GetComponent<UIManager>().AddPlayerLabel();
        var _pm = _player.GetComponent<PlayerManager>();
        _pm.Init(_id, _username, label, _camera, _local);

        players.Add(_id, _pm);

        var cell = tilemap.layoutGrid.WorldToCell(_position);
        SetTiles(_id, cell.x, cell.y, 1);
    }

    public void SetTiles(int index, int x, int y, int count)
    {
        for (int ix = 0; ix < count; ++ix)
        {
            tilemap.SetTile(new Vector3Int(x + ix, y, 0), tiles[index]);
        }
    }

    internal void DeSpawnPlayer(int _id, int reason)
    {
        if (players.TryGetValue(_id, out var _playerManager))
        {
            Destroy(_playerManager.gameObject);
        }
        players.Remove(_id);
    }
}
