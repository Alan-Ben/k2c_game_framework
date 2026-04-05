using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class RushExchangeStateInfo
{
    [ALHeader("状态类型")] public ERushExchangeState type;
    [ALHeader("当前状态显示的go列表")] public List<GameObject> goListShow;

    public static void setState(List<RushExchangeStateInfo> _stateList, ERushExchangeState _type)
    {
        if (_stateList == null) return;
        RushExchangeStateInfo matchState = null; 
        foreach (var state in _stateList)
        {
            if (state == null)
                continue;
            if (state.type == _type)
            {
                matchState = state;
            }
            ALUGUICommon.setGameObjEnable(state.goListShow, false);
        }

        if (matchState != null)
        {
            ALUGUICommon.setGameObjEnable(matchState.goListShow, true);
        }
    }
}

/// <summary>
/// 限时兑换
/// </summary>
public class GGUIMonoRushExchangeMain : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("消耗物品")]
    public NPGGUIMonoCommonItemContainer costItems;
    [ALHeader("获得物品")]
    public NPGGUIMonoCommonItemContainer rewardItems;
    [ALHeader("倒计时")]
    public List<Text> txtRefreshTimeList;
    [ALHeader("兑换按钮")]
    public GameObject btnExchange;
    [ALHeader("领奖按钮")]
    public GameObject btnReward;
    [ALHeader("钻石补充开关")]
    public NPGGUIMonoCommonToggleEx toggleUseGem;
    [ALHeader("钻石补充数量")]
    public Text txtUseGemCount;
    [ALHeader("钻石足够颜色")]
    public Color colGemEnough = Color.green;
    [ALHeader("钻石不足颜色")]
    public Color colGemNotEnough = Color.red;
    // </AutoGen:MonoDeclaration>
    [ALHeader("状态列表")]
    public List<RushExchangeStateInfo> stateInfoList = new List<RushExchangeStateInfo>();
    [ALHeader("钻石不足显示的go列表")] 
    public List<GameObject> noEnoughGemShowList;
    public List<GameObject> noEnoughGemHideList;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(9000); } }
    public static string objName { get { return UIResPathAssistant.getObjName(9000);} }
}