using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟宝箱容器
/// </summary>
public class GGUIMonoGuildBoxGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildBoxGridItem>
{
    [ALHeader("没有Item的时候显示")]
    public List<GameObject> noItemShowList;

    [ALHeader("点击Item的时候移动的Y轴上移多少")]
    public float onClickItemMoveUpLength = 100;
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
}

