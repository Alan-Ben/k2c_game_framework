using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsExploreBossEventInfo : _AMarsExploreEventInfo
    {
        [NotNull] private readonly MarsExploreEventBossRefObj _m_typeRefObj;
        
        
        public MarsExploreBossEventInfo([NotNull] Mars_ExploreEvent _serverData) 
            : base(_serverData)
        {
            _m_typeRefObj = GRefdataCoreMgr.instance.marsExploreEventBossRefCore.getRef(_serverData.getEventId());
            if (_m_typeRefObj == null)
                ALLog.Error($"[MarsExploreBossEventInfo] MarsExploreEventPveRefObj is null, eventId: {_serverData.getEventId()}");
        }


        [NotNull] public MarsExploreEventBossRefObj typeRefObj { get { return _m_typeRefObj; } }
        public override EMarsExploreEventType eventType { get { return EMarsExploreEventType.BOSS; } }


        public override List<NPCommonCostItem> getRewardItems()
        {
            return _m_typeRefObj.reward_item_list;
        }
        public override long getPower()
        {
            return _m_typeRefObj.solider_num * _m_typeRefObj.solider_power;
        }


        internal override bool _isValid()
        {
            return base._isValid() && _m_typeRefObj != null;
        }
    }
}