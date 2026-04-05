package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * 限时兑换GM命令
 */
@ACommander(comment = "限时兑换", name = "rushExchange")
public class CmdRushExchange extends UsCmdBase
{
    @ACommand(comment = "设置兑换完成可以马上领奖")
    public String setExchangeDone()
    {
        Result result = getOwner().getRushExchangeComponent().gmSetExchangeDone();
        if (!result.isSucc())
            return result.toString();

        return "ok";
    }

    @ACommand(comment = "直接发起兑换（不消耗道具）")
    public String exchangeNoCost()
    {
        Result result = getOwner().getRushExchangeComponent().gmExchangeNoCost(getContext());
        if (!result.isSucc())
            return result.toString();

        return "ok";
    }

    @ACommand(comment = "清除当天兑换次数")
    public String clearTodayCount()
    {
        getOwner().getRushExchangeComponent().gmClearTodayCount();
        return "ok";
    }

    @ACommand(comment = "直接刷新兑换信息")
    public String refresh()
    {
        boolean isSucc = getOwner().getRushExchangeComponent().doRefresh(CommonFunc.getNowTimeMS());
        if (!isSucc)
            return "refresh fail";

        return "ok";
    }
}