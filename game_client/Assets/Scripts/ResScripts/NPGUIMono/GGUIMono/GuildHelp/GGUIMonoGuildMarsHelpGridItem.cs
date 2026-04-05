using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 联盟火星互助详情
/// </summary>
public class GGUIMonoGuildMarsHelpGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playIcon;
    [ALHeader("帮助内容")]
    public Text txtHelpDetal;
    [ALHeader("帮助进度")]
    public NPGGUIMonoProgress helpProgress;
    [ALHeader("减少总耗时")]
    public Text txtReduceTime;
    // </AutoGen:MonoDeclaration>
    [ALHeader("我的帮助显隐藏列表")]
    public List<GameObject> myHelpShowList;
    public List<GameObject> myHelpHideList;
    [ALHeader("更多信息显示动画")]
    public Animation moreInfoShowAnimation;
    [ALHeader("更多信息显示动画名字")]
    public string moreInfoShowAnimationName = "show";
}

