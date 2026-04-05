using System.Collections.Generic;
using Common.GuildObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟成员的每日更新数据
    /// </summary>
    public class GuildMemberDailyData
    {
        /// <summary>
        /// 日期 用于每天重置数据
        /// </summary>
        private int _m_iDate;
        
        /// <summary>
        /// 当天已领取建设奖励列表
        /// </summary>
        private List<int> _m_lTodayDrawConstructRewardList;

        public GuildMemberDailyData(Guild_MemberDailyData _serverDailyData)
        {
            updateInfo(_serverDailyData);
        }

        /// <summary>
        /// 日期tag
        /// </summary>
        public int date { get { return _m_iDate; } }
        
        /// <summary>
        /// 重置数据
        /// </summary>
        public void resetData()
        {
            _m_iDate = (int)NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE);
            updateDrawConstructRewardList(null);
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_DAILY_DATA_CHG);
        }
        
        public void updateInfo(Guild_MemberDailyData _serverDailyData)
        {
            if(_serverDailyData == null)
                return;
            
            _m_iDate = _serverDailyData.getDate();
            updateDrawConstructRewardList(_serverDailyData.getTodayDrawConstructRewardList());
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_DAILY_DATA_CHG);
        }

        #region 建设奖励

        public void updateDrawConstructRewardList(List<int> _list)
        {
            _m_lTodayDrawConstructRewardList = _list;
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_DAILY_DRAW_CONSTRUCT_REWARD_LIST_CHG);
        }
        
        /// <summary>
        /// 是否已领取建设奖励
        /// </summary>
        /// <param name="_constructId"></param>
        /// <returns></returns>
        public bool hasDrawConstructReward(int _constructId)
        {
            return _m_lTodayDrawConstructRewardList?.Contains(_constructId) ?? false;
        }

        #endregion
    }
}