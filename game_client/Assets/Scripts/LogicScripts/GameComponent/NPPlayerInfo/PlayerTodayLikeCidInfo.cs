using System;
using System.Collections.Generic;
using Common;

namespace GOE
{
    public class PlayerTodayLikeCidInfo
    {
        
        /// <summary>
        /// 上次刷新日期
        /// </summary>
        private int  _m_lastFreshDate;
        /// <summary>
        /// 点赞过玩家CID列表
        /// </summary>
        private List<long>  _m_likeCidList;

        public PlayerTodayLikeCidInfo(Common_TodayLikeCidInfo _todayLikeCidInfo)
        {
            _m_lastFreshDate = _todayLikeCidInfo.getLastFreshDate();
            _m_likeCidList = _todayLikeCidInfo.getLikeCidList();
        }

        /// <summary>
        /// 上次刷新日期
        /// </summary>
        public int lastFreshDate { get => _m_lastFreshDate; }
        /// <summary>
        /// 点赞过玩家CID列表
        /// </summary>
        public List<long> likeCidList { get => _m_likeCidList; }

        public void updateInfo(Common_TodayLikeCidInfo _todayLikeCidInfo)
        {
            _m_lastFreshDate = _todayLikeCidInfo.getLastFreshDate();
            _m_likeCidList = _todayLikeCidInfo.getLikeCidList();
        }

        /// <summary>
        /// 是否已经点赞过
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public bool getHasLike(long _cid)
        {
            //判断日期,日期已经过了，都算没点过赞
            if(_m_lastFreshDate < TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag))
                return false;
            
            return _m_likeCidList.Contains(_cid);
        }
    }
}