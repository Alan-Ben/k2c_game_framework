package NPCommon.RefData.RefReloader;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import NPCommon.Log.CommLog;
import NPCommon.RefData.AbstractRefDataMgr;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommFile;
import NPCommon.Util.CommonFunc;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Map.Entry;

/**
 * @description: 配表重加载流程管理器
 * @author: ricci
 * @date: 2023-03-14 09:14:53
 */
public class RefDataReloader
{
    /**
     * 配表数据管理器
     */
    private final AbstractRefDataMgr _m_abstractRefDataMgr;
    /**
     * 处理准备配表任务的线程idx
     */
    private int _m_dealReloadThreadIdx;
    /**
     * 配表class类型，配表要更新的内容
     */
    private final ArrayList<RefReloadProcessObj> _m_listRefReloadProcessList;

    /**
     * 是否已经准备好配表更新内容
     */
    private volatile boolean _m_isReadyReload;

    //reload标记，重复的不做处理
    private String _m_sReloadTag;
    /**
     * 正在执行配表准备，或者正在执行配表加载
     */
    private volatile boolean _m_loading;

    /**
     * 当前已准备更新的配表 version
     */
    private String _m_strReadyRefVersion;

    public RefDataReloader(AbstractRefDataMgr _abstractRefDataMgr)
    {
        _m_abstractRefDataMgr = _abstractRefDataMgr;
        _m_listRefReloadProcessList = new ArrayList<>();
        _m_isReadyReload = false;
        _m_dealReloadThreadIdx = -1;
        _m_sReloadTag = "";
    }

    public AbstractRefDataMgr getRefDataMgr()
    {
        return _m_abstractRefDataMgr;
    }

    public boolean isReadyReload()
    {
        return _m_isReadyReload;
    }

    public void setReadyReload(boolean _isReadyReload)
    {
        _m_isReadyReload = _isReadyReload;
    }

    public String getVersion()
    {
        return _m_strReadyRefVersion;
    }

    public void setVersion(String _m_strVersion)
    {
        this._m_strReadyRefVersion = _m_strVersion;
    }

    public boolean isLoading()
    {
        return _m_loading;
    }

    public void setLoading(boolean _loading)
    {
        _m_loading = _loading;
    }

    public int getDealReloadThreadIdx()
    {
        return _m_dealReloadThreadIdx;
    }

    public void setDealReloadThreadIdx(int _dealReloadThreadIdx)
    {
        this._m_dealReloadThreadIdx = _dealReloadThreadIdx;
    }

    /**
     * 处理所有已准备的Process
     * @return 返回是否处理成功
     */
    public boolean dealAllProcess(String _tag)
    {
        if(_m_sReloadTag.equals(_tag))
            return true;

        //标记不一致则直接执行
        _m_sReloadTag = _tag;

        return __dealAllProcess();
    }

    /***
     * 真实的处理reload操作，避免多服同开的时候处理多次reload
     */
    private boolean __dealAllProcess()
    {
        if (isLoading())
        {
            CommLog.error("[RefReload] dealAllProcess has loading pls wait... curVersion:{}", getVersion());
            return false;
        }
        //没有准备好更新数据
        if (!isReadyReload())
        {
            CommLog.error("[RefReload] dealAllProcess not loading but not readyReload... curVersion:{}", getVersion());
            return false;
        }
        long startMs = CommonFunc.getNowTimeMS();
        CommLog.info("=======start RefDataReloader dealAllProcess version:{} ====", getVersion());
        //设置事务进行
        setLoading(true);

        for (RefReloadProcessObj processObj : _m_listRefReloadProcessList)
        {
            try
            {
                //加载数据
                __loadTableFromText(processObj);
            } catch (Exception e)
            {
                e.printStackTrace();
                continue;
            }
            //执行加载完成的指令
            processObj.getRefBase().onLoaded();
        }
        //设置没有在加载
        setLoading(false);
        //设置未准备加载
        setReadyReload(false);

        String readyVersion = getVersion();
        //清空待加载版本
        setVersion("");
        //重新只有refInitDealer
        getRefDataMgr().reload();

        CommLog.info("=======end RefDataReloader dealAllProcess version:{} cost:{} ms"
                , readyVersion, CommonFunc.getNowTimeMS() - startMs);
        return true;
    }

    /**
     * 执行单个 processObj 的配表热更任务
     * @param _processObj 执行任务对象
     */
    @SuppressWarnings("unchecked")
    private <T extends RefBase> void __loadTableFromText(RefReloadProcessObj _processObj)
    {
        RefContainerBase<T> refContainerBase = (RefContainerBase<T>) _processObj.getRefBase();

        ArrayList<T> tmpList = new ArrayList<T>();
        for (RefBase refBase : _processObj.getTableLinesParseObjList())
        {
            if (refBase == null)
            {
                continue;
            }

            T tmpObj = (T) refBase;
            //判断是否替换成功，如未成功则存储到待处理队列，一并放入
            if(!refContainerBase.resetRef(tmpObj))
            {
                tmpList.add(tmpObj);
            }
        }

        //一次性放入
        refContainerBase.put(tmpList);
    }

    /**
     * 使用异步线程去处理 readyAllFiles
     * @param _version 配表版本号x
     */
    public void syncRefReloadThreadDealReadyAllFiles(String _version, _ICallBackBool _callBack)
    {
        if (getDealReloadThreadIdx() < 0)
        {
            CommLog.error("syncRefReloadThreadDealReadyAllFiles not find threadIdx");
            return;
        }
        //使用指定异步线程处理
        ALAsynTaskManager.getInstance().regTask(getDealReloadThreadIdx(), () ->
        {
            if (!readyAllFiles(_version))
            {
                _callBack.onRunOver(false);
                return;
            }
            _callBack.onRunOver(true);
        });
    }

    /**
     * 将当前需要的所有txt载入，转换成ReloadProcess对象存储在，待执行的容器内
     * @return 是否执行成功
     */
    public boolean readyAllFiles(String _version)
    {
        //有事务正在进行
        if (_m_loading)
        {
            CommLog.error("[RefReload] readyAllFiles has loading pls wait... curVersion:{}", getVersion());
            return false;
        }
        CommLog.info("=======start RefDataReloader readyAllFiles version:{} ====", _version);
        long startMs = CommonFunc.getNowTimeMS();
        //设置事务进行
        setLoading(true);
        _m_listRefReloadProcessList.clear();
        //遍历所有配表,同时构造 ReloadProcess 对象
        for (Entry<RefContainerBase<?>, String> entry : getAllRefFilePath().entrySet())
        {
            //磁盘中的txt内容
            String txt = CommFile.getTextFromFile(entry.getValue());
            if (txt == null || txt.isEmpty())
            {
                //没有内容
                CommLog.error("can not find file txt for ref:{}", entry.getKey().getTableName());
                continue;
            }
            //构造Process对象
            RefReloadProcessObj processObj = new RefReloadProcessObj(entry.getKey(), txt);

            _m_listRefReloadProcessList.add(processObj);
        }
        //设置准备好数据的Version
        setVersion(_version);
        //设置已经准备好更新
        setReadyReload(true);
        //设置事务结束
        setLoading(false);

        CommLog.info("=======end RefDataReloader readyAllFiles version:{} cost:{} ms"
                , _version, CommonFunc.getNowTimeMS() - startMs);
        return true;
    }

    /**
     * 获取所有 RefContainerBase 类关联的文件地址
     * @return ArrayList<String>
     */
    public Map<RefContainerBase<?>, String> getAllRefFilePath()
    {
        Map<RefContainerBase<?>, String> allFilePathMap = new HashMap<>();
        for (RefContainerBase<?> containerBase : _m_abstractRefDataMgr.getAllContainerList())
        {
            //获取指定配表对象在硬盘中txt的位置
            String refFilePath = getRefFilePath(containerBase);
            if (refFilePath == null)
            {
                continue;
            }
            allFilePathMap.put(containerBase, refFilePath);
        }
        return allFilePathMap;
    }

    /**
     * 获取指定 RefContainerBase 类关联的文件地址
     * @param _containerBase 指定类
     */
    public String getRefFilePath(RefContainerBase<?> _containerBase)
    {
        if (_containerBase == null)
        {
            return null;
        }
        //管理器中配置的文件根目录
        String refPath = getRefDataMgr().getRefPath();

        //指定对象关联的文件名
        String tableName = _containerBase.getTableName().toLowerCase(Locale.ROOT);

        //通过拼接的方式返回文件的全目录
        return String.format("%s%c%s.txt", refPath, File.separatorChar, tableName);
    }
}
