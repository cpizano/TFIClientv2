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
    private int animationStopTimer = 0;


    public void Init(int _id, string _username, TextMeshProUGUI _label, Camera _camera)
    {
        id = _id;
        username = _username;
        mainCamera = _camera;
        label = _label;
        label.text = _username;
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 _new_pos)
    {
        // There is a weird bias towards the left animation in the
        // blend tree so we need to magify the delta (by at least 4).
        var _delta = (_new_pos - transform.position) * 4f; ;

        if (mainCamera != null)
        {
            var _pos = _new_pos;
            _pos.z = mainCamera.transform.position.z;
            mainCamera.transform.position = _pos;
        }

        transform.position = _new_pos;

        animator.enabled = true;
        animator.SetFloat("Move X", _delta.x);
        animator.SetFloat("Move Y", _delta.y);
        animationStopTimer = 5;
    }

    private void FixedUpdate()
    {
        label.transform.position = transform.position + labelOffset;
        if (animationStopTimer == 0)
        {
            animator.enabled = false;
        }
        else
        {
            animationStopTimer += -1;
        }
    }

    private void Update()
    {
        
    }
}
