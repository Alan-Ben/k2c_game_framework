package GameLogicServer.GroupMgr.ActivityMgr;

import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._APlayerGroupPlayerInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupMgr;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATPlayerGroupPlayerMgr;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResultT;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

/**
 * GameLogic框架中的活动主体对象，对活动内的所有数据逻辑进行管理和处理
 * @param <P>
 * @param <G>
 */
public abstract class _ATActivityInfo<P extends _APlayerGroupPlayerInfo, G extends _ATGroupInfo<P>>
{
    //所属分组ID
    private long _m_lGroupId;

    //活动ID
    private long _m_lActivityId;

    //玩家数据管理对象
    private _ATPlayerGroupPlayerMgr<P> _m_pPlayerMgr;
    //玩家集群管理对象
    private _ATGroupMgr<P, G> _m_atGroupMgr;

    public _ATActivityInfo(long _groupId, long _activityId)
    {
        _m_lGroupId = _groupId;

        _m_lActivityId = _activityId;

        _m_pPlayerMgr = _createPlayerGroupPlayerMgr();
        _m_atGroupMgr = _createPlayerGroupMgr();
    }

    public long getGroupId() {return _m_lGroupId;}
    public long getActivityId() {return _m_lActivityId;}
    public _ATPlayerGroupPlayerMgr<P> getPlayerMgr() {return _m_pPlayerMgr;}
    public _ATGroupMgr<P, G> getGroupMgr() {return _m_atGroupMgr;}

    /**
     * 确保玩家数据与所属群组数据均已加载，异步完成后回调
     * @param _cid         玩家CID
     */
    public void ensurePlayer(long _cid, _ICallBackResultT<P> _callback)
    {
        if(null == _m_pPlayerMgr)
        {
            if(null == _callback)
                _callback.onRunOver(CommErr.SYS_ERR, null);
            return ;
        }

        //检索玩家数据
        _m_pPlayerMgr.ensurePlayer(_cid, _callback);
    }

    /**
     * 确保玩家数据与所属群组数据均已加载，异步完成后回调
     * @param _groupId     玩家所属群组ID（队伍ID等）
     */
    public void ensureGroup(long _groupId, _ICallBackResultT<G> _callback)
    {
        if(null == _m_atGroupMgr)
        {
            if(null == _callback)
                _callback.onRunOver(CommErr.SYS_ERR, null);
            return ;
        }

        //检索玩家数据
        _m_atGroupMgr.ensureGroup(_groupId, _callback);
    }

    /**
     * 确保玩家数据与所属群组数据均已加载，委托给活动对象处理
     * @param _cid      玩家CID
     * @param _groupId  玩家所属群组ID
     */
    public void dealMsg(long _cid, long _groupId, _IWCGBasicRequestCommiter _committer, byte[] _msg, byte[] _addInfo) {
        //无返回对象不处理
        if (null == _committer)
            return;

        //确认玩家数据是否存在
        ensurePlayer(_cid, new _ICallBackResultT<P>() {
            @Override
            public void onRunOver(Result _result, P _player) {
                if (!_result.isSucc()) {
                    _committer.commitFailRes(_result.getCode());
                    return;
                }

                //继续检查group数据，如果id为0直接用null处理
                if (_groupId == 0) {
                    //直接调用消息处理
                    _dispatchMsg(_committer, _player, null, _msg, _addInfo);
                    return;
                }

                //继续调用group处理
                ensureGroup(_groupId, new _ICallBackResultT<G>() {
                    @Override
                    public void onRunOver(Result _result, G _group) {
                        if (!_result.isSucc()) {
                            _committer.commitFailRes(_result.getCode());
                            return;
                        }

                        //检查玩家是否在群组内
                        if (!_group.isInGroup(_cid)) {
                            _committer.commitFailRes(ActivityErr.TEAM_PLAYER_NOT_IN_GROUP.getCode());
                            return;
                        }

                        //调用消息处理
                        _dispatchMsg(_committer, _player, _group, _msg, _addInfo);
                    }
                });
            }
        });
    }

    /**
     * 同步指定 group 的队伍成员数据，委托 groupMgr 处理
     * @param _groupId 群组ID
     * @param _teamId  队伍ID
     */
    public void syncGroupFromTeam(long _groupId, long _teamId)
    {
        if (null == _m_atGroupMgr)
        {
            CommLog.error("_ATActivityInfo.syncGroupFromTeam groupMgr is null, groupId:{}", _groupId);
            return;
        }
        _m_atGroupMgr.syncGroup(_groupId, _teamId);
    }

    /**
     * 移除指定 group，队伍解散时调用
     * @param _groupId 群组ID
     */
    public void removeGroup(long _groupId)
    {
        if (null == _m_atGroupMgr)
        {
            CommLog.error("_ATActivityInfo.removeGroup groupMgr is null, groupId:{}", _groupId);
            return;
        }
        _m_atGroupMgr.removeGroup(_groupId);
    }

    /**
     * 活动对象的初始化接口
     * @param _addInfo
     * @return
     */
    abstract public boolean init(ByteBuffer _addInfo);

    /**
     * 活动对象的销毁接口，释放资源等
     */
    abstract public void _discard();

    /**
     * 获取了相关对象后的具体消息处理对象
     * @param _player
     * @param _group
     * @param _msg
     * @param _addInfo
     */
    protected abstract void _dispatchMsg(_IWCGBasicRequestCommiter _committer, P _player, G _group, byte[] _msg, byte[] _addInfo);

    /**
     * 创建本活动对象的玩家管理对象
     * @return
     */
    protected abstract _ATPlayerGroupPlayerMgr<P> _createPlayerGroupPlayerMgr();
    /**
     * 创建本活动对象的玩家集群管理对象
     * @return
     */
    protected abstract _ATGroupMgr<P, G> _createPlayerGroupMgr();
}
