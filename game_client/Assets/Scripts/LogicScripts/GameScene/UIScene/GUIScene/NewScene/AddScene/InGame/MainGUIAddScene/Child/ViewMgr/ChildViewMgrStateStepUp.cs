
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChildViewMgr
    {
        public class ChildViewMgrStateStepUp : _ASimpleState<ChildViewMgrType>
        {
            [NotNull] private readonly ChildViewMgr _m_viewMgr;
            private bool _m_isAbort;
            
            
            public ChildViewMgrStateStepUp([NotNull] ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
            }


            public override ChildViewMgrType state { get { return ChildViewMgrType.StepUp; } }


            protected override void _onEnter()
            {
                ChildInfo childInfo = _m_viewMgr.curSelectSeatInfo?.childInfo;
                if (childInfo == null)
                {
                    _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                    return;
                }

                _m_isAbort = true;
                GGUIWndChildMain.instance.refreshWnd();
                int serialize = enterSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (serialize != enterSerialize)
                        return;

                    _m_isAbort = false;
                    _m_viewMgr.refreshClassroomView();
                    
                    if (AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CHILD_STEP_UP_SUCCESS))
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildStepUpSuccess(childInfo));
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildStepUpResult(childInfo));
                    
                    _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                }, GRefdataCoreMgr.instance.npGeneral.child_step_up_show_delayS);
            }
            protected override void _onExit()
            {
                if (!_m_isAbort)
                    return;
                
                _m_viewMgr.refreshClassroomView();
            }
            public override bool canEnterState(ChildViewMgrType _newState)
            {
                return true;
            }
        }
    }
}