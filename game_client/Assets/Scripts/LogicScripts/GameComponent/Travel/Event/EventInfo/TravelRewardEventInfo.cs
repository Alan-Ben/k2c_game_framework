using System.Collections.Generic;
using ALPackage;
using Common.TravelObj;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历奖励事件数据
    /// </summary>
    public class TravelRewardEventInfo : _ATravelEventInfo
    {
        public TravelRewardEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelRewardEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelRewardEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        protected override void _dealEvent()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealRewardTravel(instanceId, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        _showEventResultWnd(_msg.getResult(), oldEarnings, null, _done);
                        // long nowExp = getExp();//获取玩家经验
                        // long nowEarnings = getEarnings();//获取玩家收益
                        // resultInfo = new CommonTravelEventResultInfo(this, _msg.getResult().getItemList(), nowExp - oldExp, nowEarnings - oldEarnings);
                        // GGUIWndTravelCommonResult resultWnd = new GGUIWndTravelCommonResult(resultInfo);
                        // // 显示事件结果
                        // QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_EVENT_COMMON_RESULT, true, false, false, null, resultWnd, true, false,
                        //     null, null, null, () =>
                        //     {
                        //         resultWnd.discard();
                        //         resultWnd = null;
                        //         
                        //         _done?.Invoke();
                        //     }));
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        protected override void _dealEventSimple()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealRewardTravel(instanceId, (_msg) =>
                    {
                        _done?.Invoke();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_rewardEventCenterTipText");
        }
    }
}