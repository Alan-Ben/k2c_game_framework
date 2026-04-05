using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class AnecdoteEventRewardView : _AAnecdoteEventView<AnecdoteEventRewardInfo, GTDMonoAnecdoteEvent>
    {
        private _AALGGUICommonFollowItemController _m_entranceController;
        
        
        public AnecdoteEventRewardView([NotNull] AnecdoteEventRewardInfo _eventInfo) 
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
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            eventInfo.reqDrawReward((_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
                GCommon.dealGainItem(_msg.getItemList(), TransKeyConst.common_getreward_tip, _complete);
            });
        }
    }
}