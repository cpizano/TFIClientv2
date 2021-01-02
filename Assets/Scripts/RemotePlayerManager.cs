using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerManager : PlayerManager
{
    public void Init(int _id, string _username, UIManager _hud, int _health)
    {
        InitPlayer(_id, _username, _hud, _health);
    }

}
