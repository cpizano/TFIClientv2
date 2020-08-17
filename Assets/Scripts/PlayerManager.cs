using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;

    public Camera mainCamera;


    private void Update()
    {
        if (mainCamera != null)
        {
            var _pos = this.transform.position;
            _pos.z = mainCamera.transform.position.z;
            mainCamera.transform.position = _pos;
        }
    }
}
