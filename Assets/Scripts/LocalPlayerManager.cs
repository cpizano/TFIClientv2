using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerManager : PlayerManager
{

    private Camera main_camera_;
    private UIManager hud;

    public void Init(int _id, string _username, Camera _camera, UIManager _hud, int _health)
    {
        main_camera_ = _camera;
        hud = _hud;
        InitPlayer(_id, _username, _hud.MakePlayerLabel(_username));
        HealthChange(_health);
    }

    public override void Move(Vector2 _new_pos, int _z_level)
    {
        var camera_z = main_camera_.transform.position.z;
        var camera_pos = new Vector3(_new_pos.x, _new_pos.y, camera_z);
        main_camera_.transform.position = camera_pos;

        base.Move(_new_pos, _z_level);
    }

    public override void SetZoom(int _factor)
    {
        switch (_factor)
        {
            case 0: main_camera_.transform.localScale = Vector3.one * 0.5f; break;
            case 1: main_camera_.transform.localScale = Vector3.one; break;
            case 2: main_camera_.transform.localScale = Vector3.one * 1.5f; break;
            case 3: main_camera_.transform.localScale = Vector3.one * 2.0f; break;
            default:
                throw new Exception("invalid zoon factor");
        }
    }

    public override void HealthChange(int _health)
    {
        hud.SetHealth(_health);
        base.HealthChange(_health);
    }
}
