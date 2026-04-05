package NPCommonServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommonServer.GMCommand.CSCmdBase;
import NPCommonServer.NPCommonServer;
import NPCommonServer.PlayerFreezeMgr.PlayerFreezeMgr;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
import WCGCommon.Enum.NPEnum.EServerType;

@ACommander(comment = "冻结用户命令", name = "freeze")
public class CmdFreeze extends CSCmdBase
{

    @ACommand(comment = "封禁玩家 [uid] [封禁时长 s]")
    public String FRZUser(String _uid, long _freezeTimeSec)
    {
        PlayerFreezeMgr.getInstance().freeze(_uid, _freezeTimeSec * 1000L);

        //广播到US用户被封禁
        NPCommonServer.getInstance().broadcastMessage(EServerType.USER.ordinal(), NP2US_B_Writer_001_BasicOp.make_002_PlayerFreeze(_uid));

        return "ok";
    }

    @ACommand(comment = "查询玩家封禁信息")
    public String FRZInfo()
    {
        return PlayerFreezeMgr.getInstance().toString();
    }

}
