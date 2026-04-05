package NPCommon.GMCommand.DefualtCmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.PHPParam.BSPHPParamMgr;

@ACommander(comment = "BS服务器的平台命令", name = "bsPHP")
public class CmdBSPHPParam extends CmdClassBase
{
    @ACommand(comment = "打印平台参数")
    public String phpParam()
    {
        return BSPHPParamMgr.getInstance().toString();
    }
}
