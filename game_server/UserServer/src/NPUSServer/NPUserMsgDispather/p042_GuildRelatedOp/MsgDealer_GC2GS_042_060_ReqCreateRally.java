package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import Common.MarsObj.MarsBattleV2_MemberInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_060_ReqCreateRally;
import GS2GC.p042_GuildRelatedOp.GS2GC_042_060_RetCreateRally;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 042-060 请求创建集结
 *
 * 说明：
 * 1. 当前阶段用于协议接线，完整业务流程在后续步骤补齐
 * 2. 统一返回RetCreateRally，不走通用错误协议
 */
public class MsgDealer_GC2GS_042_060_ReqCreateRally extends NPUserMsgDealer<GC2GS_042_060_ReqCreateRally>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_060_ReqCreateRally _msg)
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
        ResultOne<Long> transStateResult = team.startRally(context);
        if (!transStateResult.isSucc()) {
            _commiter.commitFailRes(transStateResult.getCode());
            return;
        }

        //获取切换的状态
        long sateSerialize = transStateResult.getData();

        //构造界面数据传输
        MarsBattleV2_MemberInfo addInfo = team.makeBattleV2_MemberInfo();

        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GS2GC_042_060_RetCreateRally>(_commiter) {
                    @Override
                    protected GS2GC_042_060_RetCreateRally _createNewTmpObj()
                    {
                        return new GS2GC_042_060_RetCreateRally();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _msgItem, GS2GC_042_060_RetCreateRally _retMsg)
                    {
                        // 创建成功后把rallyId写入当前WAIT_RALLY状态，确保状态与集结绑定
                        Result bindResult = team.bindWaitRallyId(sateSerialize, _retMsg.getRallyId());
                        if (!bindResult.isSucc()) {
                            team.resetIdleState(sateSerialize, context);
                            _msgItem.commitFailRes(bindResult.getCode());
                            return;
                        }

                        _msgItem.commitSucRes(_retMsg);
                    }
                },
                userData.getCid(),
                guildId,
                _msg,
                addInfo,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _errCode, _ANPUSUserBasicMsgItem _msgItem)
                    {
                        // Guild创建失败，回滚队伍状态到空闲
                        team.resetIdleState(sateSerialize, context);

                        _msgItem.commitFailRes(_errCode);
                    }
                }
        );
    }
}



