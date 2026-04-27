using NFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameObject : SingletonMono<GlobalGameObject>
{
#if CHEAT
    private void Update()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.W))
        {
            GameMaster.I.GameWin();
        }
    }
#endif
}
