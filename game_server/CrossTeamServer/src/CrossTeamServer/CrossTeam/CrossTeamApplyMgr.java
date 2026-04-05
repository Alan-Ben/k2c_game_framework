package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CTSDB.Bo.CrossTeamApplyBO;
import Common.CrossTeamObj.CrossTeam_ApplyInfo;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;

public class CrossTeamApplyMgr
{
    //所属队伍数据
    private CrossTeamInfo _m_team;

    //请求列表
    private ArrayList<CrossTeamApplyBO> _m_alApplyBoList;

    //锁对象
    private MutexAtom _m_mutex;

    public CrossTeamApplyMgr(CrossTeamInfo _team)
    {
        _m_team = _team;

        _m_alApplyBoList = new ArrayList<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    protected void _initApplyBo(CrossTeamApplyBO _applyBo)
    {
        _m_alApplyBoList.add(_applyBo);
    }

    /**
     * 队伍数据初始化完成后调用，对申请列表进行排序
     */
    protected void _onInited()
    {
        _sort();
    }

    /**
     * 对申请列表进行排序，按照申请时间升序排列
     */
    private void _sort()
    {
        _m_alApplyBoList.sort((_o1, _o2) ->
        {
            return Long.compare(_o1.getApplyMs(), _o2.getApplyMs());
        });
    }

    /**
     * 生成申请列表数据
     * @param _list
     */
    public void makeApplyList(ArrayList<CrossTeam_ApplyInfo>_list)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alApplyBoList.size(); i++)
            {
                CrossTeamApplyBO bo = _m_alApplyBoList.get(i);
                if (null == bo)
                    continue;

                CrossTeam_ApplyInfo applyInfo = new CrossTeam_ApplyInfo();
                applyInfo.setApplyCid(bo.getApplyCid());
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
     * 根据玩家cid查找申请数据
     *
     * @param _cid
     * @return
     */
    public CrossTeamApplyBO lookupByCid(long _cid)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alApplyBoList.size(); i++)
            {
                CrossTeamApplyBO bo = _m_alApplyBoList.get(i);
                if (null == bo)
                    continue;

                if (bo.getApplyCid() == _cid)
                    return bo;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 添加玩家的申请数据（添加内存数据和数据库数据）
     * @param _cid
     */
    public void addPlayerApply(long _cid)
    {
        _lock();

        try
        {
            CrossTeamApplyBO bo = lookupByCid(_cid);
            if(null == bo)
            {
                CrossTeamApplyBO newBo = new CrossTeamApplyBO();
                newBo.setTeamId(_m_team.getBM(), _m_team.getTeamId());
                newBo.setApplyCid(_m_team.getBM(), _cid);
                newBo.setApplyMs(_m_team.getBM(), CommonFunc.getNowTimeMS());
                newBo.insert(_m_team.getBM());

                _m_alApplyBoList.add(newBo);

                //将申请数据添加到玩家申请管理器里
                ALSynTaskManager.getInstance().regTask(()->
                {
                    _m_team.getGroup().getPlayerApplyMgr().addApply(newBo);
                });
            }
            else
            {
                bo.saveApplyMs(_m_team.getBM(), CommonFunc.getNowTimeMS());
            }

            //对申请数据进行排序
            _sort();
            //移除超过上限的申请数据
            while(_m_alApplyBoList.size() > 0 && _m_alApplyBoList.size() > RefGeneral.Ref().activity_team_apply_limit)
            {
                int lastIdx = _m_alApplyBoList.size() - 1;
                CrossTeamApplyBO removeBo = _m_alApplyBoList.remove(lastIdx);
                if(null == removeBo)
                    continue;

                removeBo.del(_m_team.getBM());
                ALSynTaskManager.getInstance().regTask(()->
                {
                    _m_team.getGroup().getPlayerApplyMgr().removeApplyBo(removeBo);
                });
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除玩家的申请数据（移除内存数据和数据库数据）
     * @param _cid
     * @return
     */
    public CrossTeamApplyBO removePlayerApply(long _cid)
    {
        _lock();

        try
        {
            CrossTeamApplyBO bo = lookupByCid(_cid);
            if(null == bo)
                return null;

            _m_alApplyBoList.remove(bo);
            bo.del(_m_team.getBM());

            final CrossTeamApplyBO removeBo = bo;
            ALSynTaskManager.getInstance().regTask(()->
            {
                _m_team.getGroup().getPlayerApplyMgr().removeApplyBo(removeBo);
            });

            return bo;
        }
        finally
        {
            _lock();
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
            _m_alApplyBoList.remove(_bo);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 队伍解散时调用，移除所有申请数据
     */
    protected void _onDissolve()
    {
        ArrayList<CrossTeamApplyBO> applyList = null;

        _lock();

        try
        {
            if(_m_alApplyBoList.isEmpty())
                return;

            applyList = new ArrayList<>(_m_alApplyBoList);

            //销毁成员数据
            _m_alApplyBoList.clear();
            _m_team.getBM().getBM(CrossTeamApplyBO.class).delAll("team_id", _m_team.getTeamId());
        }
        finally
        {
            _unlock();
        }

        //对销毁的的队伍成员数据进行处理
        if(null != applyList)
        {
            _m_team.getGroup().getPlayerApplyMgr().removeApplyBoList(applyList);
        }
    }
}
