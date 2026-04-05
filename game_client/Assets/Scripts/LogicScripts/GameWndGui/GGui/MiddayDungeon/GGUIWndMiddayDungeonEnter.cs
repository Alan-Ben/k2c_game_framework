using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndMiddayDungeonEnter : _ATALBasicUIWnd<GGUIMonoMiddayDungeonEnter>
    {
        private static GGUIWndMiddayDungeonEnter _g_instance = new GGUIWndMiddayDungeonEnter();
    
        public static GGUIWndMiddayDungeonEnter instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonEnter();
                return _g_instance;
            }
        }
        
        private GGUISubWndMiddayDungeonMiniBox _m_miniBoxWnd = null;
        private GGUISubWndMiddayDungeonBox _m_boxWnd = null;
        private int _m_timeDownSer;
    
        public GGUIWndMiddayDungeonEnter() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonEnter.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonEnter.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBattle, _onBtnBattleClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onBtnRankClick);
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
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBattle, _onBtnBattleClick);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onBtnRankClick);
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

            bool isOpen = NPPlayer.instance.middayDungeonComp.isOpen;

            ALUGUICommon.setGameObjEnable(wnd.goShowInBattle, isOpen);

            bool isPreview = NPPlayer.instance.middayDungeonComp.isPreview;
            ALUGUICommon.setGameObjEnable(wnd.goShowInPrepare, isPreview);
            _m_timeDownSer = ALSerializeOpMgr.next();
            if (isPreview)
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
           
            ALUGUICommon.setLabelTxt(wnd.txtOpenCd,  TimeUtil.millisecondsToTime_hms(NPPlayer.instance.middayDungeonComp.startTimeMs - FpsAndPingMgr.instance.serverTimeTag));
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }
        
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_ENTER);
        }
        
        private void _onBtnBattleClick(GameObject _)
        {
            if (NPPlayer.instance.middayDungeonComp.activityState != EMiddayDungeonActivityState.ONGOING)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_activity_no_open_tip);
                return;
            }
            GGUIWndMiddayDungeonBattle.instance.setInfo();
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMiddayDungeonBattle.instance, UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE, 0);
        }
        //点击排行榜
        private void _onBtnRankClick(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonRank.instance, GGUIWndMiddayDungeonRank.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_RANK);

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