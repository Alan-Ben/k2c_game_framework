using Common.CrossTeamEnum;
using Common.CrossTeamObj;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 活动队伍信息
    /// </summary>
    public class ActivityTeamInfo
    {
        // 队伍ID
        private long _m_teamId;
        // 队伍成员上限
        private int _m_memberLimit;
        // 队伍名称
        private string _m_teamName;
        // 队伍宣言
        private string _m_teamDesc;
        // 加入方式
        private ENPCrossTeamJoinType _m_eJoinType;
        // 申请条件
        private CrossTeam_SetInfo_Join _m_applyCond;
        // 成员列表
        private List<ActivityTeamMemberInfo> _m_memberInfos;

        /// <summary>
        /// 队伍ID
        /// </summary>
        public long teamId { get { return _m_teamId; } }
        /// <summary>
        /// 队伍成员上限
        /// </summary>
        public int memberLimit { get { return _m_memberLimit; } }
        /// <summary>
        /// 队伍名称
        /// </summary>
        public string teamName { get { return _m_teamName; } }
        /// <summary>
        /// 队伍宣言
        /// </summary>
        public string teamDesc { get { return _m_teamDesc; } }
        /// <summary>
        /// 加入方式
        /// </summary>
        public ENPCrossTeamJoinType joinType { get { return _m_eJoinType; } }
        /// <summary>
        /// 申请条件
        /// </summary>
        public CrossTeam_SetInfo_Join applyCond { get { return _m_applyCond; } }
        /// <summary>
        /// 成员列表
        /// </summary>
        public List<ActivityTeamMemberInfo> memberInfos { get { return _m_memberInfos; } }


        public ActivityTeamInfo(Common.CrossTeamObj.CrossTeam_Info _teamInfo)
        {
            if(null == _teamInfo)
                return;
            
            _m_teamId = _teamInfo.getTeamBase().getTeamId();
            _m_memberLimit = _teamInfo.getTeamBase().getMemberLimit();
            _m_teamName = _teamInfo.getTeamBase().getTeamName();
            _m_teamDesc = _teamInfo.getTeamBase().getTeamDec();
            _m_eJoinType = _teamInfo.getJoinType();
            _m_applyCond = _teamInfo.getJoinCond();

            _m_memberInfos = new List<ActivityTeamMemberInfo>();
            foreach (CrossTeamMember_Info crossTeamMemberInfo in _teamInfo.getMemberList())
            {
                _m_memberInfos.Add(new ActivityTeamMemberInfo(crossTeamMemberInfo));
            }
        }

        public void addMember(Common.CrossTeamObj.CrossTeamMember_Info _teamMemberInfo)
        {
            if (_teamMemberInfo == null)
                return;

            _m_memberInfos.Add(new ActivityTeamMemberInfo(_teamMemberInfo));
        }

        public void removeMember(long _cid)
        {
            if(null == _m_memberInfos)
                return;
            
            ActivityTeamMemberInfo memberInfo = null;
            for (int i = 0; i < _m_memberInfos.Count; i++)
            {
                memberInfo = _m_memberInfos[i];
                if (null != memberInfo && memberInfo.cid == _cid)
                {
                    _m_memberInfos.RemoveAt(i);
                    break;
                }
            }
        }
    }
}