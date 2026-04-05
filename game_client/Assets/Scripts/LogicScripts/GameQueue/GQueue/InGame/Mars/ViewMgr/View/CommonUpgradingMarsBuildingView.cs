using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CommonUpgradingMarsBuildingView : CommonUpgradingMarsBuildingView<GTDMonoMarsBuildingUpgrading>
    {
        public CommonUpgradingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
    }
    public class CommonUpgradingMarsBuildingView<T> : _AMarsBuildingView<T>, _IMarsBuildingView 
        where T : GTDMonoMarsBuildingUpgrading
    {
        private GGUIWndMarsBuildingHUDTimeFollowerController _m_timeWnd;
        private GGUIWndMarsBuildingRedTipHUDFollowerController _m_redTipWnd;
        private GGUICommonFollowTarget _m_followTarget;
        private GGUICommonFollowTarget _m_redTipFollowTarget;
        
        
        public CommonUpgradingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
        
        
        public GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        

        public override void tick()
        {
            _m_timeWnd?.tick();
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
        public virtual void triggerClick()
        {
            GGUIWndMarsBuildingUpgrading.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingUpgrading.instance, GGUIWndMarsBuildingUpgrading.instance.showWnd, 
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADING, false, false);
        }


        protected override void _onInitDone()
        {
            if (mono == null)
                return;

            if (mono.monoClick != null)
                mono.monoClick.onClick += triggerClick;
            if (mono.timeFollowTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(mono.timeFollowTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_followTarget);

                _m_timeWnd = new GGUIWndMarsBuildingHUDTimeFollowerController();
                _m_timeWnd.refreshWnd(buildingInfo);
                GGUIWndMarsHud.instance.addController(_m_followTarget, _m_timeWnd);
            }
            
            if (mono.redTipTarget != null)
            {
                _m_redTipFollowTarget = new GGUICommonFollowTarget(mono.redTipTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_redTipFollowTarget);

                _m_redTipWnd = new GGUIWndMarsBuildingRedTipHUDFollowerController(new GResPathIndex(7193));
                _m_redTipWnd.refreshWnd(this);
                GGUIWndMarsHud.instance.addController(_m_redTipFollowTarget, _m_redTipWnd);
            }
            
            if(mono.monoMarsIntelligentControlEffectShow != null)
                mono.monoMarsIntelligentControlEffectShow.playInitAni();
        }
        protected override void _onDiscard()
        {
            if (mono == null)
                return;
            
            if (mono.monoClick != null)
                mono.monoClick.onClick -= triggerClick;
            if (_m_followTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }

            if (_m_redTipFollowTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_redTipFollowTarget);
                _m_redTipFollowTarget.discard();
                _m_redTipFollowTarget = null;
            }

            _m_timeWnd?.discard();
            _m_timeWnd = null;
            
            _m_redTipWnd?.discard();
            _m_redTipWnd = null;
        }
    }
}