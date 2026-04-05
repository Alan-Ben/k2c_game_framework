package NPLoginCheckServer;

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
import NPLoginCheckServer.NPLCSCommonInfo.NPLCSCommonInfoMgr;

/***************
 * 登录服务器的入口类对象
 * @author Administrator
 *
 */
public class NPLCSMain
{
    /**************
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

        //初始化配置
        if (!LoginCheckServerConf.getInstance().init())
        {
            ALServerLog.Fatal("NPLoginCheckServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(LoginCheckServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + LoginCheckServerConf.getInstance().getTimeZone());

        //开启服务器
        NPLoginCheckServer serverObj = new NPLoginCheckServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , ENPLoginCheckServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "LoginCheckServer");
    }

    /**************
     * 申请一个新的用户Id返回
     * @return
     */
    public static long requestNewUid()
    {
        return NPLCSCommonInfoMgr.getInstance().requestNewUid();
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
                ENPLoginCheckServerAsynEnum.ACC_CHECK.ordinal()
                , _callTask
                , _CallBackTask);
    }

    public static <T> void regAccDBAsynTask(_AALAsynCallAndBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                ENPLoginCheckServerAsynEnum.ACC_CHECK.ordinal()
                , _CallBackTask);
    }

    public static void regAccDBAsynTask(_IALAsynRunnableTask _runnableTask)
    {
        ALAsynTaskManager.getInstance().regTask(
                ENPLoginCheckServerAsynEnum.ACC_CHECK.ordinal()
                , _runnableTask);
    }
}
