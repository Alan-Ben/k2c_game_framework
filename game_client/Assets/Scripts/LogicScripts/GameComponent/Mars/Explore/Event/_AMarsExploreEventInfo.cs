using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _AMarsExploreEventInfo : _IMarsExplorePosItem
    {
        private readonly long _m_instanceId;
        [NotNull] private readonly MarsExploreEventRefObj _m_refObj;
        private readonly MarsExplorePosRefObj _m_posRefObj;
        private readonly long _m_posId;
        private readonly int _m_exploreLvl;
        private readonly long _m_startTimeMs;
        private bool _m_isDone;


        protected _AMarsExploreEventInfo([NotNull] Mars_ExploreEvent _serverData)
        {
            _m_instanceId = _serverData.getId();
            _m_posId = _serverData.getPos();
            _m_exploreLvl = _serverData.getExploreLvl();
            _m_startTimeMs = _serverData.getCreatedMs();
            _m_refObj = GRefdataCoreMgr.instance.marsExploreEventRefCore.getRef(_serverData.getEventId());
            if (_m_refObj == null)
                ALLog.Error("[_AMarsExploreEventInfo] MarsExploreEventRefObj is null, eventId: {_serverData.getEventId()}");
            _m_posRefObj = GRefdataCoreMgr.instance.marsExplorePosRefCore.getRef(_m_posId);
            _m_isDone = _serverData.getIsDone();
        }


        public event Action onTeamIdChg;
        public event Action onIsDoneChg;        
        public long instanceId { get { return _m_instanceId; } }
        public long refId { get { return _m_refObj.event_id; } }
        public long posId { get { return _m_posId; } }
        public int exploreLvl { get { return _m_exploreLvl; } }
        [NotNull] public MarsExploreEventRefObj refObj { get { return _m_refObj; } }
        public MarsExplorePosRefObj posRefObj { get { return _m_posRefObj; } }
        public abstract EMarsExploreEventType eventType { get; }
        public bool isDone { get { return _m_isDone; } }
        public long startTime { get { return _m_startTimeMs; } }


        public abstract List<NPCommonCostItem> getRewardItems();
        public abstract long getPower();


        internal virtual bool _isValid()
        {
            // 这里是有可能是 null 的，internal 方法，给 component 用来过滤掉那些有问题的数据，外部使用的时候就不会是 null 了。
            return _m_refObj != null;
        }
        internal void _updateIsDone()
        {
            _m_isDone = true;
            onIsDoneChg?.Invoke();
        }
    }
}