using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟排行信息
    /// </summary>
    public class GuildRankInfo
    {
        //常驻排行榜id
        private long _m_lRankFixedId;
        //排行榜id
        private long _m_lRankId;
        //基础信息 - 联盟id
        private long _m_guildId;
        
        //基础信息 - 排名名次
        private int _m_rankSortOrder;

        //基础信息 - 排行分数
        private long _m_rankScore;

        //基础信息 - 分数来源id
        private long _m_sourceId;

        //是否是跨服排行榜
        private bool _m_bIsCross;

        //联盟信息
        private GuildOtherInfo _m_guildInfo;

        //当前正在请求联盟信息
        private bool _m_isReqingGuildInfo;

        //请求回调对象
        private Action<GuildOtherInfo> _m_dRequestDelegate;

        /// <summary>
        /// 常驻排行榜id
        /// </summary>
        public long rankFixedId => _m_lRankFixedId;
        /// <summary>
        /// 排行榜id
        /// </summary>
        public long rankId => _m_lRankId;
        /// <summary>
        /// 基础信息 - 联盟id
        /// </summary>
        public long guildId => _m_guildId;
        /// <summary>
        /// 基础信息 - 排名名次
        /// </summary>
        public int rankSortOrder => _m_rankSortOrder;
        /// <summary>
        /// 基础信息 - 排行分数
        /// </summary>
        public long rankScore => _m_rankScore;
        /// <summary>
        /// 是否是跨服排行榜
        /// </summary>
        public bool isCross => _m_bIsCross;

        public GuildRankInfo(Common.RankObj.Rank_BaseItem item, long _rankFixedId, bool _isCross)
        {
            if (null == item)
                return;

            _m_lRankFixedId = _rankFixedId;
            _m_bIsCross = _isCross;
            _m_guildId = item.getKey();
            _m_rankScore = item.getScore();
            _m_sourceId = item.getSourceId();
            _m_rankSortOrder = item.getRank();

            NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_lRankFixedId);
            if (rankFixedRef != null)
                _m_lRankId = rankFixedRef.rank_id;

            _m_isReqingGuildInfo = false;
            _m_dRequestDelegate = null;
        }

        public void setRankId(long _rankId)
        {
            _m_lRankId = _rankId;
        }

        public void getGuildInfo(Action<GuildOtherInfo> _action, bool _getLatestInfo, bool _needShowErorCode = true)
        {
            if (_m_guildInfo != null && !_getLatestInfo)//若已经存在联盟数据, 且不需要最新数据，则直接返回
            {
                _action?.Invoke(_m_guildInfo);
                return;                
            }

            _m_dRequestDelegate += _action;
            
            if (_m_isReqingGuildInfo)//若已经正在请求中，则直接返回
                return;

            _m_isReqingGuildInfo = true;
            NPPlayer.instance.guildComp.reqOtherGuildInfo(_m_guildId, info =>
            {
                _m_isReqingGuildInfo = false;
                
                if (info != null)
                    _m_guildInfo = new GuildOtherInfo(info);
                
                _m_dRequestDelegate?.Invoke(_m_guildInfo);
                _m_dRequestDelegate = null;
            }, () =>
            {
                _m_isReqingGuildInfo = false;
                _m_guildInfo = null;

                _m_dRequestDelegate?.Invoke(null);
                _m_dRequestDelegate = null;
            }, _needShowErorCode);
        }

        /// <summary>
        /// 获取联盟排行信息列表
        /// </summary>
        /// <returns></returns>
        public static List<GuildRankInfo> getRankInfoList(List<Common.RankObj.Rank_BaseItem> _serverRankInfoList, long _rankFixedId, bool _isCross)
        {
            if (_serverRankInfoList == null)
                return null;

            List<GuildRankInfo> rankInfoList = new List<GuildRankInfo>();
            foreach (Common.RankObj.Rank_BaseItem serverRankInfo in _serverRankInfoList)
            { 
                if(serverRankInfo != null)   
                    rankInfoList.Add(new GuildRankInfo(serverRankInfo, _rankFixedId, _isCross));
            }

            return rankInfoList;
        }
    }
}