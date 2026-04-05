using Common.GachaObj;

namespace GOE
{
    /// <summary>
    /// 抽卡记录信息
    /// </summary>
    public class GachaRecordInfo
    {
        /// <summary>
        /// 卡池物品id
        /// </summary>
        private long _m_lItemId;
        /// <summary>
        /// 抽卡时间戳 秒
        /// </summary>
        private int _m_iRollTimeSec;
        /// <summary>
        /// 数据id
        /// </summary>
        private long _m_lDbId;
        
        public GachaRecordInfo(Gacha_RecordInfo _serverInfo)
        {
            update(_serverInfo);
        }
        
        public void update(Gacha_RecordInfo _serverInfo)
        {
            if(_serverInfo == null)
                return;
            
            _m_lItemId = _serverInfo.getItemId();
            _m_iRollTimeSec = _serverInfo.getRollTimeSec();
            _m_lDbId = _serverInfo.getDbId();
        }
    }
}