package NPUSServer.UsMars.UsMarsAction.UsJoinRallyAction;

import AllRpcData.US_Service.Mars.MarsRallyJoinFailBack;
import AllRpcData.US_Service.Mars.MarsRallyJoinToWait;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.USLog;
import NPUSServer.UsMars.UsMarsAction.UsMarsActionCore;
import NPUSServer.UsMars.UsMarsAction._AUsMarsBasicAction;
import USDB.Bo.UsMarsJoinRallyBO;

/**
 * 加入集结行军行为
 *
 * 主要职责：
 * 1. 到达触发时通知Guild侧完成成员到达
 * 2. 触发失败或取消时执行Guild侧补偿并回退队伍状态
 * 3. 清理时删除行为BO，避免脏行为残留
 */
public class UsMarsJoinRallyAction extends _AUsMarsBasicAction
{
    // 行为核心对象（提供Guild、用户、BM访问入口）
    private UsMarsActionCore _m_core;
    // 行为持久化BO（记录目标集结、成员、触发时间）
    private UsMarsJoinRallyBO _m_bo;

    /**
     * 构造加入集结行军行为
     */
    public UsMarsJoinRallyAction(UsMarsActionCore _core, UsMarsJoinRallyBO _bo)
    {
        _m_core = _core;
        _m_bo = _bo;
    }

    public long getGuildId() { return _m_bo.getGuildId(); }
    public long getRallyId() { return _m_bo.getRallyId(); }
    public long getCid() { return _m_bo.getCid(); }
    public long getTeamId() { return _m_bo.getTeamId(); }

    @Override
    public long getTriggerTimeMS()
    {
        return _m_bo.getTriggerTimeMs();
    }

    @Override
    protected void _trigger(long _nowTimeMs)
    {
        // 触发时先命中联盟对象；未命中视为失败补偿场景。由于参与集结的action必定在guild所在服务器处理，因此这里直接使用服务器获取公会
        GuildInfo guildInfo = _m_core.getUSServer().getGuildMgr().lookupGuild(getGuildId());
        if (guildInfo == null)
        {
            _dealFail();
            return;
        }

        // 到达后由Guild侧按rallyId处理成员ready状态
        Result result = guildInfo.getRallyMgr().onJoinMarchArrive(getCid(), getRallyId(), getTeamId());
        if (!result.isSucc())
        {
            _dealFail();
            return;
        }

        // 到达成功：队伍切回空闲
        _setTeamWaitRally();
    }

    @Override
    protected void _dealCancel()
    {
        // 取消行为与触发失败走同一补偿逻辑
        _dealFail();
    }

    @Override
    protected void _clearData()
    {
        // 行为生命周期结束后清理持久化数据
        _m_bo.del(_m_core.getBM());
    }

    /**
     * 失败补偿
     * 1. 通知Guild侧移除成员
     * 2. 将队伍状态回退为返航
     */
    private void _dealFail()
    {
        GuildInfo guildInfo = _m_core.getUSServer().getGuildMgr().lookupGuild(getGuildId());
        if (guildInfo != null)
        {
            guildInfo.getRallyMgr().onJoinMarchFail(getCid(), getRallyId(), getTeamId());
        }

        _setTeamSendBack();
    }

    /**
     * 将队伍置为空闲状态（到达成功后）
     */
    private void _setTeamWaitRally()
    {
        // 行为在联盟US触发，这里通过RPC通知玩家所属US切换到WAIT_RALLY状态
        int usId = CommonFunc.parseServerTypeIdFromCid(getCid());

        MarsRallyJoinToWait rpc = new MarsRallyJoinToWait();
        rpc.req().setCid(getCid());
        rpc.req().setTeamId(getTeamId());

        _m_core.getUSServer().rpc2us().requestToRepeat(usId, rpc,
                null,
                99,
                () -> USLog.error(_m_core.getUSServer(),
                        "UsMarsJoinRallyAction._setTeamWaitRally - send rpc fail: cid={}, teamId={}, guildId={}, rallyId={}",
                        getCid(), getTeamId(), getGuildId(), getRallyId()));
    }

    /**
     * 将队伍置为返航状态（失败/取消补偿）
     */
    private void _setTeamSendBack()
    {
        // 行为在联盟US触发，这里通过RPC通知玩家所属US执行失败遣返
        int usId = CommonFunc.parseServerTypeIdFromCid(getCid());

        MarsRallyJoinFailBack rpc = new MarsRallyJoinFailBack();
        rpc.req().setCid(getCid());
        rpc.req().setTeamId(getTeamId());

        _m_core.getUSServer().rpc2us().requestToRepeat(usId, rpc,
                null,
                99,
                () -> USLog.error(_m_core.getUSServer(),
                        "UsMarsJoinRallyAction._setTeamSendBack - send rpc fail: cid={}, teamId={}, guildId={}, rallyId={}",
                        getCid(), getTeamId(), getGuildId(), getRallyId()));
    }
}







