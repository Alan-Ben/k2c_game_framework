using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 午间活动入口子窗口
    /// </summary>
    public class GGUISubWndMiddayDungeonEnter : _ANPGGUIBasicSubWnd<GGUISubMonoMiddayDungeonEnter>
    {
        private GGUISubWndMiddayDungeonMiniBox _m_miniBoxWnd = null;
        private GGUISubWndMiddayDungeonBox _m_boxWnd = null;
        private int _m_timeDownSer;
    
        public GGUISubWndMiddayDungeonEnter(GGUISubMonoMiddayDungeonEnter _wnd) : base(_wnd)
        {
            initWnd();
        }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_ENTER, _onBtnBattle);
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_ENTER, _onBtnBattle);
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnBattle, _onBtnBattleClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.monoBox)
            {
                _m_boxWnd = new GGUISubWndMiddayDungeonBox(wnd.monoBox, _hideBoxWnd);
            }
            if (wnd.monoMiniBox)
            {
                _m_miniBoxWnd = new GGUISubWndMiddayDungeonMiniBox(wnd.monoMiniBox, _onBtnShowBoxClick);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnBattle, _onBtnBattleClick);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if (_m_boxWnd != null) 
                _m_boxWnd.hideWnd();
            
            if (_m_miniBoxWnd != null)
                _m_miniBoxWnd.showWnd();
            DateTime activityStartTime = TimeUtil.FromUTCMilliseconds(NPPlayer.instance.middayDungeonComp.startTimeMs);//活动开始时间
            DateTime activityEndTime = TimeUtil.FromUTCMilliseconds(NPPlayer.instance.middayDungeonComp.endTimeMs);//活动结束时间
            ALUGUICommon.setLabelTxt(wnd.txtMiddayDungeonDurationTime, TextTranslate.instance.getLanguage(
                TransKeyConst.midday_dungeon_playDuration_str_str, TimeUtil.DateTime2StringHM(activityStartTime), TimeUtil.DateTime2StringHM(activityEndTime), TimeUtil.getServerTimeZone()));
            
            bool isOpen = NPPlayer.instance.middayDungeonComp.isOpen;

            ALUGUICommon.setGameObjEnable(wnd.goShowInBattle, isOpen);
            ALUGUICommon.setGameObjEnable(wnd.goShowInPrepare, !isOpen);
            _m_timeDownSer = ALSerializeOpMgr.next();
            if(isOpen)
            {
                _refreshEndTimeDown(_m_timeDownSer);
            }
            else
            {
                
                _refreshTimeDown(_m_timeDownSer);
            }
        }
        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;

            long startLeftMs = NPPlayer.instance.middayDungeonComp.startTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (startLeftMs < 0)
                startLeftMs += 24 * 60 * 60 * 1000;
            ALUGUICommon.setLabelTxt(wnd.txtOpenCd,  TimeUtil.millisecondsToTime_hms(startLeftMs));
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }

        private void _refreshEndTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
           
            ALUGUICommon.setLabelTxt(wnd.txtEndCd,  TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_end_time_cd_desc, TimeUtil.millisecondsToTime_hms(NPPlayer.instance.middayDungeonComp.endTimeMs - FpsAndPingMgr.instance.serverTimeTag)));
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshEndTimeDown(_timeDownSer);
            },1f);
        }
        private void _onBtnBattle()
        {
            if (wnd != null) 
                _onBtnBattleClick(wnd.btnBattle);
        }
        private void _onBtnBattleClick(GameObject _)
        {
            if (NPPlayer.instance.middayDungeonComp.activityState != EMiddayDungeonActivityState.ONGOING)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_activity_no_open_tip);
                return;
            }
            if (NPPlayer.instance.middayDungeonComp.bossInfo == null 
                || GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(NPPlayer.instance.middayDungeonComp.bossInfo.wave) == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_boss_has_clear_tip) ;
                return;
            }
            GGUIWndMiddayDungeonBattle.instance.setInfo();
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMiddayDungeonBattle.instance, UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE, 0);
        }

        private void _onBtnShowBoxClick()
        {
            if (_m_boxWnd != null)
            {
                _m_boxWnd.showWnd();
            }
            if (_m_miniBoxWnd != null)
            {
                _m_miniBoxWnd.hideWnd();
            }
        }     

        private void _hideBoxWnd()
        {
            if (_m_boxWnd != null)
            {
                _m_boxWnd.hideWnd();
            }
            if (_m_miniBoxWnd != null)
            {
                _m_miniBoxWnd.showWnd();
            }
        }
    }
}