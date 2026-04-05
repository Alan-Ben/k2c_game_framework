
using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟大礼管理器
    /// </summary>
    public class GuildGreatRewardMgr
    {
        [NotNull] private List<GuildGreatRewardInfo> _m_GreatRewardList = new List<GuildGreatRewardInfo>();

        private GuildGreatRewardInfo _m_iEarliestExpiredGreatRewardInfo;//最早过期的大礼信息

        private ALCommonEnableTaskController _m_task;

        public GuildGreatRewardMgr()
        {
        }
        
        [NotNull] public List<GuildGreatRewardInfo> greatRewardInfoList { get { return _m_GreatRewardList; } }
        
        /// <summary>
        /// 可领取大礼数量
        /// </summary>
        public int canDrawGreatRewardCount { get { return _m_GreatRewardList.Count; } }
        
        public void init()
        {
            _initTask();
        }
        
        public void discard()
        {
            reset();
            
            _discardTask();
        }
        
        public void reset()
        {
            _m_iEarliestExpiredGreatRewardInfo = null;
            _m_GreatRewardList.Clear();
        }
        
        /// <summary>
        /// 更新最早过期的大礼信息
        /// </summary>
        private void _refreshEarliestExpiredGreatRewardInfo()
        {
            // 若记录的最早过期大礼信息不为空, 且已经失效, 则置空
            if (_m_iEarliestExpiredGreatRewardInfo != null && !_m_iEarliestExpiredGreatRewardInfo.isValid)
                _m_iEarliestExpiredGreatRewardInfo = null;
            
            GuildGreatRewardInfo greatRewardInfo = null;
            //倒序遍历_m_iEarliestExpiredGreatRewardInfo
            for (int i = _m_GreatRewardList.Count - 1; i >= 0; i--)
            {
                greatRewardInfo = _m_GreatRewardList[i];
                if (greatRewardInfo == null)
                {
                    _m_GreatRewardList.RemoveAt(i);
                    continue;
                }

                // 若大礼已过期, 移除
                if (!greatRewardInfo.isValid)
                {
                    _m_GreatRewardList.RemoveAt(i);
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_GREAT_REWARD_EXPIRED, greatRewardInfo);
                    continue;
                }

                // 若_m_iEarliestExpiredGreatRewardInfo为空 或者 当前大礼的过期时间比_m_iEarliestExpiredGreatRewardInfo的过期时间更早, 则更新_m_iEarliestExpiredGreatRewardInfo
                if (_m_iEarliestExpiredGreatRewardInfo == null ||
                    _m_iEarliestExpiredGreatRewardInfo.expiredTimeMs > greatRewardInfo.expiredTimeMs)
                {
                    _m_iEarliestExpiredGreatRewardInfo = greatRewardInfo;
                }
            }
        }

        /// <summary>
        /// 添加联盟大礼信息
        /// </summary>
        /// <param name="_serverGreatRewardInfo"></param>
        public void addGreatReward(Guild_GreatRewardInfo _serverGreatRewardInfo)
        {
            if(_serverGreatRewardInfo == null)
                return;

            // 若大礼已过期
            if ((_serverGreatRewardInfo.getSendTimeMs() +
                 GRefdataCoreMgr.instance.npGeneral.guild_great_reward_valid_time_sec * 1000) <= FpsAndPingMgr.instance.serverTimeTag)
            {
                return;
            }
            
            GuildGreatRewardInfo greatRewardInfoInfo = new GuildGreatRewardInfo(_serverGreatRewardInfo);
            _m_GreatRewardList.Add(greatRewardInfoInfo);
            
            // 刷新最早过期的大礼信息
            _refreshEarliestExpiredGreatRewardInfo();
        }
        
        public void addGreatRewardList(List<Guild_GreatRewardInfo> _serverGreatRewardInfoList)
        {
            if(_serverGreatRewardInfoList == null)
                return;

            foreach (var serverGreatRewardInfo in _serverGreatRewardInfoList)
            {
                // 若大礼已过期
                if ((serverGreatRewardInfo.getSendTimeMs() +
                     GRefdataCoreMgr.instance.npGeneral.guild_great_reward_valid_time_sec * 1000) <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    continue;
                }
            
                GuildGreatRewardInfo greatRewardInfoInfo = new GuildGreatRewardInfo(serverGreatRewardInfo);
                _m_GreatRewardList.Add(greatRewardInfoInfo);
            }
            
            // 刷新最早过期的大礼信息
            _refreshEarliestExpiredGreatRewardInfo();
        }
        
        #region 任务

        private void _discardTask()
        {
            _m_task.setDisable();
        }
        
        private void _initTask()
        {
            _discardTask();
            _m_task = CommonTaskController.CommonEnableDurationActionAddMonoTask(_dealTask, 0.1f);//
        }

        private void _dealTask()
        {
            if(_m_iEarliestExpiredGreatRewardInfo == null)
                return;

            // 若已过期
            if (!_m_iEarliestExpiredGreatRewardInfo.isValid)
            {
                // 刷新最早过期的大礼信息
                _refreshEarliestExpiredGreatRewardInfo();
            }
        }
        
        #endregion
    }
}