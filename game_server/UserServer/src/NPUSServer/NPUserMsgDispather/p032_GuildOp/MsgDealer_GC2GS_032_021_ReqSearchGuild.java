package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import Common.GuildObj.Guild_ShowInfo;
import Common.RankObj.Rank_BaseItem;
import GC2GS.p032_GuildOp.GC2GS_032_021_ReqSearchGuild;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;

public class MsgDealer_GC2GS_032_021_ReqSearchGuild extends NPUserMsgDealer<GC2GS_032_021_ReqSearchGuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_021_ReqSearchGuild _msg)
    {
        GuildInfo guildInfo = null;

        String searchData = _msg.getSearchData();

        // 如果传入的数据是全数字，先根据公会id查找公会
        if (searchData.matches("[0-9]+"))
        {
            guildInfo = getUSServer().getGuildMgr().lookupGuild(Long.parseLong(searchData));
        }

        //如果还是查不到 则检索名字
        if (guildInfo == null)
        {
            //根据字符串长度区分处理
            if (RefGeneral.Ref().guild_simple_name_length_limit.inRange(searchData.length()))
            {
                guildInfo = getUSServer().getGuildMgr().lookupBySimpleName(searchData);
            } else if (RefGeneral.Ref().guild_name_length_limit.inRange(searchData.length()))
            {
                guildInfo = getUSServer().getGuildMgr().lookupByName(searchData);
            }
        }

        //如果没有找到公会
        if (guildInfo == null)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        Guild_ShowInfo guildShowInfo = guildInfo.makeShowInfo();

        //获取公会排行信息
        RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().guild_rank_fixed_id);
        if (rankFixedInfo != null)
        {
            rankFixedInfo.makeRankBaseByKey(guildInfo.getGuildId(), false, new _ICallBackResultT<Rank_BaseItem>()
            {
                @Override
                public void onRunOver(Result _result, Rank_BaseItem _rankBaseInfo)
                {
                    _committer.commitSucRes(US2GCWriter_032_GuildOp.make_021_RetSearchGuild(guildShowInfo, _result.isSucc() ? _rankBaseInfo : null));
                }
            });
        } else
        {
            _committer.commitSucRes(US2GCWriter_032_GuildOp.make_021_RetSearchGuild(guildShowInfo, null));
        }
    }
}
