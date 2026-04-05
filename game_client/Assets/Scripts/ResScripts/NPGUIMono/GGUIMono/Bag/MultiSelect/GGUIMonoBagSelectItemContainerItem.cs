using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

public class GGUIMonoBagSelectItemContainerItem : _AALBasicUIWndMono
{

    [ALHeader("基础数据")]
    public NPGGUIMonoCommonItem monoItem;

    [ALHeader("选中按钮列表")]
    public List<GameObject> btnSelectList;

    [ALHeader("选择状态")]
    public GameObject goSelected;

    [ALHeader("总数量文本")]
    public Text selectAllCountTxt;
}
