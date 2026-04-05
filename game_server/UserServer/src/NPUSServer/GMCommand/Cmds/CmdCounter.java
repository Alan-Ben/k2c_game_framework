package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "玩家计数相关命令", name = "counter")
public class CmdCounter extends UsCmdBase
{
    @ACommand(comment = "设置玩家计数")
    public String set(long _cid, EPlayerCounterEnum _counterType, long _count)
    {
        getUserServer().getUserCounterMgr().setPlayerCounter(_cid, _counterType, _count);

        return "ok";
    }
}
