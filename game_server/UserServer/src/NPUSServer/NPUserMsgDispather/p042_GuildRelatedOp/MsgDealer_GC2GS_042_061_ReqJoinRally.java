package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import Common.MarsObj.MarsBattleV2_MemberInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_061_ReqJoinRally;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 042-061 请求加入集结
 *
 * 说明：
 * 1. 当前阶段用于协议接线，完整行军状态与失败回滚流程在后续步骤补齐
 * 2. 成功/失败统一返回RetJoinRally
 */
public class MsgDealer_GC2GS_042_061_ReqJoinRally extends NPUserMsgDealer<GC2GS_042_061_ReqJoinRally>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_061_ReqJoinRally _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null) {
            return;
        }

        // 玩家侧不直接处理Guild对象，统一转发到Guild侧处理
        long guildId = userData.getGuildComponent().getGuildId();
        if (guildId <= 0) {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        // 加入流程先切队伍到行军加入集结状态
        MarsExploreTeam team = userData.getMarsExploreComponent().getTeamMgr().lookup(_msg.getTeamId());
        if (team == null) {
            _commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND.getCode());
            return;
        }

        //尝试加入前往集结的状态
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG);
        ResultOne<Long> transStateResult = team.startJoinRally(_msg.getRallyId(), context);
        if (!transStateResult.isSucc()) {
            _commiter.commitFailRes(transStateResult.getCode());
            return;
        }

        //获取切换的状态
        long sateSerialize = transStateResult.getData();

        //构造界面数据传输
        MarsBattleV2_MemberInfo addInfo = team.makeBattleV2_MemberInfo();

        getUSServer().dealGuildMsg(
                _commiter,
                userData.getCid(),
                guildId,
                _msg,
                addInfo,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _errCode, _ANPUSUserBasicMsgItem _commiter) {
                        // Guild侧失败，回滚队伍状态到空闲
                        team.resetIdleState(sateSerialize, context);

                        _commiter.commitFailRes(_errCode);
                    }
                }
        );
    }
}



