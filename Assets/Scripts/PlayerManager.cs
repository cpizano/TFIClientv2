﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private int id;
    private string username;
    private PlayerLabel label;
    private Animator animator;

    private int animationStopTimer = 0;
    
    protected int z_level;
    protected int health;

    public void InitPlayer(int _id, string _username, PlayerLabel _label)
    {
        id = _id;
        username = _username;
        animator = GetComponent<Animator>();
        label = _label;
    }

    public virtual void Move(Vector2 _new_pos, int _z_level)
    {
        if (z_level != _z_level)
        {
            z_level = _z_level;
            GetComponent<Renderer>().sortingOrder = z_level;
        }

        // There is a weird bias towards the left animation in the
        // blend tree so we need to magify the delta (by at least 4).
        //var _delta = (_new_pos - transform.position) * 4f; ;
        var _delta_x = _new_pos.x - transform.position.x;
        var _delta_y = _new_pos.y - transform.position.y;

        transform.position = _new_pos;

        animator.enabled = true;
        animator.SetFloat("Move X", _delta_x * 4f);
        animator.SetFloat("Move Y", _delta_y * 4f);
        animationStopTimer = 5;
    }

    public virtual void SetZoom(int _factor)
    {
        // Do nothing.
    }

    private void FixedUpdate()
    {
        label.Move(transform.position);
        
        if (animationStopTimer == 0)
        {
            animator.enabled = false;
        }
        else
        {
            animationStopTimer -= 1;
        }
    }

    public virtual void HealthChange(int _health)
    {
        health = _health;
    }
}
