using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPCommon;

namespace GOE
{
    public class AnecdoteEventChoiceView : _AAnecdoteEventView<AnecdoteEventChoiceInfo, GTDMonoAnecdoteEvent>
    {
        private _AALGGUICommonFollowItemController _m_entranceController;
        
        
        public AnecdoteEventChoiceView([NotNull] AnecdoteEventChoiceInfo _eventInfo) 
            : base(_eventInfo)
        {
        }
        

        protected override void _onInit()
        {
            if (followTarget != null)
            {
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
                    normalController.refreshWnd(eventInfo);
                    normalController.setTriggerAction(onEventClick);
                    _m_entranceController = normalController;
                }
                
                followTarget.addController(_m_entranceController);
            }
        }
        protected override void _onDiscard()
        {
            _m_entranceController?.discard();
            _m_entranceController = null;
        }
        protected override void _dealTypicalEventProcess(Action _complete)
        {
            List<NPCommon_ItemInfo> rewardList = null;
            long optionId = 0;
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(_dealDone =>
                {
                    QueueMgr.instance.AddNode(new GNodeAnecdoteEventChoice(eventInfo, (_rewardList, _optionId) =>
                    {
                        rewardList = _rewardList;
                        optionId = _optionId;
                    }, _dealDone));
                })
                .addDelegateProcess(_dealDone =>
                {
                    AnecdoteEventChoiceOptionRefObj optionRef = GRefdataCoreMgr.instance.anecdoteEventChoiceOptionRefCore.getRef(optionId);
                    GGUIWndAnecdoteEventChoiceResult.instance.refreshWnd(optionRef, rewardList);
                    QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndAnecdoteEventChoiceResult.instance, _dealDone));
                })
                .addProcess(_complete);
            process.dealProcess();
        }
    }
}