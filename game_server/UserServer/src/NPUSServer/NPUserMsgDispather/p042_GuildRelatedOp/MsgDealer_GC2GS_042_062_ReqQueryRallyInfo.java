package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import GC2GS.p042_GuildRelatedOp.GC2GS_042_062_ReqQueryRallyInfo;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 042-062 请求查询单个集结信息
 *
 * 说明：
 * 1. 任意联盟成员都可查询
 * 2. 当前阶段只接协议查询链路，后续再补更多校验
 */
public class MsgDealer_GC2GS_042_062_ReqQueryRallyInfo extends NPUserMsgDealer<GC2GS_042_062_ReqQueryRallyInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_062_ReqQueryRallyInfo _msg)
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

        getUSServer().dealGuildMsg(
                _commiter,
                userData.getCid(),
                guildId,
                _msg,
                null
        );
    }
}



