using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsExploreBattleEventInfo : _AMarsExploreEventInfo
    {
        [NotNull] private readonly MarsExploreEventBattleRefObj _m_typeRefObj;
        
        
        public MarsExploreBattleEventInfo([NotNull] Mars_ExploreEvent _serverData) 
            : base(_serverData)
        {
            _m_typeRefObj = GRefdataCoreMgr.instance.marsExploreEventBattleRefCore.getRef(_serverData.getEventId());
            if (_m_typeRefObj == null)
                ALLog.Error("[MarsExploreBattleEventInfo] MarsExploreEventBattleRefObj is null, eventId: {_serverData.getEventId()}");
        }
        
        
        [NotNull] public MarsExploreEventBattleRefObj typeRefObj { get { return _m_typeRefObj; } }
        public override EMarsExploreEventType eventType { get { return EMarsExploreEventType.BATTLE; } }


        public override List<NPCommonCostItem> getRewardItems()
        {
            return MarsUtil.getMarsExploreEventRewards(exploreLvl, _m_typeRefObj.quality);
        }
        public override long getPower()
        {
            return MarsUtil.getMarsExploreEventSoliderNum(exploreLvl, _m_typeRefObj.quality) * MarsUtil.getMarsExploreEventSoliderPower(exploreLvl, _m_typeRefObj.quality);
        }


        internal override bool _isValid()
        {
            return base._isValid() && _m_typeRefObj != null;
        }
    }
}