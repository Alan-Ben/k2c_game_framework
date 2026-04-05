using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndEarningGoalSelfPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEarningGoalSelf>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        private AchieveInfo _m_info;//成就信息
        private GGUIWndAchieveStepGrid _m_wAchieveStepGrid;//成就步骤列表
        public GGUIWndEarningGoalSelfPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
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
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoStepGrid != null)
                _m_wAchieveStepGrid = new GGUIWndAchieveStepGrid(wnd.monoStepGrid);
            _m_info = NPPlayer.instance.achieveComp.getAchimentInfo(GRefdataCoreMgr.instance.npGeneral.earning_goal_achieve_id);
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
    }
}