using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 荣誉列表界面
/// </summary>
public class GGUIMonoGraveHonorLogGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("排序标题")]
    public Text txtTitle;
    [ALHeader("时间")]
    public Text txtTime;
    [ALHeader("新晋者需要显示的Go")]
    public List<GameObject> newComerShowGos;
    // </AutoGen:MonoDeclaration>
    [ALHeader("称号列表")]
    public GGUIMonoGraveTitleGrid titleGrid;
}

