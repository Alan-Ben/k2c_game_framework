using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

public enum EPlayerLikeStat
{
    [InspectorName("常规状态，玩家自己的分享或者自己的个人信息")]
    NONE,
    [InspectorName("可点赞状态")]
    CAN_LIKE,
    [InspectorName("已经点赞状态")]
    HAS_LIKE,
}

[Serializable]
public class GGUIMonoPlayerInfoDailyRewardShow
{
    [ALHeader("获得奖励的按钮")]
    public GameObject btnGainReward;
    [ALHeader("是否有未领取的奖励的显示")]
    public List<GameObject> listHasRewardShow;
    public List<GameObject> listRewardDrawnShow;
    public List<GameObject> listNoRewardShow;


    /// <summary>
    /// 设置是否有奖励
    /// </summary>
    public void setHasReward(bool _hasReward, bool _isDrawn)
    {
        ALUGUICommon.setGameObjEnable(listHasRewardShow, false);
        ALUGUICommon.setGameObjEnable(listRewardDrawnShow, false);
        ALUGUICommon.setGameObjEnable(listNoRewardShow, false);
        
        if (_hasReward)
            ALUGUICommon.setGameObjEnable(_isDrawn ? listRewardDrawnShow : listHasRewardShow, true);
        else
            ALUGUICommon.setGameObjEnable(listNoRewardShow, true);
    }
}

/// <summary>
/// 玩家信息
/// </summary>
public class GGUIMonoPlayerInfo : _ANPBasicUIWndResBarMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("玩家形象显示")]
    public GGUIMonoCommonShowCase playerShowcase;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfoMono;
    [ALHeader("VIP按钮")]
    public GameObject btnVIP;

    [ALHeader("修改名称按钮")]
    public GameObject chgNameBtn;

    [ALHeader("复制按钮")]
    public GameObject copyBtn;

    [ALHeader("当前经验进度条")]
    public NPGGUIMonoProgress expProgress;
    [ALHeader("当前赚速进度条")]
    public NPGGUIMonoProgress earningsProgress;
    
    [ALHeader("升级按钮")]
    public GameObject upLvlBtn;
    [ALHeader("可升级时显示的GO列表")]
    public List<GameObject> listUpgradableShow;
    [ALHeader("可升级时隐藏的GO列表")]
    public List<GameObject> listUpgradableHide;
    [ALHeader("不可升级时置灰的GO列表")]
    public List<MaskableGraphic> listUnupgradableGray;

    [ALHeader("满级时需要显示的GO列表")]
    public List<GameObject> goMaxLevelShowList;
    [ALHeader("满级时需要隐藏的GO列表")]
    public List<GameObject> goMaxLevelHideList;
    [ALHeader("待加入顾问头像")]
    public RawImage imgGainHeroIcon;

    [ALHeader("皮肤按钮")]
    public GameObject btnSkin;
    [ALHeader("称号按钮")]
    public GameObject titleBtn;

    [ALHeader("头像按钮")]
    public GameObject iconBtn;

    [ALHeader("设置按钮")]
    public GameObject settingBtn;

    [ALHeader("获取伙伴的按钮")]
    public GameObject heroGainBtn;
    
    [ALHeader("每日奖励相关内容")]
    public GGUIMonoPlayerInfoDailyRewardShow dailyRewardShow;

    [ALHeader("升级特效和特效挂点")]
    public long upgradeSfxRefId;
    public Transform upgradeSfxParent;
    

    public void setUpgradable(bool _upgradable)
    {
#if NP_GAME
        ALUGUICommon.setGameObjEnable(listUpgradableShow, false);
        ALUGUICommon.setGameObjEnable(listUpgradableHide, false);
        ALUGUICommon.setGameObjEnable(_upgradable ? listUpgradableShow : listUpgradableHide, true);
        if (!_upgradable)
            GGameCommonInfo.grayImage(listUnupgradableGray);
        else
            GGameCommonInfo.disgrayImage(listUnupgradableGray);  
#endif
    }
    

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1700); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1700); } }
}
