package NPCommonServer;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALTask._AALAsynCallAndBackTask;
import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALAsynCallTask;
import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

/***************
 * 登录服务器的入口类对象
 * @author Administrator
 *
 */
public class NPCSMain
{
    /**************
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

    	//初始化服务器配置
        if (!CommonServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Common ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(CommonServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + CommonServerConf.getInstance().getTimeZone());
        //开启服务器
        NPCommonServer serverObj = new NPCommonServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , EWCGCommonServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "CommonServer");
    }

    /***************
     * 直接的注册游戏数据库操作异步任务函数
     *
     * @author alzq.z
     * @time Apr 28, 2013 1:04:10 AM
     */
    public static <T> void regCommonDBAsynTask(_IALAsynCallTask<T> _callTask, _IALAsynCallBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                EWCGCommonServerAsynEnum.COMMON_DB.ordinal()
                , _callTask
                , _CallBackTask);
    }

    public static <T> void regCommonDBAsynTask(_AALAsynCallAndBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                EWCGCommonServerAsynEnum.COMMON_DB.ordinal()
                , _CallBackTask);
    }

    public static void regCommonDBAsynTask(_IALAsynRunnableTask _runnableTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                EWCGCommonServerAsynEnum.COMMON_DB.ordinal()
                , _runnableTask);
    }
}
