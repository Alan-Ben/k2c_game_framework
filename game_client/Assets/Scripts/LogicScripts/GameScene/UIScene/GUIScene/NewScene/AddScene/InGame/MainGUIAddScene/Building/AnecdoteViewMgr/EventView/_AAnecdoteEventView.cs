using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AAnecdoteEventView<T_INFO, T_TD_MONO> : _AAnecdoteEventView
        where T_INFO : _AAnecdoteEventInfo
        where T_TD_MONO : GTDMonoAnecdoteEvent
    {
        [NotNull] private readonly T_INFO _m_eventInfo;
        private T_TD_MONO _m_mono;
        private GGUICommonFollowTarget _m_followTarget;
        
        
        protected _AAnecdoteEventView([NotNull] T_INFO _eventInfo)
            : base(_eventInfo)
        {
            _m_eventInfo = _eventInfo;
        }
        
        
        [NotNull] public new T_INFO eventInfo { get { return _m_eventInfo; } }
        public T_TD_MONO mono { get { return _m_mono; } }
        public GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        
        
        protected abstract void _onInit();
        protected abstract void _onDiscard();
        protected abstract void _dealTypicalEventProcess(Action _complete);
        
        protected void _init()
        {
            _onInit();
            _setLoadDone();
        }
        protected sealed override void _discard()
        {
            _onDiscard();

            _m_followTarget?.discard();
            _m_followTarget = null;
            if (_m_mono != null && _m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= onEventClick;
            
            MainAdditionBuildingTDScene.instance.discardAnecdoteEvent(_m_eventInfo.eventRef.event_res_index, _m_mono);
            _m_mono = null;
        }
        protected sealed override void _loadOp()
        {
            MainAdditionBuildingTDScene.instance.createAnecdoteEvent<T_TD_MONO>(_m_eventInfo.eventRef.event_res_index, _m_eventInfo.posId,
                _mono =>
                {
                    if (_mono == null)
                    {
                        _init();
                        return;
                    }

                    _m_mono = _mono;
                    if (_m_mono.hudTarget != null)
                    {
                        _m_followTarget = new GGUICommonFollowTarget(_m_mono.hudTarget, Vector3.zero);
                        GGUIWndBuildingFollow.instance.regInstance(_m_followTarget);
                    }
                    if (_m_mono.clickMono != null)
                        _m_mono.clickMono.onClick += onEventClick;

                    _init();
                });
        }
        protected virtual void dealEventStart(Action _complete)
        {
            if (_m_eventInfo.eventRef.start_type != EAnecdoteEventType.SPECIAL)
            {
                _complete?.Invoke();
                return;
            }
            
            QueueMgr.instance.AddNode(new GNodeAnecdoteBanner(GGUIWndAnecdoteBannerShowType.SPECIAL_EVENT, _complete));
        }
        protected virtual void dealBeforeDialogue(Action _complete)
        {
            GCommon.enterDialogueNode(_m_eventInfo.eventRef.before_dialogue_id, _complete);
        }
        protected virtual void dealAfterDialogue(Action _complete)
        {
            GCommon.enterDialogueNode(_m_eventInfo.eventRef.after_dialogue_id, _complete);
        }
        protected virtual void dealEventEnd(Action _complete)
        {
            if (_m_eventInfo.eventRef.end_type == EAnecdoteEventEndType.TO_BE_CONTINUED)
            {
                QueueMgr.instance.AddNode(new GNodeAnecdoteBanner(GGUIWndAnecdoteBannerShowType.TO_BE_CONTINUED, _complete));
                return;
            }

            if (_m_eventInfo.eventRef.end_type == EAnecdoteEventEndType.END)
            {
                QueueMgr.instance.AddNode(new GNodeAnecdoteBanner(GGUIWndAnecdoteBannerShowType.EVENT_ENDED, _complete));
                return;
            }
            
            _complete?.Invoke();
        }
     
        
        protected sealed override void onEventClick()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addProcess(NPPlayer.instance.anecdoteComp.setStartEventProcess)
                .addDelegateProcess(dealEventStart)
                .addDelegateProcess(dealBeforeDialogue)
                .addDelegateProcess(_dealTypicalEventProcess)
                .addDelegateProcess(dealAfterDialogue)
                .addDelegateProcess(dealEventEnd)
                .addProcess(NPPlayer.instance.anecdoteComp.setEndEventProcess);
            process.dealProcess();
        }
    }
    public abstract class _AAnecdoteEventView : _AALBasicLoadObj
    {
        public static _AAnecdoteEventView createEventView(_AAnecdoteEventInfo _eventInfo)
        {
            return _eventInfo switch
            {
                AnecdoteEventChoiceInfo choiceInfo => new AnecdoteEventChoiceView(choiceInfo),
                AnecdoteEventEarningsInfo earningsInfo => new AnecdoteEventEarningsView(earningsInfo),
                AnecdoteEventRewardInfo rewardInfo => new AnecdoteEventRewardView(rewardInfo),
                _ => null
            };
        }


        [NotNull] private readonly _AAnecdoteEventInfo _m_eventInfo;
        

        protected _AAnecdoteEventView([NotNull] _AAnecdoteEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
        }
        
        
        public _AAnecdoteEventInfo eventInfo { get { return _m_eventInfo; } }
        
        
        protected abstract void onEventClick();
        
        
        internal void _simulateClick()
        {
            onEventClick();
        }
    }
}