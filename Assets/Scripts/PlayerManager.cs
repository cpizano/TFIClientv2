using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public TextMeshProUGUI label;
    public Camera mainCamera;

    private static Vector3 labelOffset = new Vector3(0, -1.3f, 0);
    private Animator animator;
    //private Renderer renderer;
    private int animationStopTimer = 0;
    private bool isLocal = false;
    private int z_level = 0;

    public void Init(int _id, string _username, TextMeshProUGUI _label, Camera _camera, bool _local)
    {
        id = _id;
        username = _username;
        mainCamera = _camera;
        label = _label;
        label.text = _username;
        isLocal = _local;
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 _new_pos, int _z_level)
    {
        // There is a weird bias towards the left animation in the
        // blend tree so we need to magify the delta (by at least 4).
        //var _delta = (_new_pos - transform.position) * 4f; ;
        var _delta_x = _new_pos.x - transform.position.x;
        var _delta_y = _new_pos.y - transform.position.y;

        if (isLocal)
        {
            var _pos = _new_pos;
            var camera_z = mainCamera.transform.position.z;
            var camera_pos = new Vector3(_new_pos.x, _new_pos.y, camera_z);
            mainCamera.transform.position = camera_pos;
        }

        if (z_level != _z_level)
        {
            z_level = _z_level;
            GetComponent<Renderer>().sortingOrder = z_level;
        }

        transform.position = _new_pos;

        animator.enabled = true;
        animator.SetFloat("Move X", _delta_x * 4f);
        animator.SetFloat("Move Y", _delta_y * 4f);
        animationStopTimer = 5;
    }

    public void SetZoom(int _factor)
    {
        switch (_factor)
        {
            case 0: mainCamera.transform.localScale = Vector3.one * 0.5f; break;
            case 1: mainCamera.transform.localScale = Vector3.one; break;
            case 2: mainCamera.transform.localScale = Vector3.one * 1.5f; break;
            case 3: mainCamera.transform.localScale = Vector3.one * 2.0f; break;
            default:
                throw new Exception("invalid zoon factor");
        }
    }

    private void FixedUpdate()
    {
        // So the label belongs to the canvas which in turns uses
        // mainCamera as the render camera. Now the render mode is set
        // to Screen-space-overlay which operates in screen points..
        label.transform.position = mainCamera.WorldToScreenPoint(transform.position + labelOffset);
        
        if (animationStopTimer == 0)
        {
            animator.enabled = false;
        }
        else
        {
            animationStopTimer -= 1;
        }
    }

}
