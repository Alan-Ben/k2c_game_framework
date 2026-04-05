using Common.GuildEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public abstract class _AGuildJoinLimitInfo
    {
        [NotNull]protected Common.GuildObj.Guild_JoinLimitInfo _m_joinLimitInfo;//加入联盟的限制条件
        protected GuildJoinLimitRefObj _m_JoinLimitRefObj;//加入联盟的限制条件配表
        
        /// <summary>
        /// 限制类型
        /// </summary>
        public EGuildJoinLimitType type { get { return _m_joinLimitInfo.getType(); } }
        /// <summary>
        /// 数值
        /// </summary>
        public long value { get { return _m_joinLimitInfo.getValue(); } }

        public _AGuildJoinLimitInfo([NotNull] Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo)
        {
            _m_joinLimitInfo = _joinLimitInfo;

            _m_JoinLimitRefObj = GRefdataCoreMgr.instance.getGuildJoinLimitInfo(_m_joinLimitInfo.getType());
        }

        public virtual string desc
        {
            get
            {
                if (_m_JoinLimitRefObj != null)
                    return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.desc, _m_joinLimitInfo.getValue());
                return string.Empty;
            }
        }

        public virtual  string onTryJoinNotConformLimitTip
        {
            get
            {
                if (_m_JoinLimitRefObj != null)
                    return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.on_try_join_not_conform_limit_tip, _m_joinLimitInfo.getValue());
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 是否有限制
        /// </summary>
        /// <returns></returns>
        public bool hasLimit()
        {
            if (_m_JoinLimitRefObj == null)
                return false;

            return _m_joinLimitInfo.getValue() != _m_JoinLimitRefObj.no_limit_value;
        }
        
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <returns></returns>
        public virtual bool meetLimitingCondition(long _value)
        {
            if (!hasLimit())//若没有服务端限制，则直接返回true
                return true;

            return _meetLimitingCondition(_value);
        }
        protected abstract bool _meetLimitingCondition(long _value);

        /// <summary>
        /// 玩家自身是否满足条件
        /// </summary>
        /// <returns></returns>
        public bool selfMeetLimitingCondition()
        {
            if (!hasLimit())//若没有服务端限制，则直接返回true
                return true;

            return _selfMeetLimitingCondition();
        }
        protected abstract bool _selfMeetLimitingCondition();
    }
}