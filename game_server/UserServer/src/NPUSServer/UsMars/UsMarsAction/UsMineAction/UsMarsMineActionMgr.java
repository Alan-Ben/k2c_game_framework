package NPUSServer.UsMars.UsMarsAction.UsMineAction;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UsMars.UsMarsAction.UsMarsActionCore;
import USDB.Bo.UsMarsMineOccupyBO;

import java.util.Hashtable;
import java.util.List;

/**
 * Us所有火星行为的数据管理类
 */
public class UsMarsMineActionMgr
{
    //核心管理器对象
    private UsMarsActionCore _m_acActionCore;
    /** 所有进攻行为的数据集 */
    private Hashtable<Long, UsMarsMineAttackAction> _m_htAttackActionList;
    //前往某个目标的总队列统计数据集，用于判断是否有队伍向某个矿前进
    private Hashtable<Long, Integer> _m_htTargetAttCount;

    private MutexAtom _m_mutex;

    public UsMarsMineActionMgr(UsMarsActionCore _actionCore)
    {
        _m_acActionCore = _actionCore;

        _m_htAttackActionList = new Hashtable<Long, UsMarsMineAttackAction>();
        _m_htTargetAttCount = new Hashtable<Long, Integer>();

        _m_mutex = new MutexAtom();
    }

    public UsMarsActionCore getActionCore() {return _m_acActionCore;}
    public NPUserServer getUSServer() {return _m_acActionCore.getUSServer();}
    public BM getBM() {return _m_acActionCore.getUSServer().getBM();}

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /**
     * 初始化数据库，将数据加入到处理队列
     * @return
     */
    public boolean s_init()
    {
        //火星占领玩家数据
        List<UsMarsMineOccupyBO> occupyBoList = getUSServer().getBM().getBM(UsMarsMineOccupyBO.class).s_findAll();
        if(null == occupyBoList)
        {
            USLog.error(getUSServer(), "MarsActionMgr.init UsMarsMineOccupyBO fail.");
            return false;
        }

        //每个行为加入处理队列
        for(int i = 0; i < occupyBoList.size(); i++)
        {
            UsMarsMineOccupyBO bo = occupyBoList.get(i);
            if(null == bo)
                continue;

            //构造数据对象
            UsMarsMineAttackAction action = new UsMarsMineAttackAction(this, bo);
            //添加到数据集
            _m_htAttackActionList.put(action.getActionId(), action);
            //添加目标管理统计
            _addTargetCount(action.getMineInstanceId());
            //添加到总的行为管理器中
            _m_acActionCore.addAction(action);
        }

        return true;
    }

    /**
     * 取消某个进攻行为，本函数不做重复判断
     * 重复的判断在外围进行处理
     * @param _isOtherTeamForward 是否有其他玩家前往，如果false且有其他玩家前往则报错MARS_MINE_OTHER_PLAYER_FORWARD
     */
    public Result addAttackAction(long _mineInstanceId, boolean _isOtherTeamForward, ServerObj_MarsTeam_OccupyMinePlayer _player, long _startCollectMs, long _collectSpeed)
    {
        UsMarsMineAttackAction action;

        _lock();

        try
        {
            //判断是否有其他玩家前往
            if(!_isOtherTeamForward && getMineAttackCount(_mineInstanceId) > 0)
            {
                return MarsErr.MARS_MINE_OTHER_PLAYER_FORWARD;
            }

            UsMarsMineOccupyBO bo = new UsMarsMineOccupyBO();
            bo.setMineInstaceId(getBM(), _mineInstanceId);
            bo.setCid(getBM(), _player.getCid());//增加一个冗余字段，方便数据表查询
            bo.setOccupyPlayer(getBM(), CommonFunc.ByteBfferToBytes(_player.makePackage()));
            bo.setStartCollectMs(getBM(), _startCollectMs);
            bo.setCollectSpeed(getBM(), _collectSpeed);
            bo.insert(getBM());

            //构造数据对象
            action = new UsMarsMineAttackAction(this, bo);
            //添加到数据集
            _m_htAttackActionList.put(action.getActionId(), action);
            //添加统计计数
            _addTargetCount(action.getMineInstanceId());
        }
        finally
        {
            _unlock();
        }

        //先添加到本数据集，再添加到总的行为管理器中
        //在锁外处理避免冲突
        _m_acActionCore.addAction(action);

        return Result.SUCC;
    }

    /**
     * 主动取消某个进攻行为
     * @param _attackActionId
     */
    public Result cancelAttackAction(long _attackActionId)
    {
        //取消的时候，需要先从总管理器中取消，如果取消失败则直接返回
        //查询数据对象
        UsMarsMineAttackAction action = null;
        _lock();
        try
        {
            action = _m_htAttackActionList.get(_attackActionId);
        }
        finally {
            _unlock();
        }

        //尝试取消
        if(null == action || !_m_acActionCore.cancelAction(action))
        {
            return MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND;
        }

        //从本数据集中移除
        _rmvAction(action);

        return Result.SUCC;
    }

    /**
     * 获取当前前往某个矿的统计计数
     * @param _mineInstanceId
     * @return
     */
    public int getMineAttackCount(long _mineInstanceId)
    {
        _lock();

        try
        {
            Integer count = _m_htTargetAttCount.get(_mineInstanceId);
            if(null == count)
                return 0;

            return count;
        }
        finally {
            _unlock();
        }
    }

    /**
     * 移除当前的注册行为对象
     * @param _action
     */
    protected void _rmvAction(UsMarsMineAttackAction _action)
    {
        if(null == _action)
            return ;

        _lock();

        try
        {
            _m_htAttackActionList.remove(_action.getActionId());
            //减少统计计数
            _rmvTargetCount(_action.getMineInstanceId());
        }
        finally {
            _unlock();
        }
    }

    /**
     * 目标统计的相关处理函数
     * @param _targetMineId
     */
    protected void _addTargetCount(long _targetMineId)
    {
        _lock();

        try
        {
            Integer count = _m_htTargetAttCount.get(_targetMineId);
            if(null == count)
            {
                _m_htTargetAttCount.put(_targetMineId, 1);
            }
            else
            {
                _m_htTargetAttCount.put(_targetMineId, count + 1);
            }
        }
        finally {
            _unlock();
        }
    }
    protected void _rmvTargetCount(long _targetMineId)
    {
        _lock();

        try
        {
            Integer count = _m_htTargetAttCount.get(_targetMineId);
            if(null == count)
            {
                ALServerLog.Error("Rmv a target mine count when count is empty! mineId: " + _targetMineId);
                return ;
            }

            //扣除次数，如果为0则删除
            count--;
            if(0 == count)
            {
                _m_htTargetAttCount.remove(_targetMineId);
            }
            else
            {
                _m_htTargetAttCount.put(_targetMineId, count);
            }
        }
        finally {
            _unlock();
        }
    }
}
