using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CommonUnbuiltMarsBuildingView : CommonUnbuiltMarsBuildingView<GTDMonoMarsBuildingUnbuilt>
    {
        public CommonUnbuiltMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
    }
    public class CommonUnbuiltMarsBuildingView<T> : _AMarsBuildingView<T>, _IMarsBuildingView
        where T : GTDMonoMarsBuildingUnbuilt
    {
        private GGUIWndMarsBuildingBuildBtnFollowerController _m_buildBtnWnd;
        private GGUIWndMarsBuildingRedTipHUDFollowerController _m_redTipWnd;
        private GGUICommonFollowTarget _m_followTarget;
        private GGUICommonFollowTarget _m_redTipFollowTarget;
        
        
        public CommonUnbuiltMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
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
        public virtual void triggerClick()
        {
            // 必须建造按钮存在才能打开建造界面
            if (_m_buildBtnWnd == null)
                return;
            
            GGUIWndMarsBuildingBuild.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingBuild.instance, GGUIWndMarsBuildingBuild.instance.showWnd, 
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_BUILD, false, false);
        }
        
        
        protected override void _onInitDone()
        {
            if (mono == null)
                return;

            if (mono.monoClick != null)
                mono.monoClick.onClick += triggerClick;
            if (mono.buildBtnTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(mono.buildBtnTarget, Vector3.zero);
                GGUIWndMarsHud.instance.regInstance(_m_followTarget);
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
            
            _refreshBuildBtnState();
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingBuilt;  
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshBuildBtnState);
        }
        protected override void _onDiscard()
        {
            if (mono == null)
                return;
            
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingBuilt;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshBuildBtnState);

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

            _m_buildBtnWnd?.discard();
            _m_buildBtnWnd = null;
            
            _m_redTipWnd?.discard();
            _m_redTipWnd = null;
        }
        
        
        private void _onBuildingBuilt(long _, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            // 如果建筑是从建造完成确认态变为普通态，说明建筑已经建造完成，刷新建造按钮的状态
            if (_oldState == MarsBuildingInfo.StateType.Constructing && _newState == MarsBuildingInfo.StateType.Normal)
                _refreshBuildBtnState();
        }
        private void _refreshBuildBtnState()
        {
            if (mono == null)
                return;
            
            bool canShow = buildingInfo.checkCanShowBuildBtn();
            if (canShow)
            {
                if (_m_buildBtnWnd == null)
                {
                    _m_buildBtnWnd = new GGUIWndMarsBuildingBuildBtnFollowerController();
                    _m_buildBtnWnd.refreshWnd(this);
                    GGUIWndMarsHud.instance.addController(_m_followTarget, _m_buildBtnWnd);
                }
            }
            else
            {
                if (_m_buildBtnWnd != null)
                {
                    _m_buildBtnWnd.discard();
                    _m_buildBtnWnd = null;
                }
            }
        }
    }
}