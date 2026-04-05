using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class AnecdoteEventEarningsView : _AAnecdoteEventView<AnecdoteEventEarningsInfo, GTDMonoAnecdoteEvent>
    {
        private _AALGGUICommonFollowItemController _m_entranceController;
        private EState _m_state;
        
        
        public AnecdoteEventEarningsView([NotNull] AnecdoteEventEarningsInfo _eventInfo) 
            : base(_eventInfo)
        {
        }
        

        protected override void _onInit()
        {
            eventInfo.onDrawFirstReward += _onDrawFirstReward;
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _onEarningsChg;

            _refreshState();
            _refreshHud();
        }
        protected override void _onDiscard()
        {
            eventInfo.onDrawFirstReward -= _onDrawFirstReward;
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
            
            _m_entranceController?.discard();
            _m_entranceController = null;
        }
        protected override void dealEventStart(Action _complete)
        {
            if (_m_state != EState.INIT)
            {
                _complete?.Invoke();
                return;
            }
            
            base.dealEventStart(_complete);
        }
        protected override void dealBeforeDialogue(Action _complete)
        {
            if (_m_state != EState.INIT)
            {
                _complete?.Invoke();
                return;
            }
            
            base.dealBeforeDialogue(_complete);
        }
        protected override void _dealTypicalEventProcess(Action _complete)
        {
            List<NPCommon_ItemInfo> rewardList = null;
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(_dealDone =>
                {
                    if (_m_state == EState.INIT)
                    {
                        int serialize = MainCameraMono.selfInstance.openAllInputMask();
                        eventInfo.reqDrawFirstReward((_isSuc, _msg) =>
                        {
                            MainCameraMono.selfInstance.closeAllInputMask(serialize);
                            GCommon.dealGainItem(_msg?.getItemList(), TransKeyConst.common_getreward_tip, _dealDone);
                        });
                    }
                    else
                    {
                        _dealDone?.Invoke();
                    }
                })
                .addDelegateProcess(_dealDone =>
                {
                    QueueMgr.instance.AddNode(new GNodeAnecdoteEventEarnings(eventInfo, (_rewardList) =>
                    {
                        rewardList = _rewardList;
                    }, _dealDone));
                })
                .addDelegateProcess(_dealDone =>
                {
                    GCommon.dealGainItem(rewardList, TransKeyConst.common_getreward_tip, _dealDone);
                })
                .addProcess(_complete);
            process.dealProcess();
        }
        protected override void dealAfterDialogue(Action _complete)
        {
            if (_m_state != EState.COMPLETE)
            {
                _complete?.Invoke();
                return;
            }
            
            base.dealAfterDialogue(_complete);
        }
        protected override void dealEventEnd(Action _complete)
        {
            if (_m_state != EState.COMPLETE)
            {
                _complete?.Invoke();
                return;
            }
            
            base.dealEventEnd(_complete);
        }


        private bool _refreshState()
        {
            EState state = EState.INIT;
            if (eventInfo.hasDrawFirstReward)
                state = EState.DRAWN;
            if (eventInfo.canGainFinalReward)
                state = EState.COMPLETE;
            
            bool isChanged = state != _m_state;
            _m_state = state;
            return isChanged;
        }
        private void _refreshHud()
        {
            _m_entranceController?.discard();

            switch (_m_state)
            {
                case EState.INIT:
                    if (eventInfo.eventRef.start_type == EAnecdoteEventType.SPECIAL)
                    {
                        GGUIWndAnecdoteSpecialEventFollowerController specialController = new GGUIWndAnecdoteSpecialEventFollowerController();
                        specialController.refreshWnd(eventInfo);
                        specialController.setTriggerAction(onEventClick);
                        _m_entranceController = specialController;
                    }
                    else
                    {
                        GGUIWndAnecdoteNormalEventFollowerController normalController = new GGUIWndAnecdoteNormalEventFollowerController();
                        normalController.setTriggerAction(onEventClick);
                        _m_entranceController = normalController;
                    }
                    break;
                case EState.DRAWN:
                {
                    GGUIWndAnecdoteEarningProgressFollowerController progressController = new GGUIWndAnecdoteEarningProgressFollowerController();
                    progressController.refreshWnd(NPPlayer.instance.specialItemComp.goldData.earnings, eventInfo.typeRef.earnings, eventInfo);
                    progressController.setTriggerAction(onEventClick);
                    _m_entranceController = progressController;
                    break;
                }
                case EState.COMPLETE:
                {
                    GGUIWndAnecdoteEarningCompleteFollowerController completeController = new GGUIWndAnecdoteEarningCompleteFollowerController();
                    completeController.refreshWnd(eventInfo);
                    completeController.setTriggerAction(onEventClick);
                    _m_entranceController = completeController;
                    break;
                }
                default:
                    _m_entranceController = null;
                    break;
            }
            
            followTarget?.addController(_m_entranceController);
        }

        private void _onDrawFirstReward()
        {
            if (_refreshState())
                _refreshHud();
        }
        private void _onEarningsChg()
        {
            if (_m_entranceController is GGUIWndAnecdoteEarningProgressFollowerController progressWnd)
                progressWnd.setEarnings(NPPlayer.instance.specialItemComp.goldData.earnings, eventInfo.typeRef.earnings);
            
            if (_refreshState())
                _refreshHud();
        }
        

        private enum EState
        {
            INIT,
            DRAWN,
            COMPLETE,
        }
    }
}