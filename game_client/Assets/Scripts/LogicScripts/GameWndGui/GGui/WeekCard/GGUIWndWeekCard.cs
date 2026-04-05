using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;


/// <summary>
/// 周卡主界面
/// </summary>
///
public class GGUIWndWeekCard: _ANPGGUIBasicWnd<GGUIMonoWeekCard>
{
    private static GGUIWndWeekCard _g_instance = new GGUIWndWeekCard();
    private ALCommonEnableTaskController _m_task;

    public static GGUIWndWeekCard instance
    {
        get
        {
            if(null == _g_instance)
                _g_instance = new  GGUIWndWeekCard();
            return _g_instance;
        }
    }
    
    
    public GGUIWndWeekCard() : base(EALUIWndLayer.NORMAL)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoWeekCard.assetPath; }
    protected override string _monoObjName { get => GGUIMonoWeekCard.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
        _refreshWnd();
        _m_task = ALCommonEnableTickActionMonoTask.addMonoTask(_refreshTimeDown);
    }

    protected override void _onHideWnd()
    {
        _m_task.setDisable();
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnAssign, _clickAssign);
        ALUGUICommon.combineBtnClick(wnd.btnCostSubscribe, _clickCostSubscribe);
        ALUGUICommon.combineBtnClick(wnd.btnFreeSubscribe, _clickFreeSubscribe);
        ALUGUICommon.combineBtnClick(wnd.btnCostWeekCard, _clickCostWeekCard);
        ALUGUICommon.combineBtnClick(wnd.btnFreeWeekCard, _clickFreeWeekCard);
    }

    /// <summary>
    /// 付费订阅
    /// </summary>
    /// <param name="obj"></param>
    private void _clickCostSubscribe(GameObject obj)
    {
        
    }

    /// <summary>
    /// 免费订阅
    /// </summary>
    /// <param name="obj"></param>
    private void _clickFreeSubscribe(GameObject obj)
    {
        
    }

    /// <summary>
    /// 付费购买周卡
    /// </summary>
    /// <param name="obj"></param>
    private void _clickCostWeekCard(GameObject obj)
    {
        
    }

    /// <summary>
    /// 免费试用周卡
    /// </summary>
    /// <param name="obj"></param>
    private void _clickFreeWeekCard(GameObject obj)
    {
        //已经试用过了
        if (NPPlayer.instance.weekCardComp.hadUseFreeTrial)
            return;
        
        NPPlayer.instance.weekCardComp.reqWeekCardActiveFreeTrial((_info) =>
        {
            _refreshWnd();
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.week_card_payCardTip_des)
            ,TextTranslate.instance.getLanguage(TransKeyConst.cancel)
            ,null
            ,TextTranslate.instance.getLanguage(TransKeyConst.confirm)
            , () =>
            {
                _clickAssign(null);
            },true,TextTranslate.instance.getLanguage(TransKeyConst.week_card_payCardTip_title));
        });
    }

    /// <summary>
    /// 点击委派
    /// </summary>
    /// <param name="obj"></param>
    private void _clickAssign(GameObject obj)
    {
        GCommon.enterUIMainNodeShow(ESysSceneType.WEEK_CARD_ASSIGN);
    }
    
    /// <summary>
    /// 刷新显示
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        bool hasWeekCard = NPPlayer.instance.weekCardComp.hasWeekCard();
        ALUGUICommon.setGameObjEnable(wnd.hasWeekCardShow, hasWeekCard);
        ALUGUICommon.setGameObjEnable(wnd.hasWeekCardHide, !hasWeekCard);
        
        
        ALUGUICommon.setGameObjEnable(wnd.freeWeekCardShow, !NPPlayer.instance.weekCardComp.hadUseFreeTrial);
        // ALUGUICommon.setGameObjEnable(wnd.costWeekCardShow, NPPlayer.instance.weekCardComp.hadUseFreeTrial);
        
        //付费相关先都隐藏,等有付费再根据数据显示
        ALUGUICommon.setGameObjEnable(wnd.costWeekCardShow, NPPlayer.instance.weekCardComp.hadUseFreeTrial);
        ALUGUICommon.setGameObjEnable(wnd.freeSubscribeShow, false);
        ALUGUICommon.setGameObjEnable(wnd.costSubscribeShow, false);
    }
    
    /// <summary>
    /// 刷新倒计时
    /// </summary>
    private void _refreshTimeDown()
    {
        if (null == wnd)
            return;
        if (!NPPlayer.instance.weekCardComp.hasWeekCard())
            return;
        long remainTime = NPPlayer.instance.weekCardComp.expireTimeTagS - FpsAndPingMgr.instance.serverTimeTagS;

        ALUGUICommon.setLabelTxt(wnd.txtTimeDown,  TextTranslate.instance.getLanguage(TransKeyConst.week_card_mainPay_tip ,TimeUtil.millisecondsToTime_Two(remainTime * 1000)));
    }
}
