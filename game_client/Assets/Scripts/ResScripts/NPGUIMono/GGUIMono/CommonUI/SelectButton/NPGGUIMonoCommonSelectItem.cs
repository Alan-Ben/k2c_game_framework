using UnityEngine;
using System.Collections;
using ALPackage;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPGGUIMonoCommonSelectItem : _TALUGUIMonoGridItem
{
    public bool isOn;//是否选中
    public GameObject btnClick;  // 点击按钮

    public List<GameObject> isOnShow;//选中状态时显示
    public List<GameObject> isOffShow;//非选中状态显示
}

