package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_046_ReqGuildCooperateDamageRank;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

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
public class MsgDealer_GC2GS_032_046_ReqGuildCooperateDamageRank extends NPUserMsgDealer<GC2GS_032_046_ReqGuildCooperateDamageRank>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_032_046_ReqGuildCooperateDamageRank _msg)
    {
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}