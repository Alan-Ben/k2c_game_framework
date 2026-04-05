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
 * 玩家粒度群组数据管理器抽象基类
 * 与 _APlayerGroupMgr 同级，自身即为一个特殊的群组，直接管理 _APlayerGroupPlayerInfo 玩家数据集合
 *
 * 主要功能：
 * 1. 直接管理 _APlayerGroupPlayerInfo（按 CID 索引）
 * 2. 本地不存在时向对应 US 发起 RPC，拉取玩家数据后构造并缓存
 *
 * @param <P> 玩家数据类型，必须继承 _APlayerGroupPlayerInfo
 */
public abstract class _ATPlayerGroupPlayerMgr<P extends _APlayerGroupPlayerInfo>
{
    // 所属活动对象
    private _ATActivityInfo _m_activity;
    // 玩家数据，按 CID 索引，直接管理
    private HashMap<Long, P> _m_playerMap;
    // 玩家数据列表，保持插入顺序，用于顺序遍历
    private ArrayList<P> _m_playerList;
    // 锁对象
    private MutexObject _m_groupMgrMutex;

    public _ATPlayerGroupPlayerMgr(_ATActivityInfo _activity)
    {
        _m_activity = _activity;
        _m_playerMap = new HashMap<>();
        _m_playerList = new ArrayList<>();
        _m_groupMgrMutex = new MutexObject();
    }

    public _ATActivityInfo getActivity() {return _m_activity;}

    protected void _groupMgrLock() {_m_groupMgrMutex.lock();}
    protected void _groupMgrUnlock() {_m_groupMgrMutex.unlock();}

    // ==================== 玩家数据管理 ====================
    /**
     * 启动阶段直接写入玩家数据，跳过 ensure* 异步流程，防重复插入
     * @param _player 已构建好的玩家数据对象
     */
    public void sInitAdd(P _player)
    {
        if (null == _player)
            return;

        _groupMgrLock();
        try
        {
            // 已存在则跳过，防止重复注入
            if (_m_playerMap.containsKey(_player.getCid()))
                return;

            _m_playerMap.put(_player.getCid(), _player);
            _m_playerList.add(_player);
        }
        finally
        {
            _groupMgrUnlock();
        }
    }

    /**
     * 按 CID 查找玩家数据
     * @param _cid 玩家CID
     * @return 玩家数据，不存在返回 null
     */
    public P lookupPlayer(long _cid)
    {
        _groupMgrLock();
        try
        {
            return _m_playerMap.get(_cid);
        }
        finally
        {
            _groupMgrUnlock();
        }
    }

    /**
     * 确保玩家数据加载，异步完成后回调
     * @param _cid         玩家CID
     */
    public void ensurePlayer(long _cid, _ICallBackResultT<P> _callback)
    {
        //检索玩家数据
        _groupMgrLock();
        try
        {
            P player = lookupPlayer(_cid);
            if(null !=  player)
            {
                //有数据则直接回调
                if(null != _callback) {
                    ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                        @Override
                        public void run() {
                            _callback.onRunOver(Result.SUCC, player);
                        }
                    });
                }

                return ;
            }

            //调用创建处理函数
            _buildPlayerInfo(_cid, new _ICallBackResultT<P>() {
                @Override
                public void onRunOver(Result _result, P _player) {
                    if(Result.SUCC != _result || null == _player)
                    {
                        CommLog.error("buildPlayerInfo failed, cid: " + _cid);
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
                        if(_m_playerMap.containsKey(_cid)) {
                            _player =  _m_playerMap.get(_cid);
                        }
                        else
                        {
                            //放入数据集
                            _m_playerMap.put(_cid, _player);
                            _m_playerList.add(_player);
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
     * 此时需要创建玩家数据，如需要请求US数据则需要通过RPC异步处理，如不需要则直接处理并调用回调即可
     * @param _cid         玩家CID
     */
    protected abstract void _buildPlayerInfo(long _cid, _ICallBackResultT<P> _callback);
}
