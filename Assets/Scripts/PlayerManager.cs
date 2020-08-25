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

    private void FixedUpdate()
    {
        if (mainCamera != null)
        {
            MoveLabel();
        }
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            var _pos = this.transform.position;
            _pos.z = mainCamera.transform.position.z;
            mainCamera.transform.position = _pos;
        } 
        else
        {
            MoveLabel();
        }

    }

    private void MoveLabel()
    {
        label.transform.position = this.transform.position + labelOffset;
    }
}
