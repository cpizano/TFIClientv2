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
    public Tilemap tilemap_L0;

    private Tile[] tiles_L0;

    private readonly int[] water_tiles = { 192, 197, 198, 203, 204 };
    private readonly System.Random random = new System.Random();

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
        tiles_L0 = new Tile[1071];
        
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

        //var cell = tilemap_L0.layoutGrid.WorldToCell(_position);
        //SetTiles(761, cell.x, cell.y, 1);
    }

    public void SetTiles(int row, short[] indexes)
    {
        for (int ix = 0; ix < indexes.Length; ++ix)
        {
            var pos = new Vector3Int(ix, row, 0);
            tilemap_L0.SetTile(pos, LoadTitle(indexes[ix]));
        }

        tilemap_L0.RefreshAllTiles();
    }

    public Tile LoadTitle(int index)
    {
        if (index < 0)
        {
            index = GetRandomWaterTileIndex();
        }

        if (tiles_L0[index] == null)
        {
            tiles_L0[index] = Resources.Load<Tile>("Tiles/water_crop_" + index.ToString());
        }

        return tiles_L0[index];
    }

    internal int GetRandomWaterTileIndex()
    {
        return water_tiles[random.Next(0, water_tiles.GetUpperBound(0))];
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
