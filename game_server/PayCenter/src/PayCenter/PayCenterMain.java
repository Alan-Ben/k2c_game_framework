package PayCenter;

import ALBasicServer.ALBasicServer;
import ALServerLog.ALServerLog;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import PayCenter.Conf.PayCenterConf;
import PayCenter.Conf.Server.PayServerConf;
import PayCenter.Conf.Server.PayServerConfMgr;
import PayCenter.PayServer.GMCommand.Cmds.CmdServer;
import PayCenter.PayServer.PayServer;

import java.util.List;

/**
 * PayCenterMain - 支付中心服务器主入口
 * 
 * 主要功能：
 * 1. 启动PayCenter系统初始化
 * 2. 根据配置创建多个PayServer实例
 * 3. 配置时区和版本信息
 * 4. 初始化GM命令系统和数据库检查
 * 
 * 启动流程：
 * 1. 初始化PayCenter核心系统
 * 2. 加载PayServer配置列表
 * 3. 为每个配置创建PayServer实例
 * 4. 设置时区和启动参数
 * 5. 启动服务器并注册GM系统
 */
public class PayCenterMain
{
    /**
     * 主入口函数
     * 
     * @param args 命令行参数
     */
    public static void main(String[] args)
    {
        ALServerLog.initALServerLog();

        // 初始化配置
        PayCenterConf.getInstance().init();

        //服务器初始化
        ALBasicServer.initBasicServer(EPayServerAsynEnum.values().length);

        if (!PayCenter.getInstance().init())
        {
            CommLog.fatal("PayCenter Init Fail!!!");
            return;
        }

        // 设置时区
        CommonFunc.setTimeZone(PayCenterConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + PayCenterConf.getInstance().getTimeZone());

        List<PayServerConf> confList = PayServerConfMgr.getInstance().getConfList();
        for (PayServerConf conf : confList)
        {
            // 输出日志
            CommLog.info("Start PayServer TypeId: " + conf.getId());

            PayServer server = new PayServer(conf);
            PayCenter.getInstance().initAddServer(server);

            server.startServer(NPVersion.majorVersion()
                    , NPVersion.minorVersion()
                    , NPVersion.buildVersion()
                    , EPayServerAsynEnum.values().length
                    , null
                    , conf
                    , new CommonServerStartMonitor());

            if (conf.getId() == 1)
            {
                ServerGMUtil.initServer(server, PayCenter.getInstance().getDDAlert(), "PayCenter");

                // 数据库自动保存，只检测Pay主库
                BoChecker.getInstance().startCheck(_bo -> _bo.getDBTag() == EDBTag.pc_db);

                // 初始化GM命令
                GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());
            }
        }
    }
}