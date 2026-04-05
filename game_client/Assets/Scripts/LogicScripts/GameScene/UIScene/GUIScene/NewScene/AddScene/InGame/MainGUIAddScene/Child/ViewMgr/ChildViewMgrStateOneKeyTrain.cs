
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChildViewMgr
    {
        public class ChildViewMgrStateOneKeyTrain : _ASimpleState<ChildViewMgrType>
        {
            [NotNull] private readonly ChildViewMgr _m_viewMgr;
            
            
            public ChildViewMgrStateOneKeyTrain([NotNull] ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
            }

            
            public override ChildViewMgrType state { get { return ChildViewMgrType.OneKeyTrain; } }


            protected override void _onEnter()
            {
                ChildInfo childInfo = _m_viewMgr.curSelectSeatInfo?.childInfo;
                if (childInfo == null)
                {
                    _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                    return;
                }
                
                GGUIWndChildMain.instance.refreshWnd();
                new _EducateTask(this).deal();
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(ChildViewMgrType _newState)
            {
                return true;
            }


            private class _EducateTask : _IALBaseMonoTask
            {
                [NotNull] private readonly ChildViewMgrStateOneKeyTrain _m_state;
                [NotNull] private readonly ChildViewMgr _m_viewMgr;
                private readonly int _m_stateSerialize;
                
                
                public _EducateTask([NotNull] ChildViewMgrStateOneKeyTrain _state)
                {
                    _m_state = _state;
                    _m_viewMgr = _state._m_viewMgr;
                    _m_stateSerialize = _state.enterSerialize;
                }
                
                
                public void deal()
                {
                    if (_m_stateSerialize != _m_state.enterSerialize)
                        return;
                    
                    if (!_isEnable())
                    {
                        _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                        return;
                    }
                
                    ChildInfo childInfo = _m_viewMgr.curSelectSeatInfo?.childInfo;
                    if (childInfo == null)
                    {
                        _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                        return;
                    }
                    
                    NPPlayer.instance.childComp.reqTrainChild(childInfo.id, (_isSuc, _msg) =>
                    {
                        // 如果发现当前选中的席位已经变了，就刷新一次界面后不管了（btw 一般不会出现这种问题）
                        if (childInfo.seatInfo != _m_viewMgr.curSelectSeatInfo)
                        {
                            GGUIWndChildMain.instance.refreshWnd();
                            return;
                        }

                        if (_isSuc)
                        {
                            if (childInfo.canGraduate())
                            {
                                _m_viewMgr._m_stateMachine.changeState(new ChildViewMgrStateIdle(_m_viewMgr));
                            }
                            else
                            {
                                if (childInfo.canStepUp())
                                {
                                    _m_viewMgr.refreshClassroomView();

                                    GGUIWndChildMain.instance.refreshWnd();
                                }
                                else
                                {
                                    GGUIWndChildMain.instance.refreshProgress();
                                    GGUIWndChildMain.instance.refreshBrainValue();
                                }
                            }

                            GGUIWndChildMain.instance.showContainerExpCollect(childInfo, _msg.getGainValue());
                            _m_viewMgr._m_classroomView?.playEducateSfx();
                            GGUIWndChildMain.instance.playEducateSfx(childInfo);
                            if (_msg.getAddBonus() > 0)
                                GGUIWndChildMain.instance.refreshChildInfo();
                            GGUIWndChildMain.instance.refreshContainerItem(childInfo);
                        }

                        float space = GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_spaceS;
                        if (space <= 0)
                            space = 0.5f;

                        ALMonoTaskMgr.instance.addMonoTask(this, space);
                    });
                }
                private bool _isEnable()
                {
                    ChildInfo childInfo = _m_viewMgr.curSelectSeatInfo?.childInfo;
                    if (childInfo == null)
                        return false;
                    
                    if (string.IsNullOrEmpty(childInfo.name))
                        return false;
                
                    if (childInfo.seatInfo.energy <= 0)
                        return false;

                    if (childInfo.canGraduate())
                        return false;

                    if (!GCommon.isItemEnough(childInfo.getEducationCost(), false))
                        return false;
                
                    return true;
                }
            }
        }
    }
}