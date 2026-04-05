using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChildViewMgr
    {
        public class ChildViewMgrStateOneKeyTrainAll : _ASimpleState<ChildViewMgrType>
        {
            [NotNull] private readonly ChildViewMgr _m_viewMgr;
            
            
            public ChildViewMgrStateOneKeyTrainAll([NotNull] ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
            }


            public override ChildViewMgrType state { get { return ChildViewMgrType.OneKeyTrainAll; } }


            protected override void _onEnter()
            {
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
                [NotNull] private readonly ChildViewMgrStateOneKeyTrainAll _m_state;
                [NotNull] private readonly ChildViewMgr _m_viewMgr;
                [NotNull] private readonly ALStepCounter _m_stepCounter;
                private readonly int _m_stateSerialize;
                
                
                public _EducateTask([NotNull] ChildViewMgrStateOneKeyTrainAll _state)
                {
                    _m_state = _state;
                    _m_viewMgr = _state._m_viewMgr;
                    _m_stateSerialize = _state.enterSerialize;
                    _m_stepCounter = new ALStepCounter();
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

                    _m_stepCounter.resetAll();
                    _m_stepCounter.chgTotalStepCount(_m_viewMgr._m_seatList.Count);
                    _m_stepCounter.regAllDoneDelegate(() =>
                    {
                        float space = GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_spaceS;
                        if (space <= 0)
                            space = 0.5f;
                        
                        ALMonoTaskMgr.instance.addMonoTask(this, space);
                    });

                    foreach (SeatInfo seatInfo in _m_viewMgr._m_seatList)
                    {
                        ChildInfo childInfo = seatInfo.childInfo;
                        if (childInfo == null || childInfo.canGraduate() || string.IsNullOrEmpty(childInfo.name))
                        {
                            _m_stepCounter.addDoneStepCount();
                            continue;
                        }

                        if (seatInfo.energy <= 0)
                        {
                            _m_stepCounter.addDoneStepCount();
                            continue;
                        }

                        if (!GCommon.isItemEnough(childInfo.getEducationCost(), false))
                        {
                            _m_stepCounter.addDoneStepCount();
                            continue;
                        }
                        
                        NPPlayer.instance.childComp.reqTrainChild(childInfo.id, (_isSuc, _msg) =>
                        {
                            if (_isSuc)
                            {
                                // 如果是当前显示的子嗣，并且升学了，要变更整个界面和刷新 3d 模型
                                if (childInfo.canStepUp())
                                {
                                    if (childInfo.seatInfo == _m_viewMgr.curSelectSeatInfo)
                                    {
                                        _m_viewMgr.refreshClassroomView();
                                    }

                                    GGUIWndChildMain.instance.refreshWnd();
                                }
                                else if (childInfo.canGraduate())
                                {
                                    GGUIWndChildMain.instance.refreshWnd();
                                }
                                else
                                {
                                    GGUIWndChildMain.instance.refreshProgress();
                                    GGUIWndChildMain.instance.refreshBrainValue();
                                }

                                GGUIWndChildMain.instance.showContainerExpCollect(childInfo, _msg.getGainValue());
                                if (childInfo.seatInfo == _m_viewMgr.curSelectSeatInfo)
                                    _m_viewMgr._m_classroomView?.playEducateSfx();
                                GGUIWndChildMain.instance.playEducateSfx(childInfo);
                                // if (_msg.getAddBonus() > 0)
                                //     GGUIWndChildMain.instance.refreshContainerItem(childInfo);
                                GGUIWndChildMain.instance.refreshContainerItem(childInfo);
                            }

                            _m_stepCounter.addDoneStepCount();
                        });
                    }
                }
                private bool _isEnable()
                {
                    bool isAllEnergyEmpty = true;
                    bool noChildren = true;
                    bool noCostItem = true;
                    foreach (SeatInfo seatInfo in _m_viewMgr.seatList)
                    {
                        if (seatInfo.childInfo != null && !seatInfo.childInfo.canGraduate() && !string.IsNullOrEmpty(seatInfo.childInfo.name))
                        {
                            noChildren = false;
                            if (GCommon.isItemEnough(seatInfo.childInfo.getEducationCost(), false))
                                noCostItem = false;
                            if (seatInfo.energy > 0)
                                isAllEnergyEmpty = false;
                        }
                    }

                    if (noChildren)
                        return false;

                    if (isAllEnergyEmpty)
                        return false;
                    
                    if (noCostItem)
                        return false;

                    return true;
                }
            }
        }
    }
}