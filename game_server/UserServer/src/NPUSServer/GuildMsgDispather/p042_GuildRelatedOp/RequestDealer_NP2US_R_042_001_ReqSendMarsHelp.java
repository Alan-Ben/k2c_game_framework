package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_041_001_AddMarsHelp;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_001_ReqSendMarsHelp;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_042_001_ReqSendMarsHelp extends _ATRequestDealer_GuildOp<GC2GS_042_001_ReqSendMarsHelp>
{
    public RequestDealer_NP2US_R_042_001_ReqSendMarsHelp(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_001_ReqSendMarsHelp _msg)
    {
        if(null == _committer.getGuildInfo())
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //结构附加信息
        GuildOp_041_001_AddMarsHelp addInfo = new GuildOp_041_001_AddMarsHelp();
        addInfo.readPackage(_committer.getAddInfo());

        //构造求助数据
        GuildMarsHelpInfo helpInfo = _committer.getGuildInfo().getMarsHelpMgr().cmdAddHelp(
                _committer.getCid(), _msg.getObjType(), addInfo);

        if(null == helpInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_MARS_HELP_SEND_FAIL.getCode());
            return;
        }


        //联盟可以自动帮助的盟友进行处理
        helpInfo.dealAutoHelo();

        //自动求助完成后，如果求助数据达到上限，则不再推送
        if(helpInfo.isDeled())
        {

        }
        else
        {
            //推送公会其他玩家
            _committer.getGuildInfo().getMarsHelpMgr().pushMarsHelpAdd(helpInfo);
        }

        GuildOp_RetLongInfo retInfo = new GuildOp_RetLongInfo();
        retInfo.setNum(helpInfo.getId());

        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
