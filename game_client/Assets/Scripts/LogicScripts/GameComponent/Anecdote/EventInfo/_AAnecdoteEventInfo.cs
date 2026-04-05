using System;
using ALBasicProtocolPack;
using ALPackage;
using Common.AnecdoteObj;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _AAnecdoteEventInfo<T_REF, T_DATA> : _AAnecdoteEventInfo<T_REF>
        where T_REF : _IALBasicRefObj
        where T_DATA : _IALProtocolStructure, new()
    {
        protected _AAnecdoteEventInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef) 
            : base(_serverInfo, _eventRef)
        {
            T_DATA contentData = new();
            contentData.readPackage(new ALProtocolBuf(_serverInfo.getExtraData()));
            _constructContentData(contentData);
        }
        

        protected abstract void _constructContentData([NotNull] T_DATA _data);
        protected abstract void _updateData([NotNull] T_DATA _data);
        
        
        internal sealed override void _updateData(byte[] _extraData)
        {
            T_DATA contentData = new();
            contentData.readPackage(new ALProtocolBuf(_extraData));
            _updateData(contentData);
        }
    }
    public abstract class _AAnecdoteEventInfo<T_REF> : _AAnecdoteEventInfo
        where T_REF : _IALBasicRefObj
    {
        // 具体事件类型的特有配置
        [NotNull] private readonly T_REF _m_typeRef;


        protected _AAnecdoteEventInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef) 
            : base(_serverInfo, _eventRef)
        {
            _m_typeRef = (T_REF) _eventRef.event_type_ref;
        }
        
        
        /// <summary>
        /// 具体事件类型的特有配置
        /// </summary>
        [NotNull] public T_REF typeRef { get { return _m_typeRef; } }


        internal override void _updateData(byte[] _extraData)
        {
            // 不需要额外数据的类
        }
    }
    /// <summary>
    /// 经营事件数据的基类
    /// </summary>
    public abstract class _AAnecdoteEventInfo : IComparable<_AAnecdoteEventInfo>
    {
        public static _AAnecdoteEventInfo createInstance(Anecdote_EventInfo _serverInfo)
        {
            if (_serverInfo == null)
                return null;

            long eventId = _serverInfo.getEventId();
            AnecdoteEventRefObj eventRef = GRefdataCoreMgr.instance.anecdoteEventRefCore.getRef(eventId);
            if (eventRef == null)
                return null;

            return eventRef.event_type_ref switch
            {
                AnecdoteEventRewardRefObj rewardEvent => new AnecdoteEventRewardInfo(_serverInfo, eventRef),
                AnecdoteEventEarningsRefObj earningsEvent => new AnecdoteEventEarningsInfo(_serverInfo, eventRef),
                AnecdoteEventChoiceRefObj choiceEvent => new AnecdoteEventChoiceInfo(_serverInfo, eventRef),
                _ => null,
            };
        }


        // 事件实例 ID
        private readonly long _m_instanceId;
        // 事件位置 ID
        private readonly long _m_posId;
        // 事件的配置数据
        [NotNull] private readonly AnecdoteEventRefObj _m_eventRef;


        protected _AAnecdoteEventInfo([NotNull] Anecdote_EventInfo _serverInfo, [NotNull] AnecdoteEventRefObj _eventRef)
        {
            _m_instanceId = _serverInfo.getInstanceId();
            _m_posId = _serverInfo.getPosId();
            _m_eventRef = _eventRef;
        }
        
        
        /// <summary>
        /// 事件实例 ID
        /// </summary>
        public long instanceId { get { return _m_instanceId; } }
        /// <summary>
        /// 事件位置 ID
        /// </summary>
        public long posId { get { return _m_posId; } }
        /// <summary>
        /// 事件的配置数据
        /// </summary>
        [NotNull] public AnecdoteEventRefObj eventRef { get { return _m_eventRef; } }
        /// <summary>
        /// 事件是否显示
        /// </summary>
        public bool isEnable { get { return eventRef.show_condition?.IsEnable(null) ?? true; } }


        internal abstract void _updateData(byte[] _extraData);


        // 是否有大臣奖励
        public abstract bool hasHeroReward();
        
        public int CompareTo(_AAnecdoteEventInfo _other)
        {
            if (_other == null)
                return 1;

            // 优先级越高的排在前面
            int priorityCmp = _other.eventRef.show_priority.CompareTo(eventRef.show_priority);
            return priorityCmp;
        }
    }
}