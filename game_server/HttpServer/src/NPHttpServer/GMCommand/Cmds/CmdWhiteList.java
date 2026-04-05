package NPHttpServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPHttpServer.NPWhiteAccMgr.NPWhiteAccMgr;

@ACommander(comment = "白名单相关命令", name = "whiteList")
public class CmdWhiteList extends CmdClassBase
{
    @ACommand(comment = "增加白名单")
    public String addWhiteAcc(String _acc)
    {
        boolean isSuc = NPWhiteAccMgr.getInstance().addAcc(_acc);
        if (isSuc)
        {
            return "suc";
        } else
        {
            return "fail";
        }
    }

    @ACommand(comment = "移除白名单")
    public String removeWhiteAcc(String _acc)
    {
        boolean isSuc = NPWhiteAccMgr.getInstance().removeAcc(_acc);
        if (isSuc)
        {
            return "suc";
        } else
        {
            return "fail";
        }
    }
}
