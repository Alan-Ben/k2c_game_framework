using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndEarningGoalSelf : _ATALBasicUIWnd<GGUIMonoEarningGoalSelf>
    {
        private static GGUIWndEarningGoalSelf _g_instance = new GGUIWndEarningGoalSelf();
    
        public static GGUIWndEarningGoalSelf instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndEarningGoalSelf();
                return _g_instance;
            }
        }

        private AchieveInfo _m_info;//成就信息
        private GGUIWndAchieveStepGrid _m_wAchieveStepGrid;//成就步骤列表
        public GGUIWndEarningGoalSelf() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoEarningGoalSelf.assetPath; }
        protected override string _monoObjName { get => GGUIMonoEarningGoalSelf.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);

        }
    
        protected override void _onHideWnd()
        {
            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.hideWnd();
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
        }
    
        protected override void _onReset()
        {
            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.discard();
            _m_wAchieveStepGrid = null;
            if (wnd != null) ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoStepGrid != null)
                _m_wAchieveStepGrid = new GGUIWndAchieveStepGrid(wnd.monoStepGrid);
            _m_info = NPPlayer.instance.achieveComp.getAchimentInfo(GRefdataCoreMgr.instance.npGeneral.earning_goal_achieve_id);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_info == null || _m_info.achieveRefObj == null)
                return;
            if (_m_wAchieveStepGrid != null)
            {
                _m_wAchieveStepGrid.showWnd();
                _m_wAchieveStepGrid.setShowData(_m_info);
            }
        }
        
        //成就信息变更
        private void _onAchieveInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_info == null)
                return;

            AchieveInfo info = (AchieveInfo) _objects[0];
            if(info != null && info.achieveId == _m_info.achieveId)
                _refreshWnd();
        }
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EARNING_GOAL_SELF_REWARD);
        }
    }
}