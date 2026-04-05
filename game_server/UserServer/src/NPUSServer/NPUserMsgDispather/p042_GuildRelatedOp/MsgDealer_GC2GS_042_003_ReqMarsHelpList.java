package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongListInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_003_ReqMarsHelpList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_042_003_ReqMarsHelpList extends NPUserMsgDealer<GC2GS_042_003_ReqMarsHelpList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_003_ReqMarsHelpList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildOp_RetLongListInfo addInfo = new GuildOp_RetLongListInfo();
        userData.getMarsComponent().getGuildMarsHelpMgr().getHelpIdList(addInfo.getList());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
