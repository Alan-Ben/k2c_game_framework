package NPUSServer.TeamActivityMgr;

/**
 * 队伍与活动的关系信息
 */
public class TeamActivityInfo
{
    //队伍ID
    private long _m_lTeamId;
    //活动实例ID
    private long _m_lActivityInstanceId;
    //队长CID
    private long _m_lLeaderCid;

    public TeamActivityInfo(long _teamId, long _activityInstanceId, long _leaderCid)
    {
        _m_lTeamId = _teamId;
        _m_lActivityInstanceId = _activityInstanceId;
        _m_lLeaderCid = _leaderCid;
    }

    public long getTeamId() {return _m_lTeamId;}
    public long getActivityInstanceId() {return _m_lActivityInstanceId;}
    public long getLeaderCid() {return _m_lLeaderCid;}
}
