using System;
using System.Collections.Generic;
using Common.AnecdoteObj;
using GC2GS.p021_PlayerInfo;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class AnecdoteEventChoiceInfo : _AAnecdoteEventInfo<AnecdoteEventChoiceRefObj>
    {
        public AnecdoteEventChoiceInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef) 
            : base(_serverInfo, _eventRef)
        {
        }


        /// <summary>
        /// 选择选项，并在完成后返回成功与否
        /// </summary>
        public void reqSelectChoice(long _optionId, Action<bool, GS2GC_021_040_RetDealAnecdoteChoiceEvent> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_021_040_ReqDealAnecdoteChoiceEvent(instanceId, _optionId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_040_RetDealAnecdoteChoiceEvent>(_complete));
        }
        public override bool hasHeroReward()
        {
            List<AnecdoteEventChoiceOptionRefObj> optionRefList = typeRef.option_ref_list;
            if (optionRefList == null || optionRefList.Count == 0)
                return false;

            foreach (AnecdoteEventChoiceOptionRefObj optionRef in optionRefList)
            {
                if (optionRef == null)
                    continue;
                
                List<NPCommonCostItem> rewardItems = optionRef.reward_item_list;
                if (rewardItems == null || rewardItems.Count == 0)
                    continue;

                foreach (NPCommonCostItem item in rewardItems)
                {
                    if (item != null && item.getItemType() == ENPItemType.HERO)
                        return true;
                }
            }

            return false;
        }
    }
}