using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class EarningGoalPlayerInfoWnd
    {
        private EarningGoalPlayerShowMono wnd;
        private NPGGUIWndPlayerIcon _m_playerInfo; //开宴玩家信息
        private NPGGUIWndCommonShowCase _m_showCase;//展示视频
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        public EarningGoalPlayerInfoWnd(EarningGoalPlayerShowMono _mono)
        {
            wnd = _mono;
            if (wnd == null)
                return;
            if (wnd.monoShowcase != null) 
                _m_showCase = new NPGGUIWndCommonShowCase(wnd.monoShowcase);
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            
            if(wnd.itemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
        }

        public void discard()
        {
            if (_m_showCase != null) 
                _m_showCase.discard();
            _m_showCase = null;
            
            
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
        }

        public void showWnd()
        {
            if (wnd == null)
                return;

            long achieveCid = NPPlayer.instance.earningGoalComp.honorAchievePlayerCid(wnd.earningGoalHonorId, out long timestamp);
            bool hasAchieve = achieveCid > 0;

            if (hasAchieve)
            {
                //展示当前章节视频
                if (null != _m_showCase)
                {
                    GCommon.reqPlayerInfo(achieveCid, (playerInfo) =>
                    {
                        _m_showCase?.showWnd(new ShowCaseCommonResUnitInfoObj(playerInfo?.skinRef?.td_show));
                    });
                }
            
                if (null != _m_playerInfo)
                {
                    _m_playerInfo.showWnd();
                    _m_playerInfo?.setPlayer(achieveCid);
                }
                string tempTime = TimeUtil.DateTime2StringYMD(TimeUtil.FromUTCByTimeZone(timestamp));

                ALUGUICommon.setLabelTxt(wnd.txtAchieveTime, tempTime);
            }
            
            EarningGoalHonorRewardRefObj honorRewardRefObj = GRefdataCoreMgr.instance.earningGoalHonorRewardRefCore.getRef(wnd.earningGoalHonorId);
            if (honorRewardRefObj != null)
            {
                _m_wItemContainer?.showItemList(honorRewardRefObj.first_gain_item_list);
            
                ALUGUICommon.setLabelTxt(wnd.txtEarningGoal,
                    TextTranslate.instance.getLanguage(wnd.earningGoalTransKey,
                        honorRewardRefObj.earning_goal.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            }
            
            ALUGUICommon.setGameObjEnable(wnd.hasAchieveShowGos, hasAchieve);
            ALUGUICommon.setGameObjEnable(wnd.hasAchieveHideGos, !hasAchieve);
        }
        
        public void hideWnd()
        {
            if (_m_showCase != null)
                _m_showCase.hideWnd();
            if (_m_playerInfo != null)
                _m_playerInfo.hideWnd();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndEarningGoalMain : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEarningGoalMain>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        private List<EarningGoalPlayerInfoWnd> _m_playerInfoList = new List<EarningGoalPlayerInfoWnd>();

        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        public GGUIWndEarningGoalMain(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }
    
        protected override string _monoAssetPath { get => _m_sAssetPath; }
        protected override string _monoObjName { get => _m_sObjName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
        }
    
        protected override void _onHideWnd()
        {
            if (_m_playerInfoList != null)
                foreach (var playerInfoWnd in _m_playerInfoList)
                {
                    if (playerInfoWnd != null) playerInfoWnd.hideWnd();
                }
            _m_tcTickTaskController.setDisable();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {

            if (_m_playerInfoList != null)
            {
                foreach (var playerInfoWnd in _m_playerInfoList)
                {
                    playerInfoWnd?.discard();
                }
                _m_playerInfoList.Clear();
                _m_playerInfoList = null;
            }
            _m_tcTickTaskController.setDisable();
            if(wnd == null)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnHonor, _onBtnHonorClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSelf, _onBtnSelfClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.earningGoalPlayerShowList != null)
            {
                _m_playerInfoList = new List<EarningGoalPlayerInfoWnd>();
                foreach (var playerShow in wnd.earningGoalPlayerShowList)
                {
                    if(playerShow == null)
                        continue;
                    EarningGoalPlayerInfoWnd playerInfo = new EarningGoalPlayerInfoWnd(playerShow);
                    _m_playerInfoList.Add(playerInfo);
                }
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnHonor, _onBtnHonorClick);
            ALUGUICommon.combineBtnClick(wnd.btnSelf, _onBtnSelfClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_playerInfoList != null)
                foreach (var playerInfoWnd in _m_playerInfoList)
                {
                    if (playerInfoWnd != null) playerInfoWnd.showWnd();
                }

            //设置倒计时任务
            _m_tcTickTaskController.setDisable();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onCDRefresh, 1f);
        }
        
        //倒计时刷新
        private void _onCDRefresh()
        {
            if (wnd == null)
                return;

            //当前状态结束时间
            long curStateFinishTimeMs = NPPlayer.instance.earningGoalComp.getEarningGoalActivityEndTime();

            long leftTimeMs = curStateFinishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            long hour = leftTimeMs / (1000 * 60 * 60);
            long minute = (leftTimeMs / (1000 * 60)) % 60;
            long second = (leftTimeMs / 1000) % 60;
            string cdTxt =  TextTranslate.instance.getLanguage(TransKeyConst.earning_goal_timestamp_h_m_s, hour, minute, second);
            ALUGUICommon.setLabelTxt(wnd.txtCD, cdTxt);
        }

        private void _onBtnHonorClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndEarningGoalGlobal.instance, UINodeTagConst.C_EARNING_GOAL_GLOBAL_REWARD);
        }

        private void _onBtnSelfClick(GameObject _)
        {                
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndEarningGoalSelf.instance, UINodeTagConst.C_EARNING_GOAL_SELF_REWARD); 

        }
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EARNING_GOAL_MAIN);
        }
    }
}