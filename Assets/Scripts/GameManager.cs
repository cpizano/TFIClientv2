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
    public Tilemap tilemap_L1;
    public Tilemap tilemap_L2;
    public Tilemap tilemap_L3;

    private Tile[] tiles_L0;

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

    public void InitTileMap(int layers, int rows, int columns)
    {
        tiles_L0 = new Tile[1300];
    }

    public void SpawnPlayer(int _id, string _username, Vector2 _position, int _z_level, Quaternion _rotation)
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
        _pm.Move(_position, _z_level);

        //var cell = tilemap_L0.layoutGrid.WorldToCell(_position);
    }

    public void SetTiles(int layer, int row, short[] indexes)
    {
        for (int ix = 0; ix < indexes.Length; ++ix)
        {
            var cell_id = indexes[ix];
            if (cell_id == 0 || cell_id > tiles_L0.Length)
            {
                // 0 values are expected. They mark "no tile".
                continue;
            }

            // Server-side the titles are 1-based.
            cell_id -= 1;
            Vector3Int pos;

            // The 8 server side layers are mapped to 4 tilemaps. This is mostly to compensate
            // for the art assets available at this time.
            switch (layer)
            {
                case 0:
                case 1:
                    pos = new Vector3Int(ix, row, layer);
                    tilemap_L0.SetTile(pos, LoadTitle(cell_id));
                    break;
                case 2:
                case 3:
                    pos = new Vector3Int(ix, row, layer - 2);
                    tilemap_L1.SetTile(pos, LoadTitle(cell_id));
                    break;
                case 4:
                case 5:
                    pos = new Vector3Int(ix, row, layer - 4);
                    tilemap_L2.SetTile(pos, LoadTitle(cell_id));
                    break;
                case 6:
                case 7:
                    pos = new Vector3Int(ix, row, layer - 6);
                    tilemap_L3.SetTile(pos, LoadTitle(cell_id));
                    break;

                default:
                    throw new Exception("unknown layer");
            }
        }

        // We used to call RefreshAllTiles() here but it seems not necessary
        // and added significant lag. If ever needed, consider using a |dirty| flag
        // and calling it just once per layer once player moves or something like
        // that.
    }

    public Tile LoadTitle(int index)
    {
        if (tiles_L0[index] == null)
        {
            tiles_L0[index] = Resources.Load<Tile>($"Tiles/gemap_{index}");
        }

        return tiles_L0[index];
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
