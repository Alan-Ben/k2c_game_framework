package MGClient.Cmd.Cmds;

import GC2GS.p022_ChatOp.GC2GS_022_001_ReqPlayerChatLogin;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "聊天", name = "chat")
public class CmdChat extends CmdBase
{
    @Command(comment = "注册聊天")
    public void regChat()
    {
        GC2GS_022_001_ReqPlayerChatLogin proto = new GC2GS_022_001_ReqPlayerChatLogin();
        getOwner().sendGameMsg(proto);
    }


}
