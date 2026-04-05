package NPUSServer;

import ALServerLog.ALServerLog;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.NPVersion;
import NPCommon.PHPParam.BSPHPParamMgr;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import NPUSServer.GMCommand.Cmds.CmdServer;
import USDB.USDBBoFilter;

/***************
 * 登录服务器的入口类对象
 *
 * @author Administrator
 *
 */
public class NPUSMain
{
    public static NPUserServer[] g_AllServerArr;
    //异步线程基础数量
    protected static int _g_iBaseAsyncThreadCount = ENPUserServerAsynEnum.values().length;
    //异步线程数量
    protected static int _g_iAsyncThreadCount = 0;

    public static int GetServerCount()
    {
        return g_AllServerArr.length;
    }
    /***
     * 根据索引获取服务器
     * @param _idx
     * @return
     */
    public static NPUserServer GetServer(int _idx)
    {
        return g_AllServerArr[_idx];
    }

    /**
     * 根据服务器类型Id查询服务器
     * @param _typeId
     * @return
     */
    public static NPUserServer GetServerByTypeId(int _typeId)
    {
        for (NPUserServer server : g_AllServerArr)
        {
            if (server.getServerTypeId() == _typeId)
                return server;
        }
        return null;
    }

    /***
     * 根据索引获取异步线程索引
     * @param _idx
     * @return
     */
    public static int GetUserDBAsynThreadIdx(int _idx)
    {
        if(0 == _idx)
            return ENPUserServerAsynEnum.USER_DB.ordinal();

        return _g_iBaseAsyncThreadCount + _idx;
    }


    /**************
     * 主入口函数
     *
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化服务器日志
        ALServerLog.initALServerLog();

        //初始化服务器配置
        if (!UserServerConf.getInstance().init())
        {
            ALServerLog.Fatal("NP User ServerConf Init Fail!!!");
            return;
        }

        //设置初始化的异步线程数量，原来有一个，所以开启数量需要-1
        _g_iAsyncThreadCount = _g_iBaseAsyncThreadCount + (UserServerConf.getInstance().getOpenCount() - 1);

        //根据服务器配置的数量开启服务器
        g_AllServerArr = new NPUserServer[UserServerConf.getInstance().getOpenCount()];

        //逐个开启服务器
        for(int i = 0; i < g_AllServerArr.length; i++)
        {
            //输出日志
            ALServerLog.Sys("Start Server: " + i + " TypeId: " + UserServerConf.getInstance().getTypeId(i));

            g_AllServerArr[i] = new NPUserServer(i);
            //先初始化基本配置，确保端口等数据正确
            if(!g_AllServerArr[i].getConf().init())
            {
                ALServerLog.Fatal("NP User ServerConf TypeId [" + UserServerConf.getInstance().getTypeId(i) + "] Init Conf Fail!!!");
                continue;
            }

            g_AllServerArr[i].startServer(NPVersion.majorVersion()
                    , NPVersion.minorVersion()
                    , NPVersion.buildVersion()
                    , _g_iAsyncThreadCount
                    , null
                    , g_AllServerArr[i].getConf()
                    , new CommonServerStartMonitor());

            //注册第一个服务器到GM执行内作为默认执行对象
            if (0 == i)
            {
                ServerGMUtil.initServer(g_AllServerArr[i], "UserServer");

                //数据库自动保存，只检测US主库
                BoChecker.getInstance().startCheck(new USDBBoFilter());

                //初始化US的GM命令
                GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());
                
                //初始化平台参数，如果是非正式服则不需要重试
                BSPHPParamMgr.getInstance().startLoad(g_AllServerArr[i], !UserServerConf.getInstance().getIsIllegalServer());
            }
        }
    }
}
