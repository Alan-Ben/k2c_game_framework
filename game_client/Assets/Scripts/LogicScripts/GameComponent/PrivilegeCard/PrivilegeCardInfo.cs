using Common.PrivilegeCardEnum;
using Common.PrivilegeCardObj;

namespace GOE
{
    /// <summary>
    /// 权益卡信息数据
    /// </summary>
    public class PrivilegeCardInfo
    {
        //权益卡类型
        private EPrivilegeCardType _m_eCardType;
        //生效开始时间戳（秒）
        private long _m_lStartS;
        //生效结束时间戳（秒）
        private long _m_lEndS;
        //最后一次领取每日奖励的时间戳（秒）
        private long _m_lLastGainDailyRewardS;
        //配置数据
        private PrivilegeCardRefObj _m_refObj;

        /// <summary>
        /// 权益卡类型
        /// </summary>
        public EPrivilegeCardType cardType { get { return _m_eCardType; } }
        /// <summary>
        /// 生效开始时间戳（秒）
        /// </summary>
        public long startTimeS { get { return _m_lStartS; } }
        /// <summary>
        /// 生效结束时间戳（秒）
        /// </summary>
        public long endTimeS { get { return _m_lEndS; } }
        /// <summary>
        /// 配置数据
        /// </summary>
        public PrivilegeCardRefObj refObj { get { return _m_refObj; } }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool isActivate { get { return _m_lEndS > 0 && FpsAndPingMgr.instance.serverTimeTagS <= _m_lEndS; } }
        /// <summary>
        /// 今日是否可领取奖励
        /// </summary>
        public bool canGetRewardToday { get { return isActivate && (_m_lLastGainDailyRewardS == 0 || !TimeUtil.serverTimeMsIsInSameDay(_m_lLastGainDailyRewardS * 1000, FpsAndPingMgr.instance.serverTimeTag)); } }

        public PrivilegeCardInfo(PrivilegeCardObj_Info _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        public void updateInfo(PrivilegeCardObj_Info _info)
        {
            if (_info == null)
                return;

            _m_eCardType = _info.getCardType();
            _m_lStartS = _info.getStartS();
            _m_lEndS = _info.getEndS();
            _m_lLastGainDailyRewardS = _info.getLastGainDailyRewardS();
            _m_refObj = GRefdataCoreMgr.instance.privilegeCardRefCore.getRef((long) _m_eCardType);
        }
    }
}