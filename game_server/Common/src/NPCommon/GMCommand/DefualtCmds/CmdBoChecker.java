package NPCommon.GMCommand.DefualtCmds;

import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;

@ACommander(comment = "数据库相关命令", name = "BoChecker")
public class CmdBoChecker extends CmdClassBase
{
    @ACommand(comment = "显示BoChecker信息")
    public String info()
    {
        return BoChecker.getInstance().toString();
    }

    @ACommand(comment = "设置数据库检测开关(1开0关)")
    public String enable(int _isOn)
    {
        BoChecker.getInstance().setEnabled(_isOn != 0);
        return "BoChecker switch is now :" + BoChecker.getInstance().isEnabled();
    }

    @ACommand(comment = "设置最大队列长度(检测队列长度)")
    public String setMaxSize(int _maxSize)
    {
        BoChecker.getInstance().setMaxQueueSize(_maxSize);
        return "BoChecker max queue size:" + BoChecker.getInstance().getMaxQueueSize();
    }
}
