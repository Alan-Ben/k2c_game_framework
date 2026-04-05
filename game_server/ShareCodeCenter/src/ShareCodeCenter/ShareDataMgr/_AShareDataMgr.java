package ShareCodeCenter.ShareDataMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.CommonCache.TimeKeyLinkList;
import NPCommon.DB.BaseBO;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.ESccParam;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import ShareCodeCenter.EShareCodeServerAsynEnum;
import ShareCodeCenter.SCCParams;
import ShareCodeCenter.ShareCodeCenter;
import ShareCodeDB.ShareCodeDBInitializer;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public abstract class _AShareDataMgr<T extends BaseBO & _ICreateTableSql>
{
    private Map<Integer, T> _m_boInstanceMap;
    private TimeKeyLinkList<Long, ShareDataItem<T>> _m_timeKeyLink;//按时间顺序排序的队列

    public _AShareDataMgr()
    {
        _m_boInstanceMap = new HashMap<>();
        _m_timeKeyLink = new TimeKeyLinkList<>();

        checkUnloadExpiredLoader();
    }

    public _TALMySqlSafeOpDBObj<WCGDBObj> getDBObj()
    {
        return WCGDBFactory.getDbObj(ShareCodeDBInitializer.getDBTag());
    }

    public int getDBTaskThreadIndex()
    {
        return EShareCodeServerAsynEnum.SHARE_CODE_DB.ordinal();
    }

    public ShareDataItem<T> ensureShareDataItem(long _serial)
    {
        ShareDataItem<T> loader = _m_timeKeyLink.computeIfAbsent(_serial, k -> new ShareDataItem<>(this, _serial));
        _m_timeKeyLink.update(loader);
        return loader;
    }

    public void checkUnloadExpiredLoader()
    {
        _unloadExpiredLoader();
        ALSynTaskManager.getInstance().regTask(this::checkUnloadExpiredLoader, 1000);
    }

    /*****
     * 卸载过期的加载器
     */
    private void _unloadExpiredLoader()
    {
        //表示不设置超时时间
        if (-1 == getLoaderExpiredSec())
            return;

        if (_m_timeKeyLink.isEmpty())
            return;

        List<ShareDataItem<T>> expiredLoaders = _m_timeKeyLink.popFirstList(getLoaderExpiredSec());
        if (expiredLoaders == null || expiredLoaders.isEmpty())
            return;
        for (ShareDataItem<T> loader : expiredLoaders)
        {
            //移除后的回调处理
            loader.callbackOnRemoved();
        }

        CommLog.info("[CACHE]:{} unload:{},remain size:{}", getClass().getSimpleName(), expiredLoaders.size(), _m_timeKeyLink.size());
    }

    private int getLoaderExpiredSec()
    {
        return 30 * 60;
    }

    /**
     * 获取关联服务器参数枚举
     */
    public abstract ESccParam getRelativeSccParamType();

    /**
     * 获取表名
     * @return
     */
    public abstract String getTableName();

    /**
     * 获取表字段名
     */
    public abstract String getTableSerialFieldName();

    /**
     * 获取分表长度 即每多少条数据开一张新表
     */
    public abstract int getTableLength();

    /**
     * 创建数据实例
     */
    protected abstract _IALProtocolStructure createDataInstance();

    /**
     * 获取bo中的data数组
     * @param _bo bo实例
     * @return data数组
     */
    protected abstract byte[] _getDataInBo(BaseBO _bo);

    /**
     * 创建数据实例
     * @param _tableIndex 分表索引
     * @param _tableId
     * @param _serial
     * @param _data
     * @return
     */
    protected abstract T createBoInstance(int _tableIndex, long _tableId, long _serial, byte[] _data);

    /**
     * 创造分表索引实例
     * @param _index 分表索引
     */
    protected abstract T _createBaseDBBO(int _index);

    /**
     * 获取对应分表的BO实例
     * @param _index 分表索引
     */
    public synchronized T ensureBaseDBBO(int _index)
    {
        T instance = _m_boInstanceMap.get(_index);
        if (instance == null)
        {
            instance = _createBaseDBBO(_index);
            _m_boInstanceMap.put(_index, instance);
        }
        return instance;
    }

    /**
     * 计算分表索引
     * @param _serial 真实id
     */
    public int getTableIndex(long _serial)
    {
        return (int) (_serial - 1) / getTableLength();//(n - 1) / 10
    }

    /**
     * 计算分表索引
     * @param _serial 真实id
     */
    public long getTableSubIndex(long _serial)
    {
        return (_serial - 1) % getTableLength() + 1;//(n - 1) % 10;
    }

    /**
     * 查询数据
     * @param _serial   序列号
     * @param _callback 回调
     */
    public void loadData(long _serial, HandlerTwo<Boolean, _IALProtocolStructure> _callback)
    {
        ShareDataItem<T> shareDataItem = ensureShareDataItem(_serial);
        shareDataItem.asyncLoad(_callback);
    }

    /**
     * 创建数据
     * @param _data 数据
     * @return 返回序列号
     */
    public long createData(byte[] _data)
    {
        //原序列号
        long preSerial = SCCParams.getInstance().getParam(getRelativeSccParamType());
        //计算原分表的索引
        int preTableIndex = getTableIndex(preSerial);
        //获取新的序列号
        long serial = SCCParams.getInstance().incParam(getRelativeSccParamType());
        //计算分表的索引
        int tableIndex = getTableIndex(serial);

        //如果分表索引发生变化则需要建表
        if (preTableIndex != tableIndex)
        {
            String tableSql = _createBaseDBBO(tableIndex).createTableSql();
            //查询数据
            ALAsynTaskManager.getInstance().regTask(getDBTaskThreadIndex(), () ->
            {
                getDBObj().executeUpdate(tableSql);
            });
        }

        //计算在分表上的id
        long inTableId = getTableSubIndex(serial);

        //插入数据
        T boInstance = createBoInstance(tableIndex, inTableId, serial, _data);
        boInstance.insert(ShareCodeCenter.getInstance().getBM());

        return serial;
    }

    /**
     * 获取分享数据管理器
     * @param _type
     * @return
     */
    public static _AShareDataMgr<?> getShareDataMgr(int _type)
    {
        _AShareDataMgr<?> shareDataMgr = null;

        return shareDataMgr;
    }
}
