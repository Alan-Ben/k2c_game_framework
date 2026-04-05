package NPUSServer.GMCommand.Cmds;


import CommonEnum.ECurrency;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPPlayerParam;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "快速升级", name = "level")
public class CmdLevel extends UsCmdBase
{
    @ACommand(comment = "增加额外赚速[产速]")
    public String addExtraEarnings(long _value)
    {
        getOwner().getPlayerComponent().addExtraEarnings(_value);
        return "ok";
    }

    @ACommand(comment = "增加经验[值]")
    public String addExp(long _value)
    {
        getOwner().getCurrencyComponent().gainItem(ECurrency.P_EXP.ordinal(), _value, false, getContext());
        return "ok";
    }

    @ACommand(comment = "执行升级检查")
    public String checkLevelUp()
    {
        Result result = getOwner().getPlayerComponent().checkLevelUp((int) getOwner().getPlayerComponent().getParamV(ENPPlayerParam.LEVEL), false, getContext());
        if (!result.isSucc())
            return result.toString();

        return "ok";
    }

}
