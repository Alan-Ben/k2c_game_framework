using Common.CrossTeamEnum;

namespace GOE
{
    public class ActivityTeamMemberInfo
    {
        private long _m_cid;
        private Common.CrossTeamEnum.ENPCrossTeamMemberPos _m_pos;
        private long _m_joinMs;

        public long cid
        {
            get { return _m_cid; }
        }

        public ENPCrossTeamMemberPos pos
        {
            get { return _m_pos; }
        }

        public long joinMs
        {
            get { return _m_joinMs; }
        }

        public ActivityTeamMemberInfo(Common.CrossTeamObj.CrossTeamMember_Info _teamMemberInfo)
        {
            if(null == _teamMemberInfo)
                return;
            
            _m_cid = _teamMemberInfo.getCid();
            _m_pos = _teamMemberInfo.getPos();
            _m_joinMs = _teamMemberInfo.getJoinMs();
        } 
    }
}