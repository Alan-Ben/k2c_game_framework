package NPCommon.GMCommand.DefualtCmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.GitNode.NPGitNode;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;

@ACommander(comment = "系统相关命令", name = "system")
public class CmdSystem extends CmdClassBase
{
    public static final long mb = 1024 * 1024;

    @ACommand(comment = "调用系统gc")
    public String gc()
    {
        System.gc();
        StringBuilder sb = new StringBuilder();
        sb.append("Run Gc Over:\n");
        sb.append(getMemStatString());
        return sb.toString();
    }

    @ACommand(comment = "打印版本相关信息")
    public String ver()
    {
        String str = "Version: " + NPVersion.getString();
        return str + "\nGit Node:" + NPGitNode.getGitNodeInfo();
    }

    @ACommand(comment = "打印内存信息")
    public String mem()
    {
        return getMemStatString();
    }

    private String getMemStatString()
    {
        StringBuilder sb = new StringBuilder();

        long totalMem = Runtime.getRuntime().totalMemory() / mb;
        long freeMem = Runtime.getRuntime().freeMemory() / mb;
        long maxMem = Runtime.getRuntime().maxMemory() / mb;
        sb.append(String.format("[mem]: total[%dMB] , free[%dMB] , maxavail[%dMB]\n", totalMem, freeMem, maxMem));
        return sb.toString();
    }

    @ACommand(comment = "返回进程id")
    public String pid()
    {
        return String.valueOf(CommonFunc.getPid());
    }

    @ACommand(comment = "显示参数(参数名)")
    public String getP(String _sName)
    {
        return System.getProperty(_sName);
    }

    @ACommand(comment = "显示全部参数")
    public String listP()
    {
        StringBuilder sb = new StringBuilder();
        for (Object key : System.getProperties().keySet())
        {
            String value = System.getProperty(key.toString());
            sb.append('[').append(key).append("]=").append(value).append('\n');
        }
        return sb.toString();
    }

    @ACommand(comment = "设置参数(参数名,参数值)")
    public String setP(String _sName, String _value)
    {
        System.setProperty(_sName, _value);
        return "ok";
    }

    @ACommand(comment = "打印当前时间")
    public String printTime()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("now time ms:%d\n", CommonFunc.getNowTimeMS()));
        sb.append(CommonFunc.getTimeStringSec(CommonFunc.getNowTimeSec())).append('\n');
        sb.append(String.format("time zone:%s", CommonFunc.getTimeZone().toString()));
        String str = sb.toString();
        CommLog.info(str);
        return str;
    }
}
