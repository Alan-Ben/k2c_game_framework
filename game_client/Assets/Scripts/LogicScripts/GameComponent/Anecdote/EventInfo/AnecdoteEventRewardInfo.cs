using System;
using System.Collections.Generic;
using Common.AnecdoteObj;
using GC2GS.p021_PlayerInfo;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class AnecdoteEventRewardInfo : _AAnecdoteEventInfo<AnecdoteEventRewardRefObj>
    {
        public AnecdoteEventRewardInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef) 
            : base(_serverInfo, _eventRef)
        {
        }
        
        
        /// <summary>
        /// 领取奖励
        /// </summary>
        public void reqDrawReward(Action<bool, GS2GC_021_020_RetDealAnecdoteRewardEvent> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_021_020_ReqDealAnecdoteRewardEvent(instanceId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_020_RetDealAnecdoteRewardEvent>(_complete));
        }
        public override bool hasHeroReward()
        {
            List<NPCommonCostItem> rewardItems = typeRef.reward_item;
            if (rewardItems == null || rewardItems.Count == 0)
                return false;

            foreach (NPCommonCostItem item in rewardItems)
            {
                if (item != null && item.getItemType() == ENPItemType.HERO)
                    return true;
            }

            return false;
        }
    }
}