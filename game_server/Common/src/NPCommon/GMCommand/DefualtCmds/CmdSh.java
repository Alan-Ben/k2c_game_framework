package NPCommon.GMCommand.DefualtCmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Util.CommShell;

@ACommander(comment = "日志命令", name = "sh")
public class CmdSh extends CmdClassBase
{
    @ACommand(comment = "显示当前目录")
    public String ll()
    {
        return CommShell.getInstance().ll();
    }

    @ACommand(comment = "进入目录")
    public String cd(String _subDir)
    {
        return CommShell.getInstance().cd(_subDir);
    }

    @ACommand(comment = "返回程序所在目录")
    public String back()
    {
        return CommShell.getInstance().back();
    }

    @ACommand(comment = "显示末尾N条日志(日志路径名,显示行数不能超过300行,从后数第几页0开始)")
    public String tail(String _subFileName, int _pageSize, int _pageIndex)
    {
        return CommShell.getInstance().tail(_subFileName, _pageSize, _pageIndex);
    }

    @ACommand(comment = "显示前面N条日志(日志路径名,每页行数,页索引0开始)")
    public String more(String _subFileName, int _pageSize, int _pageIndex)
    {
        return CommShell.getInstance().more(_subFileName, _pageSize, _pageIndex);
    }

    @ACommand(comment = "進入Log目錄")
    public String cdLog()
    {
        CommShell.getInstance().back();
        CommShell.getInstance().cd("../log");
        return CommShell.getInstance().ll();
    }

}
