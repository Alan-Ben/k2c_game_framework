using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class BusinessBuildingView : _AALBasicLoadObj
    {
        [NotNull] private readonly BusinessBuildingInfo _m_buildingInfo;
        private readonly Vector3 _m_position;
        private readonly NPGGoIndex _m_resIndex;
        private int _m_level;

        private GTDMonoBusinessBuilding _m_mono;
        private GGUICommonFollowTarget _m_nameFollowTarget;
        private GGUICommonFollowTarget _m_earningsFollowTarget;
        private GGUIWndBusinessBuildingEntranceFollowItemController _m_nameFollower;
        
        public BusinessBuildingView([NotNull] BusinessBuildingInfo _buildingInfo, Vector3 _position)
        {
            _m_buildingInfo = _buildingInfo;
            _m_position = _position;
            _m_resIndex = _buildingInfo.getCurrentResIndex();
            _m_level = _buildingInfo.level;
        }
        

        public int level { get { return _m_level; } }
        public Vector3 position { get { return _m_position; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        

        public void refresh()
        {
            _m_level = _m_buildingInfo.level;
            _m_nameFollower?.refreshWnd();
        }
        public void showEarningTip(float _deltaTime)
        {
            if (_m_earningsFollowTarget == null || _m_buildingInfo.earningsPerS <= 0)
                return;
            
            GGUIWndBuildingCoinEarningTipFollowItemController earningTip = new GGUIWndBuildingCoinEarningTipFollowItemController();
            earningTip.setCoinNum((long)(_m_buildingInfo.earningsPerS * _deltaTime));
            _m_earningsFollowTarget.addController(earningTip);
        }
        

        protected override void _loadOp()
        {
            MainAdditionBuildingTDScene.instance.createBuilding<GTDMonoBusinessBuilding>(_m_resIndex, _m_position, _mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }
                
                _m_mono = _mono;

                if (_m_mono.nameHudTarget != null)
                {
                    _m_nameFollowTarget = new GGUICommonFollowTarget(_m_mono.nameHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_nameFollowTarget);
                }
                if (_m_mono.earningsHudTarget != null)
                {
                    _m_earningsFollowTarget = new GGUICommonFollowTarget(_m_mono.earningsHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_earningsFollowTarget);
                }
                if (_m_mono.clickMono != null)
                    _m_mono.clickMono.onClick += _onBuildingClick;

                if (_m_nameFollowTarget != null)
                {
                    _m_nameFollower = new GGUIWndBusinessBuildingEntranceFollowItemController();
                    _m_nameFollower.setBuildingInfo(_m_buildingInfo);
                    _m_nameFollowTarget.addController(_m_nameFollower);
                }
                
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            if (_m_mono == null)
                return;
            
            _m_nameFollowTarget?.discard();
            _m_earningsFollowTarget?.discard();
            _m_nameFollowTarget = null;
            _m_earningsFollowTarget = null;
            _m_nameFollower = null;
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onBuildingClick;
            
            MainAdditionBuildingTDScene.instance.discardBuilding<GTDMonoBusinessBuilding>(_m_resIndex, _m_mono);
            _m_mono = null;
        }
        
        
        internal void _refreshCanSettleHero()
        {
            _m_nameFollower?.refreshCanSettleHero();
        }
        
        
        private void _onBuildingClick()
        {
            PlayAudioMgr.instance.playClip(_m_buildingInfo.baseRef.open_building_audio_id);
            GGUIWndBusinessBuilding.instance.refreshWnd(_m_buildingInfo);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndBusinessBuilding.instance, UINodeTagConst.C_BUILDING_MAIN, null, null, 0);
        }
    }
}