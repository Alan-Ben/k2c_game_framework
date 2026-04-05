package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_037_ReqDrawConstructReward;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetIntInfo;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取建造宝箱奖励
 */
public class RequestDealer_NP2US_R_032_037_ReqDrawConstructReward extends _ATRequestDealer_GuildOp<GC2GS_032_037_ReqDrawConstructReward>
{
    public RequestDealer_NP2US_R_032_037_ReqDrawConstructReward(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_037_ReqDrawConstructReward _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //领取活跃宝箱
        Result result = guildInfo.checkConstructReward(_msg.getNum());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //提交结果，由US处理奖励下发
        _committer.commitSucRes(new GuildOp_RetIntInfo(_msg.getNum()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}