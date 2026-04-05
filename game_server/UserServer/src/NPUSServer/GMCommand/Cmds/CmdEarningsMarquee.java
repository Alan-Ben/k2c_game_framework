package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.EarningsMarqueeMgr.EarningsMarqueeInfo;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "赚速跑马灯", name = "earningsmarquee")
public class CmdEarningsMarquee extends UsCmdBase
{
    @ACommand(comment = "触发赚速跑马灯[产速]")
    public String addExtraEarnings(long _value)
    {
        getUserServer().getEarningsMarqueeMgr().checkAndTriggerMarquee(getOwner().getCid(), getOwner().getPlayerComponent().getName(), _value);
        return "done";
    }

    @ACommand(comment = "清除指定赚速下的所有记录[产速]")
    public String cleanAllRecord(long _value)
    {
        EarningsMarqueeInfo info = getUserServer().getEarningsMarqueeMgr().lookup(_value);
        if (info == null)
            return "not found";

        info.cleanRecords();
        return "ok";
    }

    @ACommand(comment = "清除当前玩家指定赚速下的记录[产速]")
    public String cleanRecord(long _value)
    {
        EarningsMarqueeInfo info = getUserServer().getEarningsMarqueeMgr().lookup(_value);
        if (info == null)
            return "not found";
        info.cleanRecord(getOwner().getCid());
        return "ok";
    }

    @ACommand(comment = "打印赚速跑马灯信息")
    public String info()
    {
        return getUserServer().getEarningsMarqueeMgr().toString();
    }

}
