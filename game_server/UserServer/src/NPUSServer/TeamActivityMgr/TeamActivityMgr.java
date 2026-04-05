package NPUSServer.TeamActivityMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import AllRpcData.CrossTeam_Service.Team.CTSGetUsAllTeam;
import Common.ServerObj.ServerObj_ActivityTeamGroup;
import Common.ServerObj.ServerObj_ActivityTeamGroupList;
import NPCommon.Log.CommLog;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import RPC._ARpcCallBack;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 队伍与活动的关系映射管理
 * 只记录本服创建的队伍与活动的关系，跨服队伍不记录
 */
public class TeamActivityMgr
{
    private NPUserServer _m_userServer;

    //key:队伍ID，value:队伍活动数据
    private HashMap<Long, TeamActivityInfo> _m_hmTeamActivityMap;
    //key:活动实例ID，value:队伍活动数据列表
    private HashMap<Long, ArrayList<TeamActivityInfo>> _m_hmActivityTeamListMap;

    private MutexAtom _m_mutex;

    public TeamActivityMgr(NPUserServer _server)
    {
        _m_userServer = _server;

        _m_hmTeamActivityMap = new HashMap<>();
        _m_hmActivityTeamListMap = new HashMap<>();

        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUserServer() {return _m_userServer;}

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 从CTS加载队伍与活动的映射关系
     */
    public void loadTeamActivityFromCTS()
    {
        List<_AActivityBase> _activityList = _m_userServer.getCommActivityMgr().getAllActivity();
        if(null == _activityList || _activityList.isEmpty())
            return;

        CTSGetUsAllTeam rpc = new CTSGetUsAllTeam();
        rpc.req().setUsId(_m_userServer.getServerTypeId());
        //依次遍历所有组队活动，获取每个活动对应的组ID列表
        for(int i = 0; i < _activityList.size(); i++)
        {
            _AActivityBase activity = _activityList.get(i);
            if(null == activity)
                continue;

            ActivityTeamDealer dealer = activity.getTeamDealer();
            if(null == dealer)
                continue;

            rpc.req().getGroupIdList().add(dealer.getSchedule().getUsGroupId());
        }
        //向CTS请求所有队伍与活动的映射关系，失败重试知道成功为止
        _m_userServer.rpc2crossTeam().requestRepeat(rpc, new _ARpcCallBack<CTSGetUsAllTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSGetUsAllTeam _rpc)
            {
                if(_errCode > 0)
                {
                    CommLog.error("TeamActivityMgr loadTeamActivityFromCTS fail, us:{} errCode:{}", _m_userServer.getServerTypeId(), _errCode);
                    return;
                }

                for(int i = 0; i < _rpc.retObj().getGroupTeamList().size(); i++)
                {
                    ServerObj_ActivityTeamGroupList groupObj = _rpc.retObj().getGroupTeamList().get(i);
                    if(null == groupObj)
                        continue;

                    _AActivityBase activity = _m_userServer.getCommActivityMgr().lookupActivityByGroupId(groupObj.getGroupId());
                    if(null == activity)
                        continue;

                    for(int j = 0; j < groupObj.getTeamList().size(); j++)
                    {
                        ServerObj_ActivityTeamGroup teamObj = groupObj.getTeamList().get(j);
                        if(null == teamObj)
                            continue;

                        TeamActivityInfo info = new TeamActivityInfo(teamObj.getTeamId(), activity.getInstanceId(), teamObj.getLeaderCid());
                        addTeamActivityMap(info);
                    }
                }

                CommLog.info("TeamActivityMgr loadTeamActivityFromCTS success, us:{}", _m_userServer.getServerTypeId());
            }
        }, 50, () ->
        {
            //50次重试失败后进行预警
            CommLog.error("TeamActivityMgr loadTeamActivityFromCTS retry 50 times fail, us:{}", _m_userServer.getServerTypeId());
        });
    }

    /**
     * 根据队伍ID查询对应的队伍数据列表
     * @param _activityInstanceId
     * @return
     */
    public ArrayList<TeamActivityInfo> getTeamListByActivityInstanceId(long _activityInstanceId)
    {
        _lock();

        try
        {
            ArrayList<TeamActivityInfo> teamList = _m_hmActivityTeamListMap.get(_activityInstanceId);
            return null == teamList ? null : new ArrayList<>(teamList);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 根据队伍ID查询对应的活动实例ID
     * @param _teamId
     * @return
     */
    public TeamActivityInfo lookupByTeam(long _teamId)
    {
        _lock();

        try
        {
            return _m_hmTeamActivityMap.get(_teamId);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 添加队伍与活动的映射关系
     * @param _info
     */
    public void addTeamActivityMap(TeamActivityInfo _info)
    {
        _lock();

        try
        {
            if(_m_hmTeamActivityMap.containsKey(_info.getTeamId()))
                return;

            _m_hmTeamActivityMap.put(_info.getTeamId(), _info);
            _m_hmActivityTeamListMap.computeIfAbsent(_info.getActivityInstanceId(), k -> new ArrayList<>()).add(_info);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 根据队伍ID查询活动实例ID
     * @param _teamId
     */
    public void removeByTeam(long _teamId)
    {
        _lock();

        try
        {
            if(!_m_hmTeamActivityMap.containsKey(_teamId))
                return;

            TeamActivityInfo info = _m_hmTeamActivityMap.remove(_teamId);
            if(null == info)
                return;

            ArrayList<TeamActivityInfo> teamList = _m_hmActivityTeamListMap.get(info.getActivityInstanceId());
            if(null != teamList)
            {
                teamList.remove(info);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 根据活动实例ID查询队伍列表，并移除映射关系
     * @param _activityInstanceId
     */
    public void removeByActivity(long _activityInstanceId)
    {
        _lock();

        try
        {
            ArrayList<TeamActivityInfo> teamList = _m_hmActivityTeamListMap.remove(_activityInstanceId);
            if (teamList != null)
            {
                for (TeamActivityInfo info : teamList)
                {
                    _m_hmTeamActivityMap.remove(info.getTeamId());
                }
            }
        }
        finally
        {
            _unlock();
        }
    }
}
