package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CTSDB.Bo.CrossTeamApplyBO;
import Common.CrossTeamObj.CrossTeam_PlayerApplyInfo;
import CrossTeamServer.CrossTeamServer;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * 玩家队伍申请数据管理器
 */
public class CrossTeamPlayerApplyMgr
{
    //分组实例对象
    private CrossGroup _m_group;

    //玩家申请列表，key:玩家id，value:申请列表
    private HashMap<Long, ArrayList<CrossTeamApplyBO>> _m_alPlayerApplyListMap;

    //锁对象
    private MutexAtom _m_mutex;

    public CrossTeamPlayerApplyMgr(CrossGroup _group)
    {
        _m_group = _group;

        _m_alPlayerApplyListMap = new HashMap<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public CrossGroup getGroup() {return _m_group;}

    /**
     * 生成玩家申请列表数据
     * @param _cid
     * @param _list
     */
    public void makePlayerApplyList(long _cid, ArrayList<CrossTeam_PlayerApplyInfo> _list)
    {
        _lock();

        try
        {
            ArrayList<CrossTeamApplyBO> applyBoList = _m_alPlayerApplyListMap.get(_cid);
            if(null == applyBoList)
                return;

            for(int i = 0; i < applyBoList.size(); i++)
            {
                CrossTeamApplyBO bo = applyBoList.get(i);
                if(null == bo)
                    continue;

                CrossTeam_PlayerApplyInfo applyInfo = new CrossTeam_PlayerApplyInfo();
                applyInfo.setTeamId(bo.getTeamId());
                applyInfo.setApplyMs(bo.getApplyMs());

                _list.add(applyInfo);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 确保玩家申请数据的存在，如果不存在则创建
     * @param _bo
     */
    public void addApply(CrossTeamApplyBO _bo)
    {
        _lock();

        try
        {
            _m_alPlayerApplyListMap.computeIfAbsent(_bo.getApplyCid(), k -> new ArrayList<>()).add(_bo);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除玩家的所有申请数据
     * @param _cid
     */
    public void removePlayerAllApply(long _cid)
    {
        _lock();

        try
        {
            ArrayList<CrossTeamApplyBO> applyList = _m_alPlayerApplyListMap.remove(_cid);
            if(null == applyList)
                return;

            //移该玩家所有数据
            for(int i = 0; i < applyList.size(); i++)
            {
                CrossTeamApplyBO bo = applyList.get(i);
                if (null == bo)
                    continue;

                bo.del(CrossTeamServer.getInstance().getBM());
            }

            //移除玩家在各个队伍里的申请数据（只移除内存数据，bo已经在外部被移除）
            ALSynTaskManager.getInstance().regTask(()->
            {
                for(int i = 0; i < applyList.size(); i++)
                {
                    CrossTeamApplyBO bo = applyList.get(i);
                    if(null == bo)
                        continue;

                    CrossTeamInfo team = _m_group.getTeamMgr().lookup(bo.getTeamId());
                    if(null == team)
                        continue;

                    team.getApplyMgr().removeApplyBo(bo);
                }
            });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除申请数据（只移除内存数据，bo已经在外部被移除）
     * @param _bo
     */
    public void removeApplyBo(CrossTeamApplyBO _bo)
    {
        _lock();

        try
        {
            ArrayList<CrossTeamApplyBO> applyBoList = _m_alPlayerApplyListMap.get(_bo.getApplyCid());
            if(null == applyBoList)
                return;

            applyBoList.remove(_bo);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除申请数据列表（只移除内存数据，bo已经在外部被移除）
     * @param _boList
     */
    public void removeApplyBoList(ArrayList<CrossTeamApplyBO> _boList)
    {
        for(int i = 0; i < _boList.size(); i++)
        {
            CrossTeamApplyBO bo = _boList.get(i);
            if(null == bo)
                continue;

            removeApplyBo(bo);
        }
    }

    /**
     * 销毁分组下的申请数据
     * 具体的bo数据已经在 _m_mgrTeamMgr._onDiscard(); 销毁了，这里只需要清理内存数据即可
     */
    protected void _onDiscard()
    {
        _lock();

        try
        {
            _m_alPlayerApplyListMap.clear();
        }
        finally
        {
            _unlock();
        }
    }
}
