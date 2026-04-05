package GameLogicServer.GroupMgr.PlayerGroupMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResultT;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * 玩家归属群体数据管理
 * @param <G>
 */
public abstract class _ATGroupMgr<P extends _APlayerGroupPlayerInfo, G extends _ATGroupInfo<P>>
{
    //所属活动对象
    private _ATActivityInfo _m_activity;
    //玩家群体分组Map
    private HashMap<Long, G> _m_hmPlayerGroupMap;
    //玩家群组数据列表
    private ArrayList<G> _m_alPlayerGroupList;
    //锁对象，只对自身类的group数据集进行保护，继承类根据需要增加锁保护其他数据
    private MutexObject _m_groupMgrMutex;

    public _ATGroupMgr(_ATActivityInfo _activity)
    {
        _m_activity = _activity;

        _m_hmPlayerGroupMap = new HashMap<>();
        _m_alPlayerGroupList = new ArrayList<>();

        _m_groupMgrMutex = new MutexObject();
    }

    public _ATActivityInfo getActivity() {return _m_activity;}

    protected void _groupMgrLock() {_m_groupMgrMutex.lock();}
    protected void _groupMgrUnlock() {_m_groupMgrMutex.unlock();}

    /**
     * 启动阶段直接写入群组数据，跳过 ensure* 异步流程，防重复插入
     * @param _group 已构建好的群组数据对象
     */
    public void sInitAdd(G _group)
    {
        if (null == _group)
            return;

        _groupMgrLock();
        try
        {
            // 已存在则跳过，防止重复注入
            if (_m_hmPlayerGroupMap.containsKey(_group.getGroupId()))
                return;

            _m_hmPlayerGroupMap.put(_group.getGroupId(), _group);
            _m_alPlayerGroupList.add(_group);
        }
        finally
        {
            _groupMgrUnlock();
        }
    }

    /**
     * 查找指定的玩家群体
     * @param _groupId
     * @return
     */
    public G lookup(long _groupId)
    {
        _groupMgrLock();

        try
        {
            return _m_hmPlayerGroupMap.get(_groupId);
        }
        finally
        {
            _groupMgrUnlock();
        }
    }


    /**
     * 确保群组数据均已加载，异步完成后回调
     * @param _groupId         玩家CID
     */
    public void ensureGroup(long _groupId, _ICallBackResultT<G> _callback)
    {
        //检索玩家数据
        _groupMgrLock();
        try
        {
            G group = lookup(_groupId);
            if(null !=  group)
            {
                //有数据则直接回调
                if(null != _callback) {
                    ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                        @Override
                        public void run() {
                            _callback.onRunOver(Result.SUCC, group);
                        }
                    });
                }

                return ;
            }

            //调用创建处理函数
            buildGroup(_groupId, new _ICallBackResultT<G>() {
                @Override
                public void onRunOver(Result _result, G _group) {
                    if(Result.SUCC != _result || null == _group)
                    {
                        CommLog.error("buildGroupInfo failed, cid: " + _groupId);
                        if(null != _callback) {
                            ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                                @Override
                                public void run() {
                                    _callback.onRunOver(CommErr.SYS_ERR, null);
                                }
                            });
                        }
                        return;
                    }

                    //增加玩家数据到本地缓存
                    _groupMgrLock();
                    try
                    {
                        //判断是否已经存在，已经存在则试用旧数据
                        if(_m_hmPlayerGroupMap.containsKey(_groupId)) {
                            _group =  _m_hmPlayerGroupMap.get(_groupId);
                        }
                        else
                        {
                            //放入数据集
                            _m_hmPlayerGroupMap.put(_groupId, _group);
                        }

                        //处理回调
                        if(null != _callback) {
                            ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                                @Override
                                public void run() {
                                    _callback.onRunOver(Result.SUCC, null);
                                }
                            });
                        }
                    }
                    finally
                    {
                        _groupMgrUnlock();
                    }
                }
            });
        }
        finally
        {
            _groupMgrUnlock();
        }
    }

    /**
     * 同步队伍成员数据到本地 Group，子类按需覆写
     * @param _groupId 群组ID
     * @param _teamId  队伍ID
     */
    public void syncGroup(long _groupId, long _teamId)
    {
        CommLog.warn("_ATGroupMgr.syncGroup not implemented, groupId:{}, teamId:{}", _groupId, _teamId);
    }

    /**
     * 移除指定 group 及其所有玩家主体数据，子类按需覆写
     * @param _groupId 群组ID
     */
    public void removeGroup(long _groupId)
    {
        CommLog.warn("_ATGroupMgr.removeGroup not implemented, groupId:{}", _groupId);
    }

    /**
     * 从数据集中移除指定 group（仅供子类覆写时调用）
     * @param _groupId 群组ID
     */
    protected void _removeGroup(long _groupId)
    {
        _groupMgrLock();
        try
        {
            G group = _m_hmPlayerGroupMap.remove(_groupId);
            if (null != group)
                _m_alPlayerGroupList.remove(group);
        }
        finally
        {
            _groupMgrUnlock();
        }
    }

    /**
     * 此时需要创建玩家集群的数据，如需要请求跨服数据则需要通过RPC异步处理，如不需要则直接处理并调用回调即可
     * @param _groupId     玩家所属群组ID（队伍ID等）
     */
    protected abstract void buildGroup(long _groupId, _ICallBackResultT<G> _callback);
}
