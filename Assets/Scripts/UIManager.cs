using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI playerLabelPrefab;

    public TextMeshProUGUI AddPlayerLabel()
    {
        var tmp = Instantiate(playerLabelPrefab, gameObject.transform);
        return tmp;
    }
}
