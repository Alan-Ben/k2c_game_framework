using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 滚动列表 可播放item显示动画的 container基类mono
/// </summary>
/// <typeparam name="_T_ITEM_MONO"></typeparam>
public abstract class _ATNPGGUIMonoShowAnimContainer<_T_ITEM_MONO> : _TALUGUIMonoContainerWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _AALBasicUIWndMono
{
    [ALHeader("是否开启显示窗口逐个播放item的显示动画")]
    public bool openItemShowAnim = false;

    [ALHeader("首个item开始播放刷新动画的延迟时间")]
    public float firstItemStartShowAnimDelay;
    
    [ALHeader("每个item刷新动画的播放间隔")]
    [FormerlySerializedAs("firstItemShowAnimDelay")]
    public float eachItemShowAnimInterval = 0.15f;
    
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