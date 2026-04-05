using Common.GachaObj;

namespace GOE
{
    /// <summary>
    /// 公屏抽卡记录信息
    /// </summary>
    public class GachaPublicRecordInfo
    {
        /// <summary>
        /// 卡池物品id
        /// </summary>
        private long _m_lItemId;
        /// <summary>
        /// 数据id
        /// </summary>
        private long _m_lDbId;
        /// <summary>
        /// 玩家名字
        /// </summary>
        private string _m_sPlayerName;

        public GachaPublicRecordInfo(Gacha_PublicRecordInfo _serverInfo)
        {
            update(_serverInfo);
        }
        
        public void update(Gacha_PublicRecordInfo _serverInfo)
        {
            if(_serverInfo == null)
                return;
            
            _m_lItemId = _serverInfo.getItemId();
            _m_lDbId = _serverInfo.getDbId();
            _m_sPlayerName = _serverInfo.getPlayerName();
        }
    }
}