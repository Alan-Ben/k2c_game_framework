using System.Collections.Generic;
using Common.GuildEnum;
using NPEnum;

namespace GOE
{
    public class GuildConstructList
    {
        /// <summary>
        /// 日期
        /// </summary>
        private int _m_iDate;
        /// <summary>
        /// 捐赠类型信息列表
        /// </summary>
        private List<Common.GuildObj.Guild_ConstructInfo> _m_lConstructList;
        /// <summary>
        /// 捐赠进度
        /// </summary>
        private int _m_iRewardPoint;
        
        public GuildConstructList(Common.GuildObj.Guild_ConstructList _serverConstructList)
        {
            updateInfo(_serverConstructList);
        }
        
        public int date { get { return _m_iDate; } }
        public List<Common.GuildObj.Guild_ConstructInfo> constructList { get { return _m_lConstructList; } }
        public int rewardPoint { get { return _m_iRewardPoint; } }

        public void resetData()
        {
            _m_iDate = (int)NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE);

            updateConstructList(null);
            updateRewardPoint(0);
        }
        
        public void updateInfo(Common.GuildObj.Guild_ConstructList _serverConstructList)
        {
            if(_serverConstructList == null)
                return;
            
            _m_iDate = _serverConstructList.getDate();
            updateConstructList(_serverConstructList.getConstructList());
            updateRewardPoint(_serverConstructList.getRewardPoint());
        }

        #region 建设类型信息列表

        public void updateConstructList(List<Common.GuildObj.Guild_ConstructInfo> _list)
        {
            _m_lConstructList = _list;
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_CONSTRUCT_INFO_CHG);
        }

        /// <summary>
        /// 根据类型获取今日建设数据
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public Common.GuildObj.Guild_ConstructInfo getTodayConstructInfo(EGuildConstructType _type)
        {
            if (_m_lConstructList == null)
                return null;

            foreach (var constructInfo in _m_lConstructList)
            {
                if (constructInfo != null && constructInfo.getType() == _type)
                    return constructInfo;
            }
            
            return null;
        }

        /// <summary>
        /// 获取今日联盟建设经验和财富
        /// </summary>
        public void getTodayConstructExpAndWealth(out long _exp, out long _wealth)
        {
            _exp = 0;
            _wealth = 0;

            if (_m_lConstructList == null)
                return;

            foreach (var constructInfo in _m_lConstructList)
            {
                if (constructInfo != null)
                {
                    _exp += constructInfo.getGainGuildExpCount();
                    _wealth += constructInfo.getGainGuildWealthCount();
                }
            }
        }
        
        #endregion
        
        /// <summary>
        /// 建设进度奖励进度变更
        /// </summary>
        /// <param name="_point"></param>
        public void updateRewardPoint(int _point)
        {
            if(_m_iRewardPoint == _point)
                return;
            
            _m_iRewardPoint = _point;
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_CONSTRUCT_PROGRESS_REWARD_POINT_CHG);
        }
    }
}