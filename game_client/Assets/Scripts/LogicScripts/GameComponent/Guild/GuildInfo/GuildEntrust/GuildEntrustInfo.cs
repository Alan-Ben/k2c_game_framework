using System;
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟建设信息
    /// </summary>
    public class GuildEntrustInfo
    {
        /// <summary>
        /// 序列号
        /// </summary>
        private long _m_lSerial;
        /// <summary>
        /// 委托品质配表id
        /// </summary>
        private long _m_lEntrustQuailtyRefId;
        /// <summary>
        /// 委托品质配表数据(不要直接引用)
        /// </summary>
        private GuildRandomEntrustQualityRefObj _m_rEntrustQuailtyRefObj;
        /// <summary>
        /// 委托事件id
        /// </summary>
        private long _m_lEntrustEventId;
        /// <summary>
        /// 委托事件配表数据(不要直接引用)
        /// </summary>
        private GuildRandomEntrustRefObj _m_rEntrustEventRefObj;
        /// <summary>
        /// 当前进度
        /// </summary>
        private int _m_iPoint;

        public GuildEntrustInfo(Guild_EntrustInfo _serverEntrustInfo)
        {
            updateInfo(_serverEntrustInfo);
        }

        /// <summary>
        /// 获取委托品质配表数据
        /// </summary>
        public GuildRandomEntrustQualityRefObj entrustQualityRefObj
        {
            get
            {
                if (_m_rEntrustQuailtyRefObj == null || _m_rEntrustQuailtyRefObj.id != _m_lEntrustQuailtyRefId)
                    _m_rEntrustQuailtyRefObj = GRefdataCoreMgr.instance.guildRandomEntrustQualityRefCore.getRef(_m_lEntrustQuailtyRefId);
                
                return _m_rEntrustQuailtyRefObj;
            }
        }
        
        /// <summary>
        /// 获取委托事件配表数据
        /// </summary>
        public GuildRandomEntrustRefObj entrustEventRefObj
        {
            get
            {
                if (_m_rEntrustEventRefObj == null || _m_rEntrustEventRefObj.id != _m_lEntrustEventId)
                    _m_rEntrustEventRefObj = GRefdataCoreMgr.instance.guildRandomEntrustRefCore.getRef(_m_lEntrustEventId);
                
                return _m_rEntrustEventRefObj;
            }
        }
        
        /// <summary>
        /// 当前进度
        /// </summary>
        public int point { get { return _m_iPoint; } }

        public void updateInfo(Guild_EntrustInfo _serverEntrustInfo)
        {
            if(_serverEntrustInfo == null)
                return;

            // 若事件序列号 或 事件id/品质 变化了, 说明变更了新委托
            bool isNewEntrust = _serverEntrustInfo.getSerial() != _m_lSerial || _serverEntrustInfo.getEventId() != _m_lEntrustEventId || _serverEntrustInfo.getRefId() != _m_lEntrustQuailtyRefId;

            _m_lSerial = _serverEntrustInfo.getSerial();
            _m_lEntrustQuailtyRefId = _serverEntrustInfo.getRefId();
            _m_lEntrustEventId = _serverEntrustInfo.getEventId();
            _m_iPoint = _serverEntrustInfo.getPoint();
            
            if(isNewEntrust)
                WinMsg.SendMsg(WinMsgType.ON_GUILD_ENTRUST_CHG_NEW);
            else
                WinMsg.SendMsg(WinMsgType.ON_GUILD_ENTRUST_INFO_CHG);
        }

        /// <summary>
        /// 获取每次处理委托获得的金币
        /// </summary>
        /// <returns></returns>
        public long getPerDealGainCoin()
        {
            // 每次处理可获取的金币（联盟总赚速度 * 每次委托处理的金币收益万分比 / 10000）
            return (long)Math.Ceiling(1d * (NPPlayer.instance.guildComp.guildInfo?.totalEarnings ?? 0) * (entrustQualityRefObj?.gain_currency_per ?? 0) / 10000d);
        }
    }
}