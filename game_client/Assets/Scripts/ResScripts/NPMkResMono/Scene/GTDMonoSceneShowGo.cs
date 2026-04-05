using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 场景对象显隐藏
/// </summary>
public class GTDMonoSceneShowGo : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    private void OnEnable()
    {
        foreach (var go in gameObjects)
        {
            ALUGUICommon.setGameObjEnable(go, true);
        }
    }

    private void OnDisable()
    {
        foreach (var go in gameObjects)
        {
            ALUGUICommon.setGameObjEnable(go, false);
        }
    }
}
