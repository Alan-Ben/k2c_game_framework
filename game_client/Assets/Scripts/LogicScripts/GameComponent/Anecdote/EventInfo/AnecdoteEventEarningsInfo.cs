using System;
using System.Collections.Generic;
using ALPackage;
using Common.AnecdoteObj;
using GC2GS.p021_PlayerInfo;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class AnecdoteEventEarningsInfo : _AAnecdoteEventInfo<AnecdoteEventEarningsRefObj, Anecdote_EventExtraData_Earnings>
    {
        // 是否已领取首次奖励
        private bool _m_hasDrawFirstReward;
        
        
        public AnecdoteEventEarningsInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef) 
            : base(_serverInfo, _eventRef)
        {
        }
        protected override void _constructContentData(Anecdote_EventExtraData_Earnings _data)
        {
            _m_hasDrawFirstReward = _data.getHadDrawFirstReward();
        }
        
        
        public event Action onDrawFirstReward;
        
        /// <summary>
        /// 是否已领取首次奖励
        /// </summary>
        public bool hasDrawFirstReward { get { return _m_hasDrawFirstReward; } }
        /// <summary>
        /// 是否可以领取最终奖励
        /// </summary>
        public bool canGainFinalReward { get { return _m_hasDrawFirstReward && NPPlayer.instance.specialItemComp.goldData.earnings >= typeRef.earnings; } }
        

        public void reqDrawFirstReward(Action<bool, GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward> _complete)
        {
            if (_m_hasDrawFirstReward)
            {
                ALLog.Error("[Anecdote] 已领取首次奖励");
                _complete?.Invoke(false, null);
                return;
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward(instanceId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward>(_complete));
        }
        public void reqGainFinalReward(Action<bool, GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward> _complete)
        {
            if (!_m_hasDrawFirstReward)
            {
                ALLog.Error("[Anecdote] 未领取首次奖励");
                _complete?.Invoke(false, null);
                return;
            }
            if (NPPlayer.instance.specialItemComp.goldData.earnings < typeRef.earnings)
            {
                ALLog.Error("[Anecdote] 赚速不足");
                _complete?.Invoke(false, null);
                return;
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_021_022_ReqDrawAnecdoteEarningsFinalReward(instanceId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward>(_complete));
        }
        
            
        
        protected override void _updateData(Anecdote_EventExtraData_Earnings _data)
        {
            bool serverData = _data.getHadDrawFirstReward();
            if (serverData == _m_hasDrawFirstReward)
                return;
            
            _m_hasDrawFirstReward = serverData;
            onDrawFirstReward?.Invoke();
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