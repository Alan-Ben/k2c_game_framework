using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CommonConstructingMarsBuildingView : CommonConstructingMarsBuildingView<GTDMonoMarsBuildingConstructing>
    {
        public CommonConstructingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
    }
    public class CommonConstructingMarsBuildingView<T> : _AMarsBuildingView<T>, _IMarsBuildingView
        where T : GTDMonoMarsBuildingConstructing
    {
        private GGUIWndMarsBuildingHUDTimeFollowerController _m_timeWnd;
        private GGUICommonFollowTarget _m_followTarget;
        
        
        public CommonConstructingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
        

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
            GGUIWndMarsBuildingConstructing.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingConstructing.instance, GGUIWndMarsBuildingConstructing.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_CONSTRUCTING, false, false);
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

            _m_timeWnd?.discard();
            _m_timeWnd = null;
        }
    }
}