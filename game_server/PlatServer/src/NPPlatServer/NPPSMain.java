package NPPlatServer;

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

/***************
 * 平台服务器的入口类对象
 * @author Administrator
 *
 */
public class NPPSMain
{
    /**************
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

        if (!PlatServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Plat Server Conf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(PlatServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + PlatServerConf.getInstance().getTimeZone());

        //开启服务器
        PlatServer serverObj = new PlatServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , ENPPlatServerAsynEnum.values().length
                , new CommonServerStartMonitor());
    }

    /***************
     * 直接的注册游戏数据库操作异步任务函数
     *
     * @author alzq.z
     * @time Apr 28, 2013 1:04:10 AM
     */
    public static <T> void regAccDBAsynTask(_IALAsynCallTask<T> _callTask, _IALAsynCallBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                ENPPlatServerAsynEnum.PLAT_DB.ordinal()
                , _callTask
                , _CallBackTask);
    }

    public static <T> void regAccDBAsynTask(_AALAsynCallAndBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                ENPPlatServerAsynEnum.PLAT_DB.ordinal()
                , _CallBackTask);
    }

    public static void regAccDBAsynTask(_IALAsynRunnableTask _runnableTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                ENPPlatServerAsynEnum.PLAT_DB.ordinal()
                , _runnableTask);
    }
}
