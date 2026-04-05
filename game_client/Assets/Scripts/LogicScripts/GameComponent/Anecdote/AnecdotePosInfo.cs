using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class AnecdotePosInfo
    {
        [ItemNotNull, NotNull] private readonly List<_AAnecdoteEventInfo> _m_eventList;
        private readonly long _m_posId;
        private readonly AnecdotePosRefObj _m_posRef;
        
        
        public AnecdotePosInfo(long _posId)
        {
            _m_eventList = new List<_AAnecdoteEventInfo>();
            
            _m_posId = _posId;
            _m_posRef = GRefdataCoreMgr.instance.anecdotePosRefCore.getRef(_m_posId);
        }
        
        
        public event Action onEventListChange;

        /// <summary>
        /// 位置 id
        /// </summary>
        public long posId { get { return _m_posId; } }
        /// <summary>
        /// 位置配置
        /// </summary>
        public AnecdotePosRefObj posRef { get { return _m_posRef; } }
        /// <summary>
        /// 是否不存在事件了
        /// </summary>
        public bool isEmpty { get { return _m_eventList.Count == 0; } }


        /// <summary>
        /// 获取下一个可以展示的事件
        /// </summary>
        [Pure]
        public _AAnecdoteEventInfo getNextEnableEvent()
        {
            foreach (_AAnecdoteEventInfo eventInfo in _m_eventList)
            {
                if (eventInfo.isEnable)
                    return eventInfo;
            }

            return null;    
        }
        
        
        internal void _addEvent(_AAnecdoteEventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;
            
            _m_eventList.Add(_eventInfo);
            _m_eventList.Sort();
            onEventListChange?.Invoke();
        }
        internal void _removeEvent(_AAnecdoteEventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;
            
            _m_eventList.Remove(_eventInfo);
            onEventListChange?.Invoke();
        }
    }
}