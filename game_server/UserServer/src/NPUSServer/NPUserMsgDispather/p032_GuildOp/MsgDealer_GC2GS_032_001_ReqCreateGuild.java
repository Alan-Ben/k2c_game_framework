package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPositionType;
import GC2GS.p032_GuildOp.GC2GS_032_001_ReqCreateGuild;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_032_001_ReqCreateGuild extends NPUserMsgDealer<GC2GS_032_001_ReqCreateGuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_001_ReqCreateGuild _msg)
    {
        //检查玩家是否已经加入联盟
        if (_committer.getUserData().getGuildComponent().getGuildId() > 0)
        {
            _committer.commitFailRes(GuildErr.PLAYER_ALREADY_IN_GUILD.getCode());
            return;
        }

        //记录名称
        Result nameRecordResult = getUSServer().getGuildMgr().getNameChecker().recordName(_msg.getName(),_msg.getSimpleName());
        if (!nameRecordResult.isSucc())
        {
            _committer.commitFailRes(nameRecordResult.getCode());
            return;
        }

        //检查道具是否足够
        if (!_committer.getUserData().hasCostItemList(RefGeneral.Ref().guild_create_cost))
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //消耗道具
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CREATE_GUILD);
        if (!_committer.getUserData().spendCostItemList(RefGeneral.Ref().guild_create_cost, context))
        {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //创建联盟
        ResultOne<GuildInfo> result = getUSServer().getGuildMgr().createGuild(
                _msg.getFlagId(),
                _msg.getName(),
                _msg.getSimpleName(),
                _msg.getDeclaration(),
                _msg.getCanFreeJoin());

        if (!result.isSucc())
        {
            //创建失败移除名称占用
            getUSServer().getGuildMgr().getNameChecker().removeRecord(_msg.getName(),_msg.getSimpleName());
            //打印错误日志
            USLog.error(getUSServer(), "MsgDealer_GC2GS_032_001_ReqCreateGuild createGuild fail. cid:{} code:{}",
                    _committer.getUserData().getCid(), result.getCode());

            _committer.commitFailRes(result.getCode());
            return;
        }

        //加入联盟
        Result joinResult = getUSServer().getGuildMgr().joinGuild(result.getData(), _committer.getUserData().getCid(), EGuildPositionType.LEADER, true, false);
        if (!joinResult.isSucc())
        {
            USLog.error(getUSServer(), "MsgDealer_GC2GS_032_001_ReqCreateGuild joinGuild fail. cid:{} guildId:{} code:{}",
                    _committer.getUserData().getCid(), result.getData().getGuildId(), joinResult.getCode());
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_001_RetCreateGuild());
    }
}
