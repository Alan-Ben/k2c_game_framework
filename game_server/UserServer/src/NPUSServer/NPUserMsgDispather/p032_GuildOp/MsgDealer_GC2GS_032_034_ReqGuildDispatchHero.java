package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_034_ReqGuildDispatchHero;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_005_HeroDispatch;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.HeroErr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_034_ReqGuildDispatchHero extends NPUserMsgDealer<GC2GS_032_034_ReqGuildDispatchHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_034_ReqGuildDispatchHero _msg)
    {
        //检查玩家是否已经加入联盟
        if (_committer.getUserData().getGuildComponent().getGuildId() <= 0)
        {
            _committer.commitFailRes(GuildErr.MEMBER_NOT_FOUND.getCode());
            return;
        }

        //查询大臣
        HeroInfo heroInfo = _committer.getUserData().getHeroComponent().lookupHero(_msg.getHeroId());
        if(null == heroInfo)
        {
            _committer.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        GuildOp_005_HeroDispatch addInfo = new GuildOp_005_HeroDispatch();
        addInfo.setHeroId(_msg.getHeroId());
        addInfo.setDispatchValue(heroInfo.getBusinessSkillMgr().calGuildDispatchAddValue());
        addInfo.setLevel(heroInfo.getLevel());
        addInfo.setPower(heroInfo.getPower());
        addInfo.setSkinId(heroInfo.getSkinId());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
