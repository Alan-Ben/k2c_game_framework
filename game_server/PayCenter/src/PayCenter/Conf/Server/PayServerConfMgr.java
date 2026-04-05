package PayCenter.Conf.Server;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommFile;
import NPCommon.Util.JsonUtil;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.util.ArrayList;
import java.util.List;

public class PayServerConfMgr
{
    private static PayServerConfMgr g_instance = new PayServerConfMgr();

    public static PayServerConfMgr getInstance()
    {
        return g_instance;
    }

    private List<PayServerConf> m_confList = new ArrayList<>();
    /*******************
     * 初始化函数
     *
     * @param _properties
     */

    public static final String defualtFilePath = "./conf/PayServerListConf.json";
    public static final String customFilePath = "./customConf/PayServerListConf.json";

    public boolean init()
    {
        if (initCore())
        {
            CommLog.info("ServerList conf Init OK!\n{}", toString());
            return true;
        } else
        {
            return false;
        }
    }

    private boolean initCore()
    {
        m_confList.clear();

        String defaultTxt = CommFile.getTextFromFile(defualtFilePath);
        if (defaultTxt == null || defaultTxt.isEmpty())
        {
            CommLog.error("default server list config:{} not found", defualtFilePath);
            return false;
        }

        List<PayServerConf> defaultConfList = new ArrayList<>();
        CommLog.info("Parsing config file:{}", defualtFilePath);
        if (!_initConfList(defaultConfList, defaultTxt))
            return false;

        String customTxt = CommFile.getTextFromFile(customFilePath);
        if (customTxt != null && !customTxt.isEmpty())//自定义配置不为空的情况下使用自定义配置
        {
            List<PayServerConf> customConfList = new ArrayList<>();
            CommLog.info("Parsing config file:{}", customFilePath);
            if (!_initConfList(customConfList, customTxt))
            {
                return false;
            } else
            {
                CommLog.info("Using custom server list:{}", customFilePath);
                m_confList.addAll(customConfList);
                return true;
            }
        } else
        {
            //自定义配置不存在，使用默认配置
            CommLog.info("custom server list file:{} is empty,ignored!", customFilePath);
            m_confList.addAll(defaultConfList);
            return true;
        }

    }

    /*******************
     * 初始化单个配置文件的处理函数
     */
    private boolean _initConfList(List<PayServerConf> _confList, String txt)
    {
        _confList.clear();

        if (null == txt || txt.isEmpty())
        {
            CommLog.error("load conf failed,txt is empty!");
            return false;
        }

        JsonElement root;
        try
        {
            root = new JsonParser().parse(txt);
        } catch (Exception e)
        {
            CommLog.error("parse  config file:{} failed!", txt, e);
            return false;
        }
        
        if (!root.isJsonArray())
        {
            CommLog.error("root of json is not array");
            return false;
        }
        
        JsonArray arrayRoot = root.getAsJsonArray();
        for (JsonElement confElement : arrayRoot)
        {
            if (!confElement.isJsonObject())
            {
                CommLog.error("config element:{} is not JsonObj", confElement.toString());
                return false;
            }

            PayServerConf conf = parseConf(confElement.getAsJsonObject());
            if (null == conf)
            {
                CommLog.error("config element:{} Parsed failed", confElement.toString());
                return false;
            }

            if (!addConf(_confList, conf))
            {
                return false;
            }
        }
        if (_confList.isEmpty())
        {
            CommLog.error("load conf failed,server list is empty!");
            return false;
        }

        return true;
    }

    private boolean addConf(List<PayServerConf> _confList, PayServerConf _conf)
    {
        for (PayServerConf conf : _confList)
        {
            if (conf.getId() == _conf.getId())
            {
                CommLog.error("duplicate plat id:{}", conf.getId());
                return false;
            }
            if (conf.getPlatServerIp().equals(_conf.getPlatServerIp()) && conf.getPlatServerPort() == _conf.getPlatServerPort())
            {
                CommLog.error("duplicate plat ip:{} port:{}", conf.getPlatServerIp(), conf.getPlatServerPort());
                return false;
            }
            if (conf.getInternalIp().equals(_conf.getInternalIp()) && conf.getInternalPort() == _conf.getInternalPort())
            {
                CommLog.error("duplicate internal ip:{} port:{}", conf.getInternalIp(), conf.getInternalPort());
                return false;
            }
        }
        _confList.add(_conf);
        return true;
    }

    /************
     * 从json字符串转化为实际的配置
     * @param _confObj
     * @return
     */
    private PayServerConf parseConf(JsonObject _confObj)
    {
        if (null == _confObj)
            return null;

        int id = JsonUtil.getInt(_confObj, "Id", 0);
        if (0 == id)
            return null;

        int RecBufferLen = JsonUtil.getInt(_confObj, "RecBufferLen", 0);
        if (0 == RecBufferLen)
            return null;
        int Port = JsonUtil.getInt(_confObj, "Port", 0);
        if (0 == Port)
            return null;
        String PlatServerIp = JsonUtil.getString(_confObj, "PlatServerIp", "");
        if (PlatServerIp.isEmpty())
            return null;
        int PlatServerPort = JsonUtil.getInt(_confObj, "PlatServerPort", 0);
        if (PlatServerPort == 0)
            return null;
        String PlatServerPassword = JsonUtil.getString(_confObj, "PlatServerPassword", "");
        if (PlatServerPassword.isEmpty())
            return null;
        String InternalIp = JsonUtil.getString(_confObj, "InternalIp", "");
        if (InternalIp.isEmpty())
            return null;
        int InternalPort = JsonUtil.getInt(_confObj, "InternalPort", 0);
        if (InternalPort == 0)
            return null;
        if (InternalPort != Port)
        {
            CommLog.error("InternalPort:{} != Port:{}", InternalPort, Port);
            return null;
        }

        return new PayServerConf(id
                , RecBufferLen
                , Port
                , PlatServerIp
                , PlatServerPort
                , PlatServerPassword
                , InternalIp
                , InternalPort
        );
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("Total %d PayServer:\n", m_confList.size()));
        for (int i = 0; i < m_confList.size(); i++)
        {
            PayServerConf conf = m_confList.get(i);

            sb.append(String.format("Server[%d]:\n", i));
            sb.append(String.format("\tId=%s\n", conf.getId()));
            sb.append(String.format("\tPlatServerIp=%s\n", conf.getPlatServerIp()));
            sb.append(String.format("\tPlatServerPort=%d\n", conf.getPlatServerPort()));
            sb.append(String.format("\tPlatServerPassword=%s\n", conf.getPlatServerPassword()));
            sb.append(String.format("\tInternalIp=%s\n", conf.getInternalIp()));
            sb.append(String.format("\tInternalPort=%s\n", conf.getInternalPort()));
            sb.append(String.format("\tPort=%d\n", conf.getPort()));
            sb.append(String.format("\tRecBufferLen=%s\n", conf.getClientRecBufferLen()));
        }
        return sb.toString();
    }

    public List<PayServerConf> getConfList()
    {
        return new ArrayList<>(m_confList);
    }
}
