using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabel
{
    private UIManager ui_manager;
    private TextMeshProUGUI label;
    private static Vector3 labelOffset = new Vector3(0, -0.7f, 0);

    public PlayerLabel(TextMeshProUGUI _label, UIManager _ui_manager)
    {
        ui_manager = _ui_manager;
        label = _label;
    }

    public void Move(Vector3 position)
    {
        label.transform.position = ui_manager.WorldToScreen(position + labelOffset);
    }
}
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI playerLabelPrefab;
    public Slider health;

    private Camera world_camera;

    public Camera WorldCamera { set => world_camera = value; }

    public Vector3 WorldToScreen(Vector3 position)
    {
        return world_camera.WorldToScreenPoint(position);
    }

    public PlayerLabel MakePlayerLabel(string name)
    {
        var label = Instantiate(playerLabelPrefab, gameObject.transform);
        label.text = name;
        return new PlayerLabel(label, this);
    }

    public void SetHealth(int value)
    {
        health.value = value / 100.0f;
    }
}
