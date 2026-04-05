package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.RankObj.Rank_BaseItem;
import GC2GS.p032_GuildOp.GC2GS_032_046_ReqGuildCooperateDamageRank;
import NPUSServer.Guild.Cooperate.GuildCooperateDamageRankList;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

import java.util.List;

/**
 * 联盟协作伤害排行榜查询 协议处理类
 *
 * 主要功能：
 * 1. 处理客户端的伤害排行榜查询请求
 * 2. 返回完整的公会内伤害排行榜数据
 * 3. 包含个人排名信息
 *
 * 执行流程：
 * 1. 验证玩家是否在公会中
 * 2. 获取公会协作信息
 * 3. 查询完整排行榜数据
 * 4. 查询个人排名
 * 5. 构造并返回响应协议
 */
public class RequestDealer_NP2US_R_032_046_ReqGuildCooperateDamageRank extends _ATRequestDealer_GuildOp<GC2GS_032_046_ReqGuildCooperateDamageRank>
{
    public RequestDealer_NP2US_R_032_046_ReqGuildCooperateDamageRank(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_046_ReqGuildCooperateDamageRank _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        GuildCooperateDamageRankList damageRankList = guildInfo.getCooperateInfo().getDamageRankList();

        // 获取完整排行榜数据
        List<Rank_BaseItem> rankList = damageRankList.makeAllRankProtoList();

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_046_RetGuildCooperateDamageRank(rankList));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}