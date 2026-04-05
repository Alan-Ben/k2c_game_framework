package ServerListMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;
import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NPCommon.Log.CommLog;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Util.Delegate.ADelegateNone;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.JsonUtil;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.util.*;
import java.util.function.Predicate;

/**
 * @description: 服务器在线信息管理器
 * 需要同步到其他服务器上的服务器信息管理，
 * 包含服务器在线信息和开放状态
 * NPServerObj_SYS_ServerItem 中 serverLogicId 是外部赋予的服务器id
 * serverTypeId 是服务器运行的实例id
 * @author: ricci
 * @date: 2022-06-24 11:24:26
 */
public class ServerListMgr
{

    /**
     * 服务器logicId -> usServerTypeId 对照表
     */
    private final Map<Integer, NpServerObj_SYS_ServerIndexInfo> _m_serverIndexMap;

    /**
     * 服务器状态信息查询map
     * serverTypeId: serverItem
     */
    private final Map<Integer, NpServerObj_SYS_ServerItem> _m_serverItemMap;


    /**
     * _m_serverItemMap 和 _m_serverIndexMap 的锁对象
     */
    private final MutexObject _m_mutex;

    /**
     * 是否已经初始化
     */
    private boolean isInited;

    /**
     * 服务器对照表更新监听
     */
    public ADelegateOne<List<NpServerObj_SYS_ServerIndexInfo>> OnServerIndexUpdate;

    /**
     * 服务器对照表更新监听
     */
    public ADelegateOne<List<NpServerObj_SYS_ServerItem>> OnServerItemUpdate;

    /**
     * 已初始化监听
     */
    public ADelegateNone OnServerListInited;

    public ServerListMgr()
    {
        this._m_serverIndexMap = new HashMap<>();
        this._m_serverItemMap = new HashMap<>();
        this._m_mutex = new MutexObject();

        OnServerIndexUpdate = new ADelegateOne<>(this);
        OnServerItemUpdate = new ADelegateOne<>(this);
        OnServerListInited = new ADelegateNone(this);
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    // region get&&set

    public List<NpServerObj_SYS_ServerIndexInfo> getServerIndexItemList()
    {
        return new ArrayList<>(_m_serverIndexMap.values());
    }

    public boolean isInited()
    {
        return isInited;
    }

    public void setInited(boolean _inited)
    {
        isInited = _inited;
    }
    // endregion


    /**
     * 初始化加载服务器列表
     * @param _serverList 给定服务器列表
     */
    public void initLoadServerList(ArrayList<NpServerObj_SYS_ServerItem> _serverList
            , ArrayList<NpServerObj_SYS_ServerIndexInfo> _indexList)
    {
        _lock();
        try
        {
            //清空
            __clear();

            //外部展示id与服务器typeId对应关系列表 初始化
            for (NpServerObj_SYS_ServerIndexInfo item : _indexList)
            {
                if (item == null)
                {
                    continue;
                }
                if (_m_serverIndexMap.containsKey(item.getServerLogicId()))
                {
                    CommLog.error("ServerListMgr loadServerIndexList has multi indexInfo typeId:{}"
                            , item.getServerTypeId());
                    continue;
                }
                _m_serverIndexMap.put(item.getServerLogicId(), item);
            }
            //服务器状态信息加载
            for (NpServerObj_SYS_ServerItem item : _serverList)
            {
                if (item == null)
                {
                    continue;
                }
                if (_m_serverItemMap.containsKey(item.getServerTypeId()))
                {
                    CommLog.warn("ServerListMgr loadServerList has multi serverItem typeId:{}"
                            , item.getServerTypeId());
                    continue;
                }
                _m_serverItemMap.put(item.getServerTypeId(), item);
            }

            //触发初始化通知
            OnServerListInited.onAsyncEvent();
            setInited(true);
            CommLog.info("ServerListMgr init  ok serverList:{}", toString());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 从json中读取服务器列表
     */
    public boolean initFromJsonStr(String _jsonStr)
    {
        try
        {
            ArrayList<NpServerObj_SYS_ServerItem> serverItems = new ArrayList<>();
            ArrayList<NpServerObj_SYS_ServerIndexInfo> serverIndexes = new ArrayList<>();

            JsonObject obj = new JsonParser().parse(_jsonStr).getAsJsonObject();
            JsonObject data = JsonUtil.getJsonObj(obj, "data");
            if (data == null)
            {
                CommLog.error("ServerListMgr initFromJsonStr data is null,str:{}", _jsonStr);
                return false;
            }
            JsonArray server_list = JsonUtil.getJsonArray(data, "server_list");
            for (JsonElement element : server_list)
            {
                if (element == null)
                {
                    continue;
                }
                JsonObject serverObj = element.getAsJsonObject();
            /*
                areaTag:"as"
                ,groupId:1
                ,serverTypeId:1
                ,serverName:"我是1服"
                ,onlineStateTypeId:1
                ,showStateTypeId:1
                ,isNew:true
                ,startDate:"2022-07-01"
                ,ext:""
             */
                String areaTag = JsonUtil.getString(serverObj, "areaTag");
                long groupId = JsonUtil.getLong(serverObj, "groupId");
                int serverTypeId = JsonUtil.getInt(serverObj, "serverTypeId");
                String serverName = JsonUtil.getString(serverObj, "serverName");
                int onlineStateTypeId = JsonUtil.getInt(serverObj, "onlineStateTypeId");
                int showStateTypeId = JsonUtil.getInt(serverObj, "showStateTypeId");
                boolean isNew = JsonUtil.getBool(serverObj, "isNew");
                String startDate = JsonUtil.getString(serverObj, "startDate");
                String ext = JsonUtil.getString(serverObj, "ext");

                serverItems.add(new NpServerObj_SYS_ServerItem(areaTag, groupId, serverTypeId,
                        serverName, onlineStateTypeId, showStateTypeId, isNew, startDate, ext));
            }

            JsonArray server_index_list = JsonUtil.getJsonArray(data, "server_index_list");
            for (JsonElement element : server_index_list)
            {
                if (element == null)
                {
                    continue;
                }
                JsonObject serverObj = element.getAsJsonObject();
            /*
                serverLogicId:1
                ,serverTypeId:1
             */

                int serverLogicId = JsonUtil.getInt(serverObj, "serverLogicId");
                int serverTypeId = JsonUtil.getInt(serverObj, "serverTypeId");

                serverIndexes.add(new NpServerObj_SYS_ServerIndexInfo(serverLogicId, serverTypeId));
            }
            
            initLoadServerList(serverItems, serverIndexes);
            
            return true;
        } catch (Exception e)
        {
            CommLog.error("", e);
            return false;
        }
    }

    /**
     * 通过 NP_SYS_ServerItem 数据初始化对象
     * @param _serverInfoList ArrayList<NP_SYS_ServerItem>
     */
    public void initLoadServerList(ArrayList<NP_SYS_ServerItem> _serverInfoList)
    {
        _lock();
        try
        {
            //构建两个初始化所需列表
            ArrayList<NpServerObj_SYS_ServerItem> _serverList = new ArrayList<>();
            ArrayList<NpServerObj_SYS_ServerIndexInfo> _indexList = new ArrayList<>();
            transSysItem(_serverList, _indexList, _serverInfoList);
            //初始化ServerList
            initLoadServerList(_serverList, _indexList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 转换 NP_SYS_ServerItem 为 NpServerObj_SYS_ServerItem 和 NpServerObj_SYS_ServerIndexInfo
     */
    public void transSysItem(ArrayList<NpServerObj_SYS_ServerItem> _serverList,
                             ArrayList<NpServerObj_SYS_ServerIndexInfo> _indexList,
                             ArrayList<NP_SYS_ServerItem> _serverInfoList)
    {
        for (NP_SYS_ServerItem serverItem : _serverInfoList)
        {
            if (serverItem == null)
            {
                continue;
            }
            //服务器列表
            NpServerObj_SYS_ServerItem sysServerItem = new NpServerObj_SYS_ServerItem();
            sysServerItem.setAreaTag(serverItem.getAreaTag());
            sysServerItem.setGroupId(serverItem.getGroupId());
            sysServerItem.setServerTypeId(serverItem.getServerTypeId());
            sysServerItem.setServerName(serverItem.getServerName());
            sysServerItem.setOnlineStateTypeId(serverItem.getOnlineStateTypeId());
            sysServerItem.setShowStateTypeId(serverItem.getShowStateTypeId());
            sysServerItem.setIsNew(serverItem.getIsNew());
            sysServerItem.setStartDate(serverItem.getStartDate());
            sysServerItem.setExt(serverItem.getExt());
            _serverList.add(sysServerItem);

            //服务器index列表
            NpServerObj_SYS_ServerIndexInfo sysServerIndexInfo = new NpServerObj_SYS_ServerIndexInfo();
            sysServerIndexInfo.setServerLogicId(serverItem.getServerLogicId());
            sysServerIndexInfo.setServerTypeId(serverItem.getServerTypeId());
            _indexList.add(sysServerIndexInfo);
        }
    }

    /**
     * 更新服务器列表
     * @param _serverItemList 服务器列表
     * @return boolean
     */
    public boolean updateServerItem(ArrayList<NP_SYS_ServerItem> _serverItemList)
    {
        if (_serverItemList.isEmpty())
        {
            CommLog.warn("ServerListMgr updateServerItem serverItemList is empty");
            return true;
        }

        ArrayList<NpServerObj_SYS_ServerItem> serverList = new ArrayList<>();
        ArrayList<NpServerObj_SYS_ServerIndexInfo> indexList = new ArrayList<>();
        transSysItem(serverList, indexList, _serverItemList);

        _lock();
        try
        {
            //更新展示数据
            for (NpServerObj_SYS_ServerItem serverItem : serverList)
            {
                _m_serverItemMap.put(serverItem.getServerTypeId(), serverItem);
            }
            for (NpServerObj_SYS_ServerIndexInfo item : indexList)
            {
                _m_serverIndexMap.put(item.getServerLogicId(), item);
            }
        } finally
        {
            _unlock();
        }

        //维护完成再触发更新
        OnServerIndexUpdate.onAsyncEvent(indexList);
        OnServerItemUpdate.onAsyncEvent(serverList);

        //日志
        StringBuilder sb = new StringBuilder();
        sb.append("server list update:\n");
        for (NpServerObj_SYS_ServerItem serverItem : serverList)
        {
            sb.append(getString(serverItem)).append("\n");
        }
        sb.append("server index list update:\n");
        for (NpServerObj_SYS_ServerIndexInfo item : indexList)
        {
            sb.append(getString(item)).append("\n");
        }
        CommLog.info(sb.toString());

        return true;
    }

    /**
     * 更新服务器信息
     * @param _newItem 新的信息对象
     */
    public void updateServerItem(NpServerObj_SYS_ServerItem _newItem)
    {
        _lock();
        try
        {
            //更新展示数据
            _m_serverItemMap.put(_newItem.getServerTypeId(), _newItem);
        } finally
        {
            _unlock();
        }

        OnServerItemUpdate.onAsyncEvent(Collections.singletonList(_newItem));

        //日志
        StringBuilder sb = new StringBuilder();
        sb.append("server list update:\n");
        sb.append(getString(_newItem)).append("\n");
        CommLog.info(sb.toString());
    }

    /**
     * 清理容器中的数据
     */
    private void __clear()
    {
        _lock();
        try
        {
            _m_serverIndexMap.clear();
            _m_serverItemMap.clear();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询服务器信息
     * @param _serverTypeId 服务器id
     * @return NpServerObj_SYS_ServerItem
     */
    public NpServerObj_SYS_ServerItem lookupServerInfoByTypeId(int _serverTypeId)
    {
        _lock();
        try
        {
            return _m_serverItemMap.get(_serverTypeId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询服务器信息
     * @param _serverLogicId 服务器逻辑id
     * @return NpServerObj_SYS_ServerItem
     */
    public NpServerObj_SYS_ServerItem lookupServerInfoByLogicId(int _serverLogicId)
    {
        _lock();
        try
        {
            NpServerObj_SYS_ServerIndexInfo indexInfo = _m_serverIndexMap.get(_serverLogicId);
            if (indexInfo == null)
                return null;

            return _m_serverItemMap.get(indexInfo.getServerTypeId());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过过滤方法获得所需服务器列表
     * @param _testFunc 过滤方法，为true加入到列表
     * @return ArrayList<NpServerObj_SYS_ServerItem>
     */
    public ArrayList<NP_SYS_ServerItem> lookupServerListByTestFunc(Predicate<NP_SYS_ServerItem> _testFunc)
    {
        _lock();
        try
        {
            ArrayList<NP_SYS_ServerItem> list = new ArrayList<>();
            for (NP_SYS_ServerItem item : makeServerItemList())
            {
                if (item == null)
                {
                    continue;
                }

                if (_testFunc.test(item))
                {
                    list.add(item);
                }
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造服务器信息列表
     * @return ArrayList<NP_SYS_ServerItem>
     */
    public ArrayList<NP_SYS_ServerItem> makeServerItemList()
    {
        ArrayList<NP_SYS_ServerItem> serverItems = new ArrayList<>();
        _lock();
        try
        {
            //遍历logicId列表
            for (NpServerObj_SYS_ServerIndexInfo indexInfo : _m_serverIndexMap.values())
            {
                if (indexInfo == null)
                {
                    continue;
                }
                //查询服务器状态信息
                NpServerObj_SYS_ServerItem serverItem = _m_serverItemMap.get(indexInfo.getServerTypeId());
                if (serverItem == null)
                {
                    continue;
                }
                //构造返回信息
                NP_SYS_ServerItem retItem = new NP_SYS_ServerItem(serverItem.getAreaTag(), serverItem.getGroupId(),
                        indexInfo.getServerLogicId(), indexInfo.getServerTypeId(), serverItem.getServerName(),
                        serverItem.getOnlineStateTypeId(), serverItem.getShowStateTypeId(), serverItem.getIsNew(),
                        serverItem.getStartDate(), serverItem.getExt());
                serverItems.add(retItem);
            }
            //排序返回
            serverItems.sort(Comparator.comparingInt(NP_SYS_ServerItem::getServerTypeId));
            return serverItems;
        } finally
        {
            _unlock();
        }

    }

    private String getString(NpServerObj_SYS_ServerItem _item)
    {
        return String.format("[typeId:%d]\tserverName:[%s]\tonlineTypeId:[%s]\tshowStateTypeId:[%s]\tstartDate:[%s]"
                , _item.getServerTypeId()
                , _item.getServerName()
                , _item.getOnlineStateTypeId()
                , _item.getShowStateTypeId()
                , _item.getStartDate().isEmpty() ? "NULL" : _item.getStartDate()
        );
    }

    private String getString(NpServerObj_SYS_ServerIndexInfo _item)
    {
        return String.format("[logicId:%d-typeId:%d]"
                , _item.getServerLogicId()
                , _item.getServerTypeId()
        );
    }
    
    /**
     * 构造monitor需要的数据
     * @param _obj
     */
    public void makeMonitorBaseInfo(JsonObject _obj)
    {
    	_lock();
    	
    	try
    	{
    		JsonArray serverArray = new JsonArray();
    		for (NpServerObj_SYS_ServerIndexInfo indexInfo : _m_serverIndexMap.values())
            {
                if (indexInfo == null)
                	continue;
                
                JsonObject obj = new JsonObject();
                obj.addProperty("type_id", indexInfo.getServerTypeId());
                obj.addProperty("logic_id", indexInfo.getServerLogicId());

                serverArray.add(obj);
            }
    		
    		_obj.addProperty("us_list", serverArray.toString());
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (NpServerObj_SYS_ServerIndexInfo item : _m_serverIndexMap.values())
        {
            sb.append(getString(item)).append("\n");
        }
        sb.append("\n==============\n");
        for (NpServerObj_SYS_ServerItem item : _m_serverItemMap.values())
        {
            sb.append(getString(item)).append("\n");
        }
        return sb.toString();
    }

}
