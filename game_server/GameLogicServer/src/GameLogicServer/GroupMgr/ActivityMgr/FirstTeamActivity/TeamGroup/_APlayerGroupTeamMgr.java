package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.TeamGroup;

import ALBasicProtocolPack._IALProtocolStructure;
import AllRpcData.CrossTeam_Service.Team.CTSGetGLSTeamData;
import Common.ServerObj.ServerObj_GLSTeamMember;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._APlayerGroupPlayerInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupMgr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResultT;
import RPC._ARpcCallBack;

import java.util.ArrayList;

/**
 * 队伍型归属管理器抽象基类
 *
 * 主要功能：
 * 1. 统一封装跨服队伍数据拉取流程
 * 2. 委托子类完成本地队伍对象构建
 *
 * @param <P> 玩家信息类型
 * @param <T> 队伍信息类型
 */
public abstract class _APlayerGroupTeamMgr<P extends _APlayerGroupPlayerInfo, T extends _APlayerGroupTeamInfo<P>> extends _ATGroupMgr<P, T>
{
    /**
     * 构造队伍管理器
     * @param _activity 所属活动对象
     */
    public _APlayerGroupTeamMgr(_ATActivityInfo<P, T> _activity)
    {
        super(_activity);
    }

    /**
     * 构建指定队伍数据
     * @param _groupId 队伍ID
     * @param _callback 构建结果回调
     */
    @Override
    protected void buildGroup(long _groupId, _ICallBackResultT<T> _callback)
    {
        CTSGetGLSTeamData rpc = new CTSGetGLSTeamData();
        rpc.req().setTeamId(_groupId);

        GameLogicServer.getInstance().rpc2cts().request(rpc, new _ARpcCallBack<CTSGetGLSTeamData>()
        {
            @Override
            public void call_back(int _errCode, CTSGetGLSTeamData _rpc)
            {
                if (_errCode > 0)
                {
                    // RPC失败时透传错误码
                    if (null != _callback)
                        _callback.onRunOver(Result.failed(_errCode), null);
                    return;
                }

                T teamInfo = _buildPlayerGroup(_groupId, _rpc.retObj().getTeamData());
                if (null == teamInfo)
                {
                    // 远端成功但本地构建失败，统一返回对象错误
                    if (null != _callback)
                        _callback.onRunOver(CommErr.OBJ_ERR, null);
                    return;
                }

                // 本地对象构建成功，回调成功结果
                if (null != _callback)
                    _callback.onRunOver(Result.SUCC, teamInfo);
            }
        });
    }

    /**
     * 同步指定 group 的队伍成员数据：向 CTS 拉取最新成员列表后更新本地 group
     * @param _groupId 群组ID
     * @param _teamId  队伍ID
     */
    @Override
    public void syncGroup(long _groupId, long _teamId)
    {
        // 先确保 group 存在
        ensureGroup(_groupId, (_result, _group) ->
        {
            if (!_result.isSucc() || null == _group)
            {
                CommLog.error("_APlayerGroupTeamMgr.syncGroup ensureGroup failed: groupId={}", _groupId);
                return;
            }

            // 向 CTS 拉取最新成员列表
            CTSGetGLSTeamData rpc = new CTSGetGLSTeamData();
            rpc.req().setTeamId(_teamId);

            GameLogicServer.getInstance().rpc2cts().request(rpc, new _ARpcCallBack<CTSGetGLSTeamData>()
            {
                @Override
                public void call_back(int _errCode, CTSGetGLSTeamData _rpc)
                {
                    if (_errCode > 0)
                    {
                        CommLog.error("_APlayerGroupTeamMgr.syncGroup CTSGetGLSTeamData failed: groupId={}, teamId={}, errCode={}",
                                _groupId, _teamId, _errCode);
                        return;
                    }

                    // 转换为 cid 列表，更新 group（内部同步清理不再存在的玩家主体）
                    ArrayList<Long> memberCidList = new ArrayList<>();
                    for (ServerObj_GLSTeamMember m : _rpc.retObj().getTeamData().getMemberList())
                        memberCidList.add(m.getCid());

                    _group.setMemberCidList(memberCidList);
                }
            });
        });
    }

    /**
     * 移除指定 group 及其所有玩家主体数据（队伍解散时调用）
     * @param _groupId 群组ID
     */
    @Override
    public void removeGroup(long _groupId)
    {
        T group = lookup(_groupId);
        if (null == group)
            return;

        // 清空成员列表（同时清理本地玩家主体缓存）
        group.setMemberCidList(new ArrayList<>());
        // 从数据集中移除
        _removeGroup(_groupId);
    }

    /**
     * 由子类将远端结构转换为本地队伍对象
     * @param _groupId 队伍ID
     * @param _obj 远端协议结构
     * @return 本地队伍对象
     */
    protected abstract T _buildPlayerGroup(long _groupId, _IALProtocolStructure _obj);
}
