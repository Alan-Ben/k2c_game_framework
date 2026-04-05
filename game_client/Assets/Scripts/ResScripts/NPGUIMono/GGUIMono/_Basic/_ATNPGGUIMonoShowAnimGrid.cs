using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// 滚动列表 可播放item显示动画的 grid基类mono
/// </summary>
/// <typeparam name="_T_ITEM_MONO"></typeparam>
public abstract class _ATNPGGUIMonoShowAnimGrid<_T_ITEM_MONO> : _TALUGUIMonoGridWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _TALUGUIMonoGridItem
{
    [ALHeader("是否开启显示窗口逐个播放item的显示动画")]
    public bool openItemShowAnim = false;
    [ALHeader("显示窗口的第一批Item播放刷新动画的播放间隔")]
    public float firstItemShowAnimDelay = 0.2f;
    [ALHeader("是否每次加载窗口只播放一次动画")]
    public bool isShowAniOnlyOne = false;

    [ALHeader("列表滚动到最顶部(或最右)需要显示的GO列表")]
    public List<GameObject> goScrollToTopShowList;
    [ALHeader("列表滚动到最顶部(或最右)需要隐藏的GO列表")]
    public List<GameObject> goScrollToTopHideList;
    [ALHeader("列表滚动到最底部(或最左)需要显示的GO列表")]
    public List<GameObject> goScrollToBottomShowList;
    [ALHeader("列表滚动到最底部(或最左)需要隐藏的GO列表")]
    public List<GameObject> goScrollToBottomHideList;
}