package NPUSServer.CommonActivityMgr.Core;

import Common.ServerObj.ServerObj_RankSettleActivityTeam;
import Common.ServerObj.ServerObj_RankSettleActivityTeamList;
import NPCommon.Util.CommonFunc;
import NPUSServer.TeamActivityMgr.TeamActivityInfo;
import USDB.Bo.ActivityBaseBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class ActivitySettleTeamMgr
{
    //所属活动对象
    private _AActivityBase _m_activity;

    //当前结算时队长对应的盟主
    private HashMap<Long, Long> _m_hmTeamLeaderCidMap;
    //当前结算时所有队长CID数据集合
    private HashSet<Long> _m_hsTeamLeaderCidSet;

    //活动数据BO
    private ActivityBaseBO _m_bo;

    public ActivitySettleTeamMgr(_AActivityBase _activity, ActivityBaseBO _bo)
    {
        _m_activity = _activity;
        _m_bo = _bo;

        _m_hmTeamLeaderCidMap = new HashMap<>();
        _m_hsTeamLeaderCidSet = new HashSet<>();

        if(null != _m_bo.getSettledGuildLeadersSet())
        {
            ByteBuffer buff = ByteBuffer.wrap(_m_bo.getSettledGuildLeadersSet());
            ServerObj_RankSettleActivityTeamList objList = new ServerObj_RankSettleActivityTeamList();
            objList.readPackage(buff);

            for(int i = 0; i < objList.getList().size(); i++)
            {
                ServerObj_RankSettleActivityTeam obj = objList.getList().get(i);
                if(null == obj)
                    continue;

                _m_hmTeamLeaderCidMap.put(obj.getTeamId(), obj.getLeaderCid());
                _m_hsTeamLeaderCidSet.add(obj.getLeaderCid());
            }
        }
    }

    private void _lock() {_m_activity._activityLock();}
    private void _unlock() {_m_activity._activityUnlock();}

    /**
     * 获取并记录此时所有队长CID集合
     */
    public void dumpAllLeaderCid(ArrayList<TeamActivityInfo> _list)
    {
        _lock();

        try
        {
            _m_hmTeamLeaderCidMap.clear();
            _m_hsTeamLeaderCidSet.clear();

            ServerObj_RankSettleActivityTeamList listObj = new ServerObj_RankSettleActivityTeamList();
            if(null != _list && !_list.isEmpty())
            {
                for(int i = 0; i < _list.size(); i++)
                {
                    TeamActivityInfo obj = _list.get(i);
                    if(null == obj)
                        continue;

                    //记录当前队伍的队长CID数据
                    ServerObj_RankSettleActivityTeam teamObj = new ServerObj_RankSettleActivityTeam();
                    teamObj.setTeamId(obj.getTeamId());
                    teamObj.setLeaderCid(obj.getLeaderCid());
                    listObj.addList(teamObj);

                    //记录当前队伍的队长CID数据到内存集合中
                    _m_hmTeamLeaderCidMap.put(obj.getTeamId(), obj.getLeaderCid());
                    //记录当前队伍的队长CID数据到内存集合中
                    _m_hsTeamLeaderCidSet.add(obj.getLeaderCid());
                }
            }

            _m_bo.saveSettledActivityTeamLeadersSet(_m_activity.getUSServer().getBM(), CommonFunc.ByteBfferToBytes(listObj.makePackage()));
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 是否队长检查
     * @param _cid
     * @return
     */
    public boolean isLeader(long _cid)
    {
        _lock();

        try
        {
            return _m_hsTeamLeaderCidSet.contains(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定队伍的队长CID
     * @param _teamId
     * @return
     */
    public long getLeaderCid(long _teamId)
    {
        _lock();

        try
        {
            Long value = _m_hmTeamLeaderCidMap.get(_teamId);

            return null == value ? 0 : value;
        }
        finally
        {
            _unlock();
        }
    }
}
