package NPLoginCheckServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr;
import NPLoginCheckServer.NPSDKLoginMgr.NPSDKLoginMgr;

@ACommander(comment = "SDK相关命令", name = "sdk")
public class CmdSDK extends CmdClassBase
{
    @ACommand(comment = "设置下次检查时间")
    public String chgNextCheckTimeMs(long _timeMs)
    {
        NPSDKLoginMgr.getInstance().chgNextCheckTimeMs(_timeMs);
        return "ok";
    }

    @ACommand(comment = "设置进行多链接检查的间隔时间")
    public String chgCheckingGapTimeMs(long _timeMs)
    {
        NPSDKLoginMgr.getInstance().chgCheckingGapTimeMs(_timeMs);
        return "ok";
    }

    @ACommand(comment = "打印信息")
    public String info()
    {
        return NPSDKLoginMgr.getInstance().toString();
    }

    @ACommand(comment = "增加sdk链接")
    public String addSdkUrl(String _url)
    {
        NPSDKLoginMgr.getInstance().addStandByUrl(_url);
        return "ok";
    }

    @ACommand(comment = "移除sdk链接")
    public String removeSdkUrl(String _url)
    {
        NPSDKLoginMgr.getInstance().removeStandByUrl(_url);
        return "ok";
    }

    @ACommand(comment = "通过命令移除登录账号信息")
    public String removeAcc(String _acc)
    {
        return NPAccInfoMgr.getInstance().cmdRemoveAccountInfo(_acc) ? "ok" : "fail";
    }

    @ACommand(comment = "修改SDK登录预警阈值")
    public String chgWarnThreshold(int _count)
    {
        NPSDKLoginMgr.getInstance().getLoginFailLogger().chgWarnThreshold(_count);
        return "ok";
    }

    @ACommand(comment = "修改SDK登录预警间隔时间Sec")
    public String chgDelaySec(int _sec)
    {
        NPSDKLoginMgr.getInstance().getLoginFailLogger().chgDelaySec(_sec);
        return "ok";
    }
}
