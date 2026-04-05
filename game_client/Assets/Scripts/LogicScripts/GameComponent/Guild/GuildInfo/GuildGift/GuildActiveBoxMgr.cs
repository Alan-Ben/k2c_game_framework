
using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟活跃宝箱管理器
    /// </summary>
    public class GuildActiveBoxMgr
    {
        [NotNull] private List<GuildActiveBoxInfo> _m_ActivityBoxList = new List<GuildActiveBoxInfo>();

        private GuildActiveBoxInfo _m_iEarliestExpiredBoxInfo;//最早过期的宝箱信息

        private ALCommonEnableTaskController _m_task;

        public GuildActiveBoxMgr()
        {
        }

        /// <summary>
        /// 活跃宝箱列表
        /// </summary>
        [NotNull] public List<GuildActiveBoxInfo> activeBoxInfoList { get { return _m_ActivityBoxList; } }

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
            _m_iEarliestExpiredBoxInfo = null;
            _m_ActivityBoxList.Clear();
        }
        
        /// <summary>
        /// 更新最早过期的宝箱信息
        /// </summary>
        private void _refreshEarliestExpiredBoxInfo()
        {
            // 若记录的最早过期宝箱信息不为空, 且已经失效, 则置空
            if (_m_iEarliestExpiredBoxInfo != null && !_m_iEarliestExpiredBoxInfo.isValid)
                _m_iEarliestExpiredBoxInfo = null;
            
            GuildActiveBoxInfo boxInfo = null;
            //倒序遍历_m_iEarliestExpiredBoxInfo
            for (int i = _m_ActivityBoxList.Count - 1; i >= 0; i--)
            {
                boxInfo = _m_ActivityBoxList[i];
                if (boxInfo == null)
                {
                    _m_ActivityBoxList.RemoveAt(i);
                    continue;
                }

                // 若宝箱已过期, 移除
                if (!boxInfo.isValid)
                {
                    _m_ActivityBoxList.RemoveAt(i);
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_ACTIVE_BOX_EXPIRED, boxInfo);
                    continue;
                }

                // 若_m_iEarliestExpiredBoxInfo为空 或者 当前宝箱的过期时间比_m_iEarliestExpiredBoxInfo的过期时间更早, 则更新_m_iEarliestExpiredBoxInfo
                if (_m_iEarliestExpiredBoxInfo == null ||
                    _m_iEarliestExpiredBoxInfo.expiredTimeMs > boxInfo.expiredTimeMs)
                {
                    _m_iEarliestExpiredBoxInfo = boxInfo;
                }
            }
        }

        /// <summary>
        /// 添加活跃宝箱信息
        /// </summary>
        /// <param name="_serverActiveBoxInfo"></param>
        public void addActiveBox(Guild_ActiveBoxInfo _serverActiveBoxInfo)
        {
            if(_serverActiveBoxInfo == null)
                return;

            // 若宝箱已过期
            if ((_serverActiveBoxInfo.getSendTimeMs() +
                 GRefdataCoreMgr.instance.npGeneral.active_box_valid_time_sec * 1000) <= FpsAndPingMgr.instance.serverTimeTag)
            {
                return;
            }
            
            GuildActiveBoxInfo boxInfo = new GuildActiveBoxInfo(_serverActiveBoxInfo);
            _m_ActivityBoxList.Add(boxInfo);
            
            // 刷新最早过期的宝箱信息
            _refreshEarliestExpiredBoxInfo();
        }
        
        public void addActiveBoxList(List<Guild_ActiveBoxInfo> _serverActiveBoxInfoList)
        {
            if(_serverActiveBoxInfoList == null)
                return;

            foreach (var serverActiveBoxInfo in _serverActiveBoxInfoList)
            {
                // 若宝箱已过期
                if ((serverActiveBoxInfo.getSendTimeMs() +
                     GRefdataCoreMgr.instance.npGeneral.active_box_valid_time_sec * 1000) <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    continue;
                }
            
                GuildActiveBoxInfo boxInfo = new GuildActiveBoxInfo(serverActiveBoxInfo);
                _m_ActivityBoxList.Add(boxInfo);
            }
            
            // 刷新最早过期的宝箱信息
            _refreshEarliestExpiredBoxInfo();
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
            if(_m_iEarliestExpiredBoxInfo == null)
                return;

            // 若已过期
            if (!_m_iEarliestExpiredBoxInfo.isValid)
            {
                // 刷新最早过期的宝箱信息
                _refreshEarliestExpiredBoxInfo();
            }
        }
        
        #endregion
    }
}