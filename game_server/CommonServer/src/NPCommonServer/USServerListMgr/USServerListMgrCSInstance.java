package NPCommonServer.USServerListMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import ALServerLog.ALServerLog.LogLevel;
import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;
import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NPCommon.Log.CommLog;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommonServer.CommonServerConf;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_B_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_001_BasicOp;
import ServerListMgr.ServerListMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.List;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-24 16:44:47
 */
public class USServerListMgrCSInstance extends ServerListMgr implements _IHandlerHolder
{
    //////单例的//////
    private static final USServerListMgrCSInstance _s_instance = new USServerListMgrCSInstance();
    public static USServerListMgrCSInstance getInstance()
    {
        return _s_instance;
    }
    
    //用于后台数据变化的序列号
    private static long _s_lPHPSerial;
    public synchronized static long getPHPSerial()
    {
    	_s_lPHPSerial = ALSerializeMaker.makeNewSerialize();
    	return _s_lPHPSerial;
    }
    
    //平台请求失败次数，用于是否预警
    private int _m_iPHPFailCount;

    private USServerListMgrCSInstance()
    {
        OnServerListInited.addHandler(this, new HandlerNone()
        {
            @Override
            public void handle()
            {
                //向GS广播初始化信息
                NPCommonServer.getInstance().broadcastMessage(EServerType.GATE.ordinal()
                        , NP2GS_B_Writer_001_BasicOp.make_006_USIndexListInit(getServerIndexItemList()));

                //向US广播初始化信息
                NPCommonServer.getInstance().broadcastMessage(EServerType.USER.ordinal()
                        , NP2US_B_Writer_001_BasicOp.make_006_USInfoListInit());
            }
        });

        OnServerIndexUpdate.addHandler(this, new HandlerOne<List<NpServerObj_SYS_ServerIndexInfo>>()
        {
            @Override
            public void handle(List<NpServerObj_SYS_ServerIndexInfo> _serverList)
            {
                //向GS广播服务器信息变更
                NPCommonServer.getInstance().broadcastMessage(EServerType.GATE.ordinal()
                        , NP2GS_B_Writer_001_BasicOp.make_007_USIndexListChg(_serverList));
            }
        });

        OnServerItemUpdate.addHandler(this, new HandlerOne<List<NpServerObj_SYS_ServerItem>>()
        {
            @Override
            public void handle(List<NpServerObj_SYS_ServerItem> _serverList)
            {
                for (NpServerObj_SYS_ServerItem serverItem : _serverList)
                {
                    NPCommonServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), serverItem.getServerTypeId()
                            , NP2US_Writer_001_BasicOp.make_003_USInfoChg(serverItem));
                }
            }
        });
    }
    
    /**
     * 开启服务器列表加载
     * @return
     */
    public boolean startLoad() 
    {
        //使用文件加载
        if (CommonServerConf.getInstance().getNeedLoadFooServerList())
        {
            //加载配置
            String serverJsonText = USServerListMgrCSInstance.getInstance().readServerList();

            //加载本地配置
            boolean initSuc = USServerListMgrCSInstance.getInstance().initFromJsonStr(serverJsonText);
            if (!initSuc)
            {
                ALServerLog.Fatal("USServerListMgrCSInstance.getInstance().initFromJsonStr Fail!!!");
                return false;
            }
            
            ALServerLog.Info("USServerList Load From File Suc.");
        }
        else //获取后台的服务器列表（CS -> HS -> PHP后台）
        {
        	reqUSServerListFromPHP();
        }

        return true;
    }

    /**
     * 构造服务器索引信息
     * @param serverItem 服务器信息结构体
     * @return NpServerObj_SYS_ServerIndexInfo
     */
    public NpServerObj_SYS_ServerIndexInfo makeServerIndexInfo(NpServerObj_SYS_ServerIndexInfo serverItem)
    {
        NpServerObj_SYS_ServerIndexInfo indexInfo = new NpServerObj_SYS_ServerIndexInfo();
        indexInfo.setServerLogicId(serverItem.getServerLogicId());
        indexInfo.setServerTypeId(serverItem.getServerTypeId());
        return indexInfo;
    }

    /**
     * 从文件读取服务器信息
     * @param _filePath 文件路径
     * @return 服务器Json信息
     */
    public String _readServerList(String _filePath)
    {
        FileInputStream serverJsonListStream = null;
        try
        {
            serverJsonListStream = new FileInputStream(_filePath);
        } catch (Exception e)
        {
            CommLog.error("", e);
        }

        if (serverJsonListStream == null)
        {
            return "";
        }

        StringBuilder serverJsonText = new StringBuilder();
        try
        {
            //读取数据
            byte[] bytes = new byte[serverJsonListStream.available()];// 修改了这里
            int len;// 记录每次读取的字节的个数
            while ((len = serverJsonListStream.read(bytes)) != -1 && len != 0)
            {
                String str = new String(bytes, 0, len);
                serverJsonText.append(str);
            }

            serverJsonListStream.close();
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "";
        }

        return serverJsonText.toString();
    }

    /**
     * 读取服务器列表
     */
    public String readServerList()
    {
        //先从默认配置中读取
        String serverJsonText = _readServerList("./conf/FooServerListJson.txt");

        //再从自定义配置中读取
        String customServerJsonText = _readServerList("./customConf/FooServerListJson.txt");
        //如果自定义配置不为空，则使用自定义配置
        if (!customServerJsonText.isEmpty())
        {
            serverJsonText = customServerJsonText;
        }

        return serverJsonText;
    }
    
    /********************************* Begin: PHP 后台交互部分 *********************************/
    
    /**
     * 向PHP发起请求
     */
    public void reqUSServerListFromPHP()
    {
    	long phpSerial = getPHPSerial();
    	CommLog.info("USServerList Init Load Server Send PHP Request, Serial:{}.", phpSerial);
    	ALSynTaskManager.getInstance().regTask(new USServerListLoadFromPHPTask(phpSerial));
    }
    /**
     * 请求回调成功，确认是否更新服务器列表
     * @param _phpSerial
     * @param _itemList
     */
    public void callbackUSServerListFromPHPSuc(long _phpSerial, ArrayList<NP_SYS_ServerItem> _itemList)
    {
    	if(_phpSerial != _s_lPHPSerial)
    		return;
    	
    	_m_iPHPFailCount = 0;
    	
    	USServerListMgrCSInstance.getInstance().initLoadServerList(_itemList);
    	
    	CommLog.sys("USServerList Init Load Server from PHP Suc, Serial:{}.", _phpSerial);
    	NPCommonServer.getInstance().getDDAlert().alert(LogLevel.INFO, "PHP-USLIST-SUC", "后台更新User服务器列表成功");
    }
    /**
     * 请求回调失败，确认是否再次发起请求
     * @param _phpSerial
     */
    public void callbackUSServerListFromPHPFail(long _phpSerial)
    {
    	if(_phpSerial != _s_lPHPSerial)
    		return;
    	
    	_m_iPHPFailCount++;
    	
    	CommLog.error("USServerList Init Load Server from PHP Error, FailCount:{} Serial:{}.", _m_iPHPFailCount, _phpSerial);
    	if(_m_iPHPFailCount > 10)
    	{
    		NPCommonServer.getInstance().getDDAlert().err("PHP-USLIST-ERR", "后台获取User服务器列表失败，次数：[" + _m_iPHPFailCount + "]");
    	}
    	
    	//1秒后重新发起请求
    	ALSynTaskManager.getInstance().regTask(() -> reqUSServerListFromPHP(), 3000);
    }
    
    /********************************* End: PHP 后台交互部分 *********************************/
}
