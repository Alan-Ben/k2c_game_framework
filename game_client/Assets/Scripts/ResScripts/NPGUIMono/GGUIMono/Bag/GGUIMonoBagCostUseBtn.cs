using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 显示消耗的按钮
/// </summary>
public class GGUIMonoBagCostUseBtn : _AALBasicUIWndMono
{

    [ALHeader("使用按钮")]
    public GameObject useBtn;

    [ALHeader("消耗item  - 没有消耗时隐藏")]
    public NPGGUIMonoCommonItem costItem;

    [ALHeader("有消耗时需要隐藏的Go List")]
    public List<GameObject> costHideGoList;
}

