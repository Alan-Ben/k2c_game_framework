package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.RunResult;
import NPUSServer.HotFixActivityMgr.HotFixActivityMgr;
import NPUSServer.NPUSMain;

import java.io.File;

@ACommander(comment = "热加载相关命令", name = "hot")
public class CmdHot extends NPCommon.GMCommand.DefualtCmds.CmdHot
{
    @Override
    @ACommand(comment = "加载类(类必须上传于../hot/目录下)")
    public RunResult load(String _classes)
    {
        RunResult runResult = super.load(_classes);
        if (runResult.isSucc())
        {
            //逐个服务器处理
            for(int i = 0; i < NPUSMain.GetServerCount(); i++)
            {
                NPUSMain.GetServer(i).usServerBroadcastServerVersionToGS();
            }
        }
        return runResult;
    }


    @ACommand(comment = "加载运行活动jar包，jar包必须上传于./activities/ 目录下(包名称)")
    public void loadA(String _jarName)
    {
        if(!_jarName.endsWith(".jar"))
            _jarName = _jarName+".jar";

        File file = new File("./activities/"+ _jarName);
        if(!file.exists())
        {
            takeCallBack().onRunOver(false, String.format("file: %s not exists!", file.getPath()));
            return ;
        }
        if(!file.isFile())
        {
            takeCallBack().onRunOver(false, String.format("file: %s is not a file!", file.getPath()));
            return;
        }

        HotFixActivityMgr.getInstance().loadOneAsync(file, (_result) ->
        {
            if (!_result.isSucc())
            {
                CommLog.error("load activity jar:{} err:{}", file.getName(), _result.getMsg(), new Exception());
                takeCallBack().onRunOver(_result.isSucc(), _result.getMsg());
                return;
            }
            RunResult result = load(NPVersion.class.getName());
            if (!result.isSucc())
            {
                takeCallBack().onRunOver(result.isSucc(), result.getMsg());
                return;
            }

            String msg = String.format("load activity jar:%s succ:%s!", file.getName(), result.getMsg());
            CommLog.info(msg);
            takeCallBack().onRunOver(true, msg);
        });
    }
}
