using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CommonNormalMarsBuildingView : CommonNormalMarsBuildingView<GTDMonoMarsBuildingNormal>
    {
        public CommonNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
    }
    public class CommonNormalMarsBuildingView<T> : _AMarsBuildingView<T>, _IMarsBuildingView
        where T : GTDMonoMarsBuildingNormal
    {
        private GGUIWndMarsBuildingHUDNameFollowerController _m_nameWnd;
        private GGUIWndMarsBuildingHUDNameFollowerController _m_levelWnd;
        private GGUIWndMarsBuildingRedTipHUDFollowerController _m_redTipWnd;
        private GGUICommonFollowTarget _m_followTarget;
        private GGUICommonFollowTarget _m_levelFollowTarget;
        private GGUICommonFollowTarget _m_redTipFollowTarget;
        

        protected GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        protected GGUICommonFollowTarget levelFollowTarget { get { return _m_levelFollowTarget; } }
        
        
        public CommonNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }

        public override void tick()
        {
        }

        public override void playIntelligentControlEffect(long _intelligentControlId, Action _complete = null)
        {
            if (mono == null || mono.monoMarsIntelligentControlEffectShow == null)
            {
                _complete?.Invoke();
            }
            else
            {
                mono.monoMarsIntelligentControlEffectShow.playEffect(_intelligentControlId, _complete);
            }
        }

        public void setSelected(bool _selected)
        {
            if (mono == null)
                return;

            mono.setSelectState(_selected);
        }
        public void triggerClick()
        {
            _triggerClickInternal();
            AccountSettingMgr.instance.unreadMarsBuildingChangeSaver?.removeUnreadBuildingId(buildingInfo.buildRefId);
        }
        
        
        protected override void _onInitDone()
        {
            if (mono == null)
                return;

            if (mono.monoClick != null)
                mono.monoClick.onClick += triggerClick;
            if (mono.nameTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(mono.nameTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_followTarget);

                _m_nameWnd = new GGUIWndMarsBuildingHUDNameFollowerController(buildingInfo.refObj.name_ui_path_id);
                _m_nameWnd.refreshWnd(this);
                GGUIWndMarsHud.instance.addAutoHideController(_m_followTarget, _m_nameWnd);
            }

            if (mono.levelTarget != null)
            {
                _m_levelFollowTarget = new GGUICommonFollowTarget(mono.levelTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_levelFollowTarget);

                _m_levelWnd = new GGUIWndMarsBuildingHUDNameFollowerController(new GResPathIndex(7119));
                _m_levelWnd.refreshWnd(this);
                GGUIWndMarsHud.instance.addController(_m_levelFollowTarget, _m_levelWnd);
            }
            
            if(mono.redTipTarget != null)
            {
                _m_redTipFollowTarget = new GGUICommonFollowTarget(mono.redTipTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_redTipFollowTarget);

                _m_redTipWnd = new GGUIWndMarsBuildingRedTipHUDFollowerController(new GResPathIndex(7193));
                _m_redTipWnd.refreshWnd(this);
                GGUIWndMarsHud.instance.addController(_m_redTipFollowTarget, _m_redTipWnd);
            }
            
            if(mono.monoMarsIntelligentControlEffectShow != null)
                mono.monoMarsIntelligentControlEffectShow.playInitAni();
            
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingBuilt;  
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshUpgradeState);
        }
        protected override void _onDiscard()
        {
            if (mono == null)
                return;
            
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingBuilt;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshUpgradeState);
            
            if (mono.monoClick != null)
                mono.monoClick.onClick -= triggerClick;
            if (_m_followTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }

            if (_m_levelFollowTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_levelFollowTarget);
                _m_levelFollowTarget.discard();
                _m_levelFollowTarget = null;
            }

            if (_m_redTipFollowTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_redTipFollowTarget);
                _m_redTipFollowTarget.discard();
                _m_redTipFollowTarget = null;
            }
            
            _m_nameWnd?.discard();
            _m_nameWnd = null;
            
            _m_levelWnd?.discard();
            _m_levelWnd = null;
            
            _m_redTipWnd?.discard();
            _m_redTipWnd = null;
        }
        protected virtual void _triggerClickInternal()
        {
            GGUIWndMarsHud.instance.showAutoHideController(_m_nameWnd);
            GGUIWndMarsBuildingInfo.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                EUIQueueStageType.MAIN ,UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
        }
        
        
        private void _onBuildingBuilt(long _, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            // 如果建筑是从建造完成确认态变为普通态，说明建筑已经建造完成，刷新建造按钮的状态
            if (_newState == MarsBuildingInfo.StateType.Normal)
                _refreshUpgradeState();
        }
        private void _refreshUpgradeState()
        {
            _m_nameWnd?.refreshUpgradeState();
            _m_levelWnd?.refreshUpgradeState();
        }
    }
}