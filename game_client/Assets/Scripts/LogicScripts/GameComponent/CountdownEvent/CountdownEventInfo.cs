using Common.CountdownEventObj;

namespace GOE
{
    /// <summary>
    /// 倒计时事件信息
    /// </summary>
    public class CountdownEventInfo
    {
        //数据库id
        private long _m_lDbId;
        //倒计时事件id
        private long _m_lCountdownEventId;
        //倒计时事件配置
        private CountdownEventRefObj _m_lCountdownEventRef;
        //倒计时结束时间戳
        private long _m_lFinishTimeMs;
        //重置次数
        private int _m_iResetCount;

        /// <summary>
        /// 数据库id
        /// </summary>
        public long dbId { get { return _m_lDbId; } }
        /// <summary>
        /// 倒计时事件id
        /// </summary>
        public long countdownEventId { get { return _m_lCountdownEventId; } }
        /// <summary>
        /// 倒计时事件配置
        /// </summary>
        public CountdownEventRefObj countdownEventRef { get { return _m_lCountdownEventRef; } }
        /// <summary>
        /// 倒计时结束时间戳
        /// </summary>
        public long finishTimeMs { get { return _m_lFinishTimeMs; } }
        /// <summary>
        /// 重置次数
        /// </summary>
        public int resetCount { get { return _m_iResetCount; } }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isValid { get { return _m_lDbId > 0; } }

        //构造函数
        public CountdownEventInfo(CountdownEvent_Info _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新事件信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(CountdownEvent_Info _info)
        {
            if (_info == null)
                return;

            _m_lDbId = _info.getDbId();
            _m_lCountdownEventId = _info.getEventId();
            _m_lCountdownEventRef = GRefdataCoreMgr.instance.countdownEventRefCore.getRef(_m_lCountdownEventId);
            if (_m_lCountdownEventRef != null)
                _m_lFinishTimeMs = _info.getTriggerTimeMs() + _m_lCountdownEventRef.duration * 1000;
            _m_iResetCount = _info.getResetCount();
        }
    }
}
