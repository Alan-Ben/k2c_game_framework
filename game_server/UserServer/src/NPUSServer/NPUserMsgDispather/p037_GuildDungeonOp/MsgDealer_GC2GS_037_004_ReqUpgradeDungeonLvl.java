package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import GC2GS.p037_GuildDungeonOp.GC2GS_037_004_ReqUpgradeDungeonLvl;
import GS2GC.p037_GuildDungeonOp.GS2GC_037_004_RetUpgradeDungeonLvl;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import USLOGDB.OptBo.Opt037004GuildDungeonUpgradeLvlBO;

public class MsgDealer_GC2GS_037_004_ReqUpgradeDungeonLvl extends NPUserMsgDealer<GC2GS_037_004_ReqUpgradeDungeonLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_004_ReqUpgradeDungeonLvl _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //此时转发消息开启
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GS2GC_037_004_RetUpgradeDungeonLvl>(_commiter) {
                    @Override
                    protected GS2GC_037_004_RetUpgradeDungeonLvl _createNewTmpObj() {
                        return new GS2GC_037_004_RetUpgradeDungeonLvl();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GS2GC_037_004_RetUpgradeDungeonLvl _retMsg) {
                        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_UPGRADE_LVL);

                        //日志数据
                        Opt037004GuildDungeonUpgradeLvlBO optBo = new Opt037004GuildDungeonUpgradeLvlBO();
                        optBo.setGuildId(getUSServer().getBM(), userData.getGuildComponent().getGuildId());
                        optBo.setDungeonId(getUSServer().getBM(), _msg.getDungeonId());
                        optBo.setPreLvl(getUSServer().getBM(), _msg.getCurLvl());
                        optBo.setCurLvl(getUSServer().getBM(), _retMsg.getUpgradeLvl());
                        userData.logEvent(optBo, context);

                        //返回操作结果
                        _commiter.commitSucRes(_retMsg);
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
