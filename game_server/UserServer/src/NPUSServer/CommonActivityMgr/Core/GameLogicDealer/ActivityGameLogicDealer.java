package NPUSServer.CommonActivityMgr.Core.GameLogicDealer;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_007_QuitInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_008_RegUs;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_009_SyncGroupTeamData;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_010_RemoveGroupByTeam;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_020_DealMsg;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPGeneralListener.Writer.NP2GLS_RB_Writer_001_BasicOp;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ATNPUserMsgRedirectCommiter;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

public class ActivityGameLogicDealer
{
    //活动对象
    private _AActivityBase _m_activity;

    //游戏逻辑服务器请求失败重试次数
    private int _m_iFailDiscardCount;

    public ActivityGameLogicDealer(_AActivityBase _activity)
    {
        _m_activity = _activity;
    }

    public _AActivityBase getActivity() {return _m_activity;}

    /**
     * 活动销毁相关处理：通知游戏逻辑服务器退出实例
     */
    public void _onDiscard()
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_007_QuitInstance(_m_activity),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_007_QuitInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        //通知游戏逻辑服务器退出实例失败，重试机制：每3秒重试一次，最多重试10次
                        _m_iFailDiscardCount++;
                        if(_m_iFailDiscardCount > 10)
                        {
                            CommLog.error("ActivityGameLogicDealer _onDiscard fail, errCode:{} instanceId:{} activityId:{} gameLogicInstanceId:{}",
                                    _errCode, _m_activity.getInstanceId(), _m_activity.getActivityId(), _m_activity.getGameLogicInstanceId());
                            return;
                        }

                        ALSynTaskManager.getInstance().regTask(() ->
                        {
                            _onDiscard();
                        }, 3000);
                    }
                });
    }

    /**
     * US 启动时向游戏逻辑服务器注册自身：将当前 US ID 加入实例管理，失败自动重试，最多10次
     */
    public void _regUs()
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_008_RegUs(_m_activity),
                new _IWCGCallbackDealer()
                {
                    //注册失败重试次数
                    private int _iFailRegUsCount = 0;

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_008_RegUs();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        //注册失败，重试机制：每3秒重试一次，最多重试10次
                        _iFailRegUsCount++;
                        if (_iFailRegUsCount > 10)
                        {
                            CommLog.error("ActivityGameLogicDealer _regUs fail, errCode:{} instanceId:{} activityId:{} gameLogicInstanceId:{}",
                                    _errCode, _m_activity.getInstanceId(), _m_activity.getActivityId(), _m_activity.getGameLogicInstanceId());
                            return;
                        }

                        ALSynTaskManager.getInstance().regTask(() -> _regUs(), 3000);
                    }
                });
    }

    /**
     * 处理游戏逻辑消息：转发消息到对应的游戏逻辑主体服务器
     * @param _committer
     * @param _cid
     * @param _playerGroupId 玩家归属主体ID（根据活动配置数据区分，GuildId/TeamId）
     * @param _msg
     * @param _addInfo
     * @param _failCallback
     */
    public void dealMsg(_ANPUSUserBasicMsgItem _committer,
                        long _cid, long _playerGroupId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo,
                        _ICallBackIntT<_ANPUSUserBasicMsgItem> _failCallback)
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_020_DealMsg(_m_activity, _cid, _playerGroupId, _msg, _addInfo),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_020_DealMsg();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        _committer.commitSucResByBuffer(((NP2GLS_RB_001_020_DealMsg) _retMsg).get_buffer_Msg());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        if (null != _failCallback)
                            _failCallback.onRunOver(_errCode, _committer);
                        else
                            _committer.commitFailRes(_errCode);
                    }
                });
    }

    /**
     * 处理游戏逻辑消息：重定向提交者到对应的游戏逻辑主体服务器进行处理，适用于需要在游戏逻辑服务器进行提交验证的消息
     * @param _committer
     * @param _cid
     * @param _playerGroupId 玩家归属主体ID（根据活动配置数据区分，GuildId/TeamId）
     * @param _msg
     * @param _addInfo
     * @param _failCallback
     */
    public void dealMsgByRedirectCommiter(_ATNPUserMsgRedirectCommiter _committer,
                                          long _cid, long _playerGroupId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo,
                                          _ICallBackIntT<_ANPUSUserBasicMsgItem> _failCallback)
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_020_DealMsg(_m_activity, _cid, _playerGroupId, _msg, _addInfo),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_020_DealMsg();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        _committer.commitSucResByBuffer(((NP2GLS_RB_001_020_DealMsg) _retMsg).get_buffer_Msg());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        if (null != _failCallback)
                            _failCallback.onRunOver(_errCode, _committer);
                        else
                            _committer.commitFailRes(_errCode);
                    }
                });
    }

    /**
     * 通知 GLS 同步指定 group 的队伍成员数据
     * @param _groupId 群组ID
     * @param _teamId  队伍ID
     */
    public void syncGroupTeamData(long _groupId, long _teamId)
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(
                NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_009_SyncGroupTeamData(_m_activity, _groupId, _teamId),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_009_SyncGroupTeamData();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg) {}

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("ActivityGameLogicDealer.syncGroupTeamData failed: groupId={}, teamId={}, errCode={}",
                                _groupId, _teamId, _errCode);
                    }
                });
    }

    /**
     * 通知 GLS 移除指定 group，队伍解散时调用
     * @param _groupId 群组ID
     */
    public void removeGroupByTeam(long _groupId)
    {
        int glsServerId = CommonFunc.parseGameLogicServerId(_m_activity.getGameLogicInstanceId());
        getActivity().getUSServer().sendRequestToBSServer(
                NPEnum.EServerType.GAME_LOGIC.ordinal(), glsServerId,
                NP2GLS_RB_Writer_001_BasicOp.make_010_RemoveGroupByTeam(_m_activity, _groupId),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_010_RemoveGroupByTeam();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg) {}

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("ActivityGameLogicDealer.removeGroupByTeam failed: groupId={}, errCode={}",
                                _groupId, _errCode);
                    }
                });
    }
}
