using ALPackage;
using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 邮件状态
/// </summary>
public enum EMailReadStat
{
    [InspectorName("未读")]
    NO_READ,//未读
    [InspectorName("已读")]
    HAS_READ,//已读
}

public class GGUISubMonoMailItemPrefab : _AALBasicUIWndMono
{
    [ALHeader("读取按钮")]
    public GameObject btnRead;//读取按钮 
    [ALHeader("红点图标")]
    public GameObject goRedPoint;//小红点图标   
    [ALHeader("邮件标题")]
    public TextEx txtTitle;//邮件标题文字  
    [ALHeader("邮件副标题")]
    public TextEx txtSubTitle;//邮件副标题文字
    [ALHeader("邮件发送者")]
    public TextEx txtSender;//邮件发送者
    [ALHeader("发送者图标")]
    public RawImage senderIcon;//发送者图标
    [ALHeader("邮件有效期")]
    public TextEx txtValidityTime;//邮件有效期    
    [ALHeader("奖励邮件标志")]
    public GameObject goRewardTip;//奖励邮件标志

    [ALHeader("奖励物品")]
    public GGUIMonoCommonRewardContainerItem rewardItemMono;
    
    [ALHeader("收藏状态显示")]
    public List<GameObject> goLockShow;//收藏状态
    [ALHeader("收藏状态隐藏")]
    public List<GameObject> goLockHide;//收藏状态
    [ALHeader("必读状态")]
    public GameObject goNeedRead;//必读状态
    [ALHeader("邮件读取状态显隐配置")]
    public List<NPCommonEnumStatInfo<EMailReadStat>> mailStatInfos;
    [ALHeader("已读已领取置灰对象")]
    public List<MaskableGraphic> hasReadHasGetGrayList;
    [ALHeader("邮件领取状态显隐配置")]
    public List<NPCommonEnumStatInfo<ENPCommonGetStat>> mailGetStatInfos;
}
