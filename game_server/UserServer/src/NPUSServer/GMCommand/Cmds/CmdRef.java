package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.RunResult;
import NPUSServer.GMCommand.UsCmdPlayerExecutor;
import NPUSServer.GMCommand.UsCmdServerExecutor;
import NPUSServer.NPUSMain;
import NPUSServer.NPUserServer;

@ACommander(comment = "配置表相关命令", name = "ref")
public class CmdRef extends NPGameRes.GmCmds.CmdRef
{
    @Override
    @ACommand(comment = "重新加载")
    public RunResult reload(String _reloadTag)
    {
        RunResult runResult = super.reload(_reloadTag);
        if (runResult.isSucc())
        {
            //逐个服务器处理
            for (int i = 0; i < NPUSMain.GetServerCount(); i++)
            {
                NPUSMain.GetServer(i).usServerBroadcastServerVersionToGS();
            }
        }
        return runResult;
    }

    @Override
    @ACommand(comment = "准备热更新数据")
    public RunResult reloadReady()
    {
        RunResult runResult = super.reloadReady();
        if (runResult.isSucc())
        {
            //逐个服务器处理
            for (int i = 0; i < NPUSMain.GetServerCount(); i++)
            {
                NPUSMain.GetServer(i).usServerBroadcastServerVersionToGS();
            }
        }
        return runResult;
    }

    @Override
    @ACommand(comment = "处理热更新数据")
    public RunResult reloadDeal(String _reloadTag)
    {
        RunResult runResult = super.reloadDeal(_reloadTag);
        if (runResult.isSucc())
        {
            //逐个服务器处理
            for (int i = 0; i < NPUSMain.GetServerCount(); i++)
            {
                NPUSMain.GetServer(i).usServerBroadcastServerVersionToGS();
            }
        }
        return runResult;
    }

    public NPUserServer getUserServer()
    {
        if (getExecutor() instanceof UsCmdServerExecutor)
        {
            return ((UsCmdServerExecutor) getExecutor()).getUsServer();
        } else if (getExecutor() instanceof UsCmdPlayerExecutor)
        {
            return ((UsCmdPlayerExecutor) getExecutor()).getUserData().getUSServer();
        } else
        {
            return null;
        }
    }
}
