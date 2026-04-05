using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟宝箱
/// </summary>
public class GGUIMonoGuildBoxScoreContainerItem : _AALBasicUIWndMono
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("宝箱名字")]
    public Text txtName;
    [ALHeader("积分")]
    public Text txtScore;
    [ALHeader("第奇数个item需要显示的GO列表")]
    public List<GameObject> goOddNumberItemShowList;
    [ALHeader("第奇数个item需要隐藏的GO列表")]
    public List<GameObject> goOddNumberItemHideList;
    // </AutoGen:MonoDeclaration>
}

