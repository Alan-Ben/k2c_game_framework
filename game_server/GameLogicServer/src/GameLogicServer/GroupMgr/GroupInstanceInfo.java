package GameLogicServer.GroupMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_IntList;
import GLSDB.Bo.GroupInstanceBO;
import GameLogicServer.Common.GLS_ID;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.HashSet;

/**
 * 分组实例对象，包含分组实例基础数据，活动处理对象，功能转发协议
 */
public class GroupInstanceInfo
{
    //分组实例bo数据
    private GroupInstanceBO _m_bo;

    //实例ID，基于规则生成：groupId*100 + 2位服务器id
    private long _m_lInstanceId;

    //活动对象
    private _ATActivityInfo<?, ?> _m_activity;

    //参与的US服务器ID集合
    private HashSet<Integer> _m_usIdSet;
    //锁对象
    private MutexAtom _m_mutex;

    public GroupInstanceInfo(GroupInstanceBO bo, _ATActivityInfo<?, ?> _activity)
    {
        _m_bo = bo;

        _m_lInstanceId = GLS_ID.makeInstanceId(_m_bo.getGroupId());

        _m_activity = _activity;

        _m_usIdSet = new HashSet<>();

        _m_mutex = new MutexAtom();

        _initUsIdSet();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public GroupInstanceBO getBo() {return _m_bo;}

    public _ATActivityInfo<?, ?> getActivity() {return _m_activity;}

    public boolean isNeedDiscard() {return _m_bo.getNeedDiscard();}

    /**
     * 初始化US服务器ID集合
     */
    private void _initUsIdSet()
    {
        _m_usIdSet.clear();

        if(null == _m_bo.getUsIds())
            return;

        ByteBuffer buff = ByteBuffer.wrap(_m_bo.getUsIds());
        Common_IntList listObj = new Common_IntList();
        listObj.readPackage(buff);
        _m_usIdSet.addAll(listObj.getValueList());
    }

    /**
     * 获取实例ID（用于全区数据标识）
     * @return
     */
    public long getInstanceId()
    {
        return _m_lInstanceId;
    }

    /**
     * 判断US服务器ID集合是否为空
     * @return
     */
    public boolean isUsIdSetEmpty()
    {
        _lock();

        try
        {
            return _m_usIdSet.isEmpty();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 添加US服务器ID到集合中，并保存数据库
     * @param _usId
     */
    public void addUsId(int _usId)
    {
        _lock();

        try
        {
            if(!_m_usIdSet.add(_usId))
                return;

            _saveUsIdSet();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 从集合中移除US服务器ID，并保存数据库
     * @param _usId
     */
    public void removeUsId(int _usId)
    {
        _lock();

        try
        {
            if(!_m_usIdSet.remove(_usId))
                return;

            _saveUsIdSet();

            //移除US服务器后需要检查是否满足销毁条件，满足条件则标记销毁并保存数据库
            if(isNeedDiscard())
            {
                ALSynTaskManager.getInstance().regTask(()->
                {
                    GroupInstanceMgr.getInstance().checkDiscard(this);
                });
            }
        }
        finally
        {
            _unlock();
        }
    }

    private void _saveUsIdSet()
    {
        Common_IntList listObj = new Common_IntList();
        listObj.getValueList().addAll(_m_usIdSet);

        _m_bo.saveUsIds(GameLogicServer.getInstance().getBM(), CommonFunc.ByteBfferToBytes(listObj.makePackage()));
    }

    /**
     * 确保玩家数据与所属群组数据均已加载，委托给活动对象处理
     * @param _cid      玩家CID
     * @param _groupId  玩家所属群组ID
     */
    public void dealMsg(long _cid, long _groupId, _IWCGBasicRequestCommiter _committer, byte[] _msg, byte[] _addInfo)
    {
        //无返回对象不处理
        if(null == _committer)
            return ;

        //确认玩家数据是否存在
        _m_activity.dealMsg(_cid, _groupId, _committer, _msg, _addInfo);
    }

    /**
     * 更新销毁标记，需要检查是否满足销毁条件，如果不满足需要后续继续检查，满足条件则标记销毁并保存数据库
     */
    public void tryDiscard()
    {
        if(_m_bo.getNeedDiscard())
            return;

        _m_bo.saveNeedDiscard(GameLogicServer.getInstance().getBM(), true);

        CommLog.info("GroupInstanceInfo tryDiscard, groupId:{} instanceId:{} activityId:{}", _m_bo.getGroupId(), getInstanceId(), _m_bo.getActivityId());
    }

    /**
     * 销毁分组实例，删除数据库记录
     */
    protected void _discard()
    {
        _m_bo.del(GameLogicServer.getInstance().getBM());

        CommLog.info("GroupInstanceInfo discard, groupId:{} instanceId:{} activityId:{}", _m_bo.getGroupId(), getInstanceId(), _m_bo.getActivityId());

        ALSynTaskManager.getInstance().regTask(() -> {_m_activity._discard();});
    }
}
