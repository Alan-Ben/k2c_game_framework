using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

public enum EProcessStat
{
    //TODO  可能还得改下
    NONE,//不存在这个进度值
    FULL,//满值
    PART,//部分值， >0 且 <最大值
    EMPTY,//空值
}


/// <summary>
/// 普通商店界面
/// </summary>
public class GGUIMonoShopNormalPage : _AALBasicUIWndMono
{
    [ALHeader("如果只有一个页签，是否需要显示页签")]
    public bool oneTabNeedShow = false;
    [ALHeader("子页签列表")]
    public GGUIMonoShopSubTabContainer monoSubTabContainer;
    [ALHeader("自动刷新倒计时控件")]
    public NPGGUIMonoCommonCountDown autoCountDownMono;

    [ALHeader("恢复免费点数倒计时")]
    public NPGGUIMonoCommonCountDown freeCountDownMono;
    [ALHeader("付费刷新消耗")]
    public NPGGUIMonoCommonItem costCommonItemMono;

    [ALHeader("免费刷新按钮")]
    public GameObject freeRefreshBtn;
    [ALHeader("免费刷新次数")]
    public TextEx freeRefreshCount;

    [ALHeader("付费刷新按钮")]
    public GameObject costRefreshBtn;
    
    [ALHeader("有自动刷新显示的Go List")]
    public List<GameObject> autoShowGoList;
    [ALInfo("NONE,//不存在这个进度值" +
            "FULL,//满值" +
            "PART,//部分值， >0 且 <最大值" +
            "EMPTY,//空值")]
    [ALHeader("免费次数状态配置")]
    public List<NPCommonEnumStatInfo<EProcessStat>> freeProcessStatList;

    [ALHeader("付费消耗显示的Go List")]
    public List<GameObject> costShowGoList;

    [ALHeader("纯自动刷新需要隐藏的Go List")]
    public List<GameObject> autoNeedHideGoList;

    [ALHeader("商店容器")]
    public GGUIMonoShopNormalPageGrid shopGridMono;

    [ALHeader("没有商店物品显示的GoList")]
    public List<GameObject> noShopShowGoList;

    [ALHeader("刷新商店成功播放音效id")]
    public long refreshSucSoundId;

}
