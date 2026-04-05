package NPUSServer.CommonActivityMgr.Core.Team;

import AllRpcData.CrossTeam_Service.Team.*;
import Common.CrossTeamObj.*;
import Common.ServerObj.ServerObj_ActivityTeamUser;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPGameRes.Refs.Activity.RefActivityTeam;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.TeamActivityMgr.TeamActivityInfo;
import NPUSServer.UsActivityScheduleMgr.UsActivityScheduleInfo;
import RPC._ARpcCallBack;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;

/**
 * 活动组队方法
 */
public class ActivityTeamDealer
{
    //活动对象
    private _AActivityBase _m_activity;
    //配置数据
    private RefActivityTeam _m_refActivityTeam;

    public ActivityTeamDealer(_AActivityBase _activity, RefActivityTeam _ref)
    {
        _m_activity = _activity;
        _m_refActivityTeam = _ref;
    }

    public _AActivityBase getActivity() {return _m_activity;}
    public RefActivityTeam getRef() {return _m_refActivityTeam;}

    public long getInstanceId() {return _m_activity.getInstanceId();}
    public long getActivityId() {return _m_activity.getActivityId();}

    public NPUserServer getUSServer() {return _m_activity.getUSServer();}
    public UsActivityScheduleInfo getSchedule() {return _m_activity.getSchedule();}

    public void _onDiscard()
    {
        //移除队伍与活动的映射关系
        getUSServer().getTeamActivityMgr().removeByActivity(getInstanceId());

        //向CTS请求退出队伍，失败重试10次
        CTSQuitTeamGroup rpc = new CTSQuitTeamGroup();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setUsId(getUSServer().getServerTypeId());

        getUSServer().rpc2crossTeam().requestRepeat(rpc, new _ARpcCallBack<CTSQuitTeamGroup>()
        {
            @Override
            public void call_back(int _errCode, CTSQuitTeamGroup _rpc)
            {
                if(_errCode == 0)
                {
                    CommLog.info("TeamActivity QuitTeamGroup Success! id:{} activity:{} groupId:{}",
                            getInstanceId(), getActivityId(), getSchedule().getUsGroupId());
                }
            }
        }, 10, ()->
        {
            CommLog.warn("TeamActivity QuitTeamGroup Failed! id:{} activity:{} groupId:{}",
                    getInstanceId(), getActivityId(), getSchedule().getUsGroupId());
        });
    }

    /**
     * US启动时向跨服队伍服务器注册自身：将当前US ID加入分组管理，失败自动重试，最多10次
     */
    public void _regUs()
    {
        CTSRegUsGroup rpc = new CTSRegUsGroup();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setUsId(getUSServer().getServerTypeId());

        getUSServer().rpc2crossTeam().requestRepeat(rpc, new _ARpcCallBack<CTSRegUsGroup>()
        {
            @Override
            public void call_back(int _errCode, CTSRegUsGroup _rpc)
            {
                // 成功无需处理
            }
        }, 10, () ->
        {
            CommLog.error("ActivityTeamDealer._regUs - 注册失败: instanceId={}, activityId={}, groupId={}",
                    getInstanceId(), getActivityId(), getSchedule().getUsGroupId());
        });
    }

    /**
     * 获取队伍成员上限
     * @return
     */
    public int getMemberLimit()
    {
        return _m_refActivityTeam.member_limit;
    }

    /**
     * 获取队伍信息
     * @param _teamId
     * @param _callback
     */
    public void getTeam(long _teamId, _ICallBackResultT<CrossTeam_Info> _callback)
    {
        CTSGetTeam rpc = new CTSGetTeam();
        rpc.req().setTeamId(_teamId);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSGetTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getTeam());
            }
        });
    }

    /**
     * 获取玩家所在队伍信息
     * @param _cid
     * @param _callback
     */
    public void getPlayerTeam(long _cid, _ICallBackResultT<CrossTeam_Info> _callback)
    {
        CTSGetPlayerTeam rpc = new CTSGetPlayerTeam();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setCid(_cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetPlayerTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSGetPlayerTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getTeam());
            }
        });
    }

    /**
     * 获取玩家所在队伍ID
     * @param _cid
     * @param _callback
     */
    public void getPlayerTeamBase(long _cid, _ICallBackResultT<CrossTeam_BaseInfo> _callback)
    {
        CTSGetPlayerTeamBase rpc = new CTSGetPlayerTeamBase();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setCid(_cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetPlayerTeamBase>()
        {
            @Override
            public void call_back(int _errCode, CTSGetPlayerTeamBase _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getTeamBase());
            }
        });
    }

    /**
     * 获取玩家所在队伍ID
     * @param _cid
     * @param _callback
     */
    public void getPlayerTeamId(long _cid, _ICallBackResultT<Long> _callback)
    {
        getPlayerTeamBase(_cid, (_result, _teamBase)->
        {
            if(!_result.isSucc())
            {
                _callback.onRunOver(_result, 0L);
                return;
            }

            _callback.onRunOver(Result.SUCC, _teamBase.getTeamId());
        });
    }

    /**
     * 创建活动队伍
     */
    public void createTeam(long _cid, int _memberLimit,
                           Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType,
                           CrossTeam_SetInfo_Join _joinCond,
                           String _teamName, String _teamDec,
                           _ICallBackResultT<CrossTeam_Info> _callback)
    {
        CTSCreateTeam rpc = new CTSCreateTeam();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setCid(_cid);
        rpc.req().setMemberLimit(_memberLimit);
        rpc.req().setJoinType(_joinType);
        rpc.req().setJoinCond(_joinCond);
        rpc.req().setTeamName(_teamName);
        rpc.req().setTeamDec(_teamDec);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSCreateTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSCreateTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                //更新队伍与活动的映射关系
                TeamActivityInfo teamActivityInfo = new TeamActivityInfo(rpc.retObj().getTeam().getTeamBase().getTeamId(), getInstanceId(), _cid);
                getUSServer().getTeamActivityMgr().addTeamActivityMap(teamActivityInfo);

                // 创建成功后直接通知 GLS 同步 group 成员数据
                long teamId = _rpc.retObj().getTeam().getTeamBase().getTeamId();
                long groupId = getSchedule().getUsGroupId();
                getActivity().getGameLogicDealer().syncGroupTeamData(groupId, teamId);

                //调用处理对象
                _callback.onRunOver(Result.SUCC, _rpc.retObj().getTeam());
            }
        });
    }

    /**
     * 修改队伍设置（名称、宣言、加入方式、申请条件）
     */
    public void setTeamSetting(long _teamId, long _cid,
                               String _teamName, String _teamDec,
                               Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType,
                               CrossTeam_SetInfo_Join _joinCond,
                               _ICallBackResult _callback)
    {
        CTSSetTeamSetting rpc = new CTSSetTeamSetting();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().setTeamName(_teamName);
        rpc.req().setTeamDec(_teamDec);
        rpc.req().setJoinType(_joinType);
        rpc.req().setJoinCond(_joinCond);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSSetTeamSetting>()
        {
            @Override
            public void call_back(int _errCode, CTSSetTeamSetting _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }
                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 更新队伍申请条件
     * @param _teamId
     * @param _cid
     * @param _applyCondValue
     * @param _callback
     */
    public void updateJoinCond(long _teamId, long _cid, CrossTeam_SetInfo_Join _applyCondValue, _ICallBackResult _callback)
    {
        CTSSetTeamApplyCond rpc = new CTSSetTeamApplyCond();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().setCondValue(_applyCondValue);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSSetTeamApplyCond>()
        {
            @Override
            public void call_back(int _errCode, CTSSetTeamApplyCond _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 加入活动队伍
     * @param _teamUser
     * @param _teamId
     * @param _joinCondList 玩家入队条件列表
     * @param _callback
     */
    public void joinTeam(ServerObj_ActivityTeamUser _teamUser, long _teamId, ArrayList<CrossTeam_SetInfo_Join> _joinCondList, _ICallBackResultT<CrossTeam_Info> _callback)
    {
        CTSJoinTeam rpc = new CTSJoinTeam();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_teamUser.getCid());
        rpc.req().getJoinCondList().addAll(_joinCondList);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSJoinTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSJoinTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getTeam());
            }
        });
    }

    /**
     * 退出活动队伍
     * @param _teamId
     * @param _cid
     * @param _callback
     */
    public void quitTeam(long _teamId, long _cid, _ICallBackResult _callback)
    {
        CTSQuitTeam rpc = new CTSQuitTeam();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSQuitTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSQuitTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 踢出活动队伍成员
     * @param _teamId
     * @param _cid
     * @param _kickCid
     * @param _callback
     */
    public void kickActivityTeamPlayer(long _teamId, long _cid, long _kickCid, _ICallBackResult _callback)
    {
        CTSKickTeamPlayer rpc = new CTSKickTeamPlayer();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().setKickCid(_kickCid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSKickTeamPlayer>()
        {
            @Override
            public void call_back(int _errCode, CTSKickTeamPlayer _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 解散活动队伍
     * @param _teamId
     * @param _cid
     * @param _callback
     */
    public void dissolveTeam(long _teamId, long _cid, _ICallBackResult _callback)
    {
        CTSDissolveTeam rpc = new CTSDissolveTeam();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSDissolveTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSDissolveTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                //更新队伍与活动的映射关系
                getUSServer().getTeamActivityMgr().removeByTeam(_teamId);

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 获取指定玩家所在活动队伍的聊天房间ID
     * @param _cid
     * @param _callback
     */
    public void getChatRoomId(long _cid, _IGetChatRoomCallBackResult _callback)
    {
        CTSGetPlayerTeamRoom rpc = new CTSGetPlayerTeamRoom();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setCid(_cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetPlayerTeamRoom>()
        {
            @Override
            public void call_back(int _errCode, CTSGetPlayerTeamRoom _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), 0L, 0, 0);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getRoomId(), NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.CROSS_TEAM.ordinal());
            }
        });
    }

    /**
     * 申请加入活动队伍
     * @param _teamId
     * @param _cid
     * @param _joinCondList
     * @param _callback
     */
    public void applyJoinTeam(long _teamId, long _cid, ArrayList<CrossTeam_SetInfo_Join> _joinCondList, _ICallBackResult _callback)
    {
        CTSApplyJoinTeam rpc = new CTSApplyJoinTeam();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().getJoinCondList().addAll(_joinCondList);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSApplyJoinTeam>()
        {
            @Override
            public void call_back(int _errCode, CTSApplyJoinTeam _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 获取指定队伍的申请列表
     * @param teamId
     * @param callback
     */
    public void getTeamApplyList(long teamId, _ICallBackResultT<ArrayList<CrossTeam_ApplyInfo>> callback)
    {
        CTSGetTeamApplyList rpc = new CTSGetTeamApplyList();
        rpc.req().setTeamId(teamId);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetTeamApplyList>()
        {
            @Override
            public void call_back(int _errCode, CTSGetTeamApplyList _rpc)
            {
                if(_errCode > 0)
                {
                    callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                callback.onRunOver(Result.SUCC, _rpc.retObj().getApplyList());
            }
        });
    }

    /**
     * 获取指定玩家当前分组下的所有队伍申请
     * @param cid
     * @param callback
     */
    public void getPlayerTeamApplyList(long cid, _ICallBackResultT<ArrayList<CrossTeam_PlayerApplyInfo>> callback)
    {
        CTSGetTeamPlayerApplyList rpc = new CTSGetTeamPlayerApplyList();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setCid(cid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetTeamPlayerApplyList>()
        {
            @Override
            public void call_back(int _errCode, CTSGetTeamPlayerApplyList _rpc)
            {
                if(_errCode > 0)
                {
                    callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                callback.onRunOver(Result.SUCC, _rpc.retObj().getApplyList());
            }
        });
    }

    /**
     * 同意队伍申请
     * @param _teamId
     * @param _cid
     * @param _applyCid
     * @param _callback
     */
    public void agreeTeamApply(long _teamId, long _cid, long _applyCid, _ICallBackResult _callback)
    {
        CTSAgreeTeamApply rpc = new CTSAgreeTeamApply();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().setApplyCid(_applyCid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSAgreeTeamApply>()
        {
            @Override
            public void call_back(int _errCode, CTSAgreeTeamApply _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 拒绝队伍申请
     * @param _teamId
     * @param _cid
     * @param _applyCid
     * @param _callback
     */
    public void refuseTeamApply(long _teamId, long _cid, long _applyCid, _ICallBackResult _callback)
    {
        CTSRefuseTeamApply rpc = new CTSRefuseTeamApply();
        rpc.req().setTeamId(_teamId);
        rpc.req().setCid(_cid);
        rpc.req().setApplyCid(_applyCid);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSRefuseTeamApply>()
        {
            @Override
            public void call_back(int _errCode, CTSRefuseTeamApply _rpc)
            {
                if (_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            }
        });
    }

    /**
     * 获取指定分组的分页队伍列表
     * @param _page     页码（从1开始）
     * @param _callback 回调，返回队伍基础数据列表和总数量
     */
    public void getGroupTeamList(int _page, _ICallBackResultT<CTSGetGroupTeamList> _callback)
    {
        CTSGetGroupTeamList rpc = new CTSGetGroupTeamList();
        rpc.req().setGroupId(getSchedule().getUsGroupId());
        rpc.req().setPage(_page);

        getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<CTSGetGroupTeamList>()
        {
            @Override
            public void call_back(int _errCode, CTSGetGroupTeamList _rpc)
            {
                if (_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc);
            }
        });
    }
}
