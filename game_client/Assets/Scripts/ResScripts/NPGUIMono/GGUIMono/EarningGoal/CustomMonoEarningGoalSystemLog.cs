
using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class CustomMonoEarningGoalSystemLog : MonoBehaviour
{
    [ALHeader("荣耀目标id")]
    public long earningGoalHonorId;
    [ALHeader("该玩家形象展示")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("玩家形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcase;
    [ALHeader("翻译key{0:玩家名字}{1:赚速目标}")] 
    public string earningGoalTransKey ;
    [ALHeader("赚速目标文本")]
    public Text txtEarningGoal;
    
    #if NP_GAME

    private NPGGUIWndPlayerIcon _m_playerInfo; //开宴玩家信息
    private NPGGUIWndCommonShowCase _m_showCase;//展示视频

    private void Awake()
    {
        if (monoShowcase != null) 
            _m_showCase = new NPGGUIWndCommonShowCase(monoShowcase);
        if (playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(playerInfo);
    }

    private void OnDestroy()
    {
        if (_m_showCase != null) 
            _m_showCase.discard();
        _m_showCase = null;
            
            
        _m_playerInfo?.discard();
        _m_playerInfo = null;
    }

    private void OnEnable()
    {      
       _refreshWnd();
        WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
    }

    private void OnDisable()
    {
        WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
    }

    private void _refreshWnd()
    {
        long achieveCid = NPPlayer.instance.earningGoalComp.honorAchievePlayerCid(earningGoalHonorId, out long timestamp);
        bool hasAchieve = achieveCid > 0;

        if (hasAchieve)
        {
            GCommon.reqPlayerInfo(achieveCid, (playerInfo) =>
            {
                _m_showCase?.showWnd(new ShowCaseCommonResUnitInfoObj(playerInfo?.skinRef?.td_show));
                
                if (null != _m_playerInfo)
                {
                    _m_playerInfo.showWnd();
                    _m_playerInfo?.setPlayerInfo(playerInfo);
                }
                EarningGoalHonorRewardRefObj honorRewardRefObj = GRefdataCoreMgr.instance.earningGoalHonorRewardRefCore.getRef(earningGoalHonorId);
                if (honorRewardRefObj != null)
                {
                    ALUGUICommon.setLabelTxt(txtEarningGoal, TextTranslate.instance.getLanguage(earningGoalTransKey, playerInfo?.name, honorRewardRefObj.earning_goal.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
                }
            });
        }
    }
    #endif
}
