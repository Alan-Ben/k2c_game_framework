using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 杰出者详情
    /// </summary>
    public class GGUIWndGravePlayerDetail : _ATALBasicUIWnd<GGUIMonoGravePlayerDetail>
    {
        private static GGUIWndGravePlayerDetail _g_instance = new GGUIWndGravePlayerDetail();
    
        public static GGUIWndGravePlayerDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGravePlayerDetail();
                return _g_instance;
            }
        }
        private int _m_graveTypeId = -1;  // 杰出者类型ID
        private NPCommonSimplePlayerInfo _m_playerInfo;
        private long _m_titleId; 
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        private NPGGUIWndCommonShowCase _m_playerShowcaseWnd;  // 玩家形象显示
        // </AutoGen:WndDeclaration>
        private GGUIWndSubPlayerTitle _m_playerTitleWnd;

        public GGUIWndGravePlayerDetail() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGravePlayerDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGravePlayerDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            _m_playerShowcaseWnd?.showWnd();
            // </AutoGen:_onShowWnd>
            _m_playerTitleWnd?.showWnd();
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            _m_playerShowcaseWnd?.hideWnd();
            // </AutoGen:_onHideWnd>
            _m_playerTitleWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            _m_playerShowcaseWnd?.resetWnd();
            // </AutoGen:_onReset>
            _m_playerTitleWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            _m_playerShowcaseWnd?.discard();
            _m_playerShowcaseWnd = null;
            // </AutoGen:_onDiscard>
            _m_playerTitleWnd?.discard();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnHonorLog, _onClickbtnHonorLog);
                ALUGUICommon.uncombineBtnClick(wnd.btnVisit, _onClickbtnVisit);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            if (wnd.playerShowcase != null)
                _m_playerShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.playerShowcase);
            // </AutoGen:_onWndInitDone>
            if(wnd.monoSubPlayerTitle != null)
                _m_playerTitleWnd = new GGUIWndSubPlayerTitle(wnd.monoSubPlayerTitle);
            
            ALUGUICommon.combineBtnClick(wnd.btnHonorLog, _onClickbtnHonorLog);
            ALUGUICommon.combineBtnClick(wnd.btnVisit, _onClickbtnVisit);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
        }

        public void setInfo(int _graveTypeId, NPCommonSimplePlayerInfo _playerInfo, long _titleId = 0)
        {
            _m_graveTypeId = _graveTypeId;
            _m_playerInfo = _playerInfo;
            _m_titleId = _titleId;
            _refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            // <AutoGen:_refreshWnd>
            // <UserCode name="playerInfo">
            // </UserCode>
            // <UserCode name="playerShowcase">
          
            // </UserCode>
            // <UserCode name="hasPlayerShowGos">
            // </UserCode>
            // <UserCode name="noPlayerShowGos">
            // </UserCode>
            // </AutoGen:_refreshWnd>
            
            bool hasPlayer = _m_playerInfo != null;
            if (_m_playerInfo != null)
            {
                if(_m_playerInfoWnd != null)
                {
                    _m_playerInfoWnd.showWnd();
                    _m_playerInfoWnd.setPlayerInfo(_m_playerInfo);
                }
                if(_m_playerShowcaseWnd != null)
                {
                    _m_playerShowcaseWnd.showWnd(new ShowCaseCommonResUnitInfoObj(_m_playerInfo.skinRef?.td_show));
                }
            }
            else
            {
                if(_m_playerInfoWnd != null)
                {
                    _m_playerInfoWnd.hideWnd();
                }
                if(_m_playerShowcaseWnd != null)
                {
                    _m_playerShowcaseWnd.hideWnd();
                }
            }
            if (null != _m_playerTitleWnd)
                _m_playerTitleWnd.setInfo(_m_titleId, 1);
            ALUGUICommon.setGameObjEnable(wnd.hasPlayerShowGos, hasPlayer);
            ALUGUICommon.setGameObjEnable(wnd.noPlayerShowGos, !hasPlayer);

        }
        
        // <AutoGen:Method>
        // 荣誉记录点击事件
        private void _onClickbtnHonorLog(GameObject go)
        {
            // <UserCode name="btnHonorLog">
            GGUIWndGraveHonorLog.instance.setInfo(_m_graveTypeId);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGraveHonorLog.instance, GGUIWndGraveHonorLog.instance.showWnd, UINodeTagConst.C_GRAVE_HONOR_LOG);
            // </UserCode>
        }
        // 拜访按钮点击事件
        private void _onClickbtnVisit(GameObject go)
        {
            // <UserCode name="btnVisit">
            FriendCommon.showPlayerInfo(_m_playerInfo);
            // </UserCode>
        }
        // </AutoGen:Method>

        private void _onClickBtnClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GRAVE_PLAYER_DETAIL);
        }
    }
}