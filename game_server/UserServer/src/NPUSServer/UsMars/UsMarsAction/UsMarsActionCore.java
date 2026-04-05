package NPUSServer.UsMars.UsMarsAction;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.GuildRallyErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.SynTask.SynUsMarsActionCoreTickTask;
import NPUSServer.UsMars.UsMarsAction.UsJoinRallyAction.UsMarsJoinRallyAction;
import NPUSServer.UsMars.UsMarsAction.UsMineAction.UsMarsMineActionMgr;
import USDB.Bo.UsMarsJoinRallyBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 针对本服务器内的火星相关资源进行行为操作的相关行为管理对象
 * 如前往矿区，后续的交互行为等
 */
public class UsMarsActionCore
{
    //US服务器对象
    private NPUserServer _m_usUSServer;

    //所有需要监听行为的接口队列
    private ArrayList<_AUsMarsBasicAction> _m_lMarsActionList;

    //tick操作序列号
    private long _m_lTickSerialize;

    private MutexAtom _m_mutex;

    //矿相关行为管理器
    private UsMarsMineActionMgr _m_mamMineActionMgr;

    public UsMarsActionCore(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_lMarsActionList = new ArrayList<_AUsMarsBasicAction>();

        _m_lTickSerialize = ALSerializeMaker.makeNewSerialize();

        _m_mutex = new MutexAtom();

        _m_mamMineActionMgr = new UsMarsMineActionMgr(this);
    }


    public NPUserServer getUSServer() {return _m_usUSServer;}
    public BM getBM() {return getUSServer().getBM();}

    public UsMarsMineActionMgr getMineActionMgr() {return _m_mamMineActionMgr;}

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 初始化
     * @return
     */
    public boolean s_init()
    {
        if(!_m_mamMineActionMgr.s_init()) {
            ALServerLog.Error("Init Mine Action Fail!");
            return false;
        }

        List<UsMarsJoinRallyBO> joinBoList = getBM().getBM(UsMarsJoinRallyBO.class).s_findAll();
        if (joinBoList != null) {
            for (UsMarsJoinRallyBO bo : joinBoList) {
                if (bo == null) {
                    continue;
                }

                UsMarsJoinRallyAction action = new UsMarsJoinRallyAction(this, bo);
                addAction(action);
            }
        }

        //开启Tick任务
        startTickTask();

        return true;
    }

    /**
     * 开启Tick任务处理
     */
    public void startTickTask()
    {
        _lock();

        try {
            //刷新序列号
            _m_lTickSerialize = ALSerializeMaker.makeNewSerialize();

            //开启任务处理
            ALSynTaskManager.getInstance().regTask(new SynUsMarsActionCoreTickTask(this, _m_lTickSerialize), 1000);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 服务端定时检测的处理函数
     * @param _nowTimeMS
     */
    private ArrayList<_AUsMarsBasicAction> tmpNeedDealList = new ArrayList<_AUsMarsBasicAction>();
    public void tick(long _tickSerialize, long _nowTimeMS)
    {
        _lock();

        try
        {
            //序列号不一致则不处理
            if(_tickSerialize != _m_lTickSerialize)
                return ;

            tmpNeedDealList.clear();

            for(int i = 0; i < _m_lMarsActionList.size(); )
            {
                _AUsMarsBasicAction tmpAction = _m_lMarsActionList.get(i);
                if(null == tmpAction)
                {
                    _m_lMarsActionList.remove(i);
                    continue;
                }

                //判断是否到达时间
                if(_nowTimeMS >= tmpAction.getTriggerTimeMS())
                {
                    //从队列移除，并加入待处理队列
                    _m_lMarsActionList.remove(i);
                    tmpNeedDealList.add(tmpAction);

                    continue;
                }

                //累加处理下一个
                i++;
            }
        }
        finally {
            _unlock();
        }

        //逐个处理触发行为
        for(_AUsMarsBasicAction action : tmpNeedDealList)
        {
            action._trigger(_nowTimeMS);

            //调用行为的数据清理
            action._clearData();
        }
        //清空队列
        tmpNeedDealList.clear();
    }

    /**
     * 添加一个行为
     * @param _action
     */
    public void addAction(_AUsMarsBasicAction _action)
    {
        if(null == _action)
            return ;

        _lock();
        try
        {
            _m_lMarsActionList.add(_action);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 取消当前行为的处理，返回是否成功取消
     * @param _action
     * @return 如果成功取消返回true
     */
    public boolean cancelAction(_AUsMarsBasicAction _action)
    {
        boolean res = false;
        _lock();

        try
        {
            res = _m_lMarsActionList.remove(_action);
        }
        finally {
            _unlock();
        }

        //如果清理成功，需要触发相关处理，并清理数据
        if(res)
        {
            //处理取消行为
            _action._dealCancel();

            //清理数据
            _action._clearData();
        }

        return res;
    }

    /**
     * 新增加入集结行为，并立即加入tick队列
     */
    public Result addJoinRallyAction(long _guildId, long _rallyId, long _cid, long _teamId, long _triggerTimeMs)
    {
        UsMarsJoinRallyBO bo = null;
        UsMarsJoinRallyAction action;

        _lock();
        try {
            bo = new UsMarsJoinRallyBO();
            bo.setGuildId(getBM(), _guildId);
            bo.setRallyId(getBM(), _rallyId);
            bo.setCid(getBM(), _cid);
            bo.setTeamId(getBM(), _teamId);
            bo.setTriggerTimeMs(getBM(), _triggerTimeMs);
            bo.insert(getBM());

            action = new UsMarsJoinRallyAction(this, bo);
        } catch (Exception e) {
            if (bo != null && bo.getId() > 0) {
                bo.del(getBM());
            }
            return GuildRallyErr.RALLY_OP_DISABLE;
        } finally {
            _unlock();
        }

        addAction(action);
        return Result.SUCC;
    }
}

