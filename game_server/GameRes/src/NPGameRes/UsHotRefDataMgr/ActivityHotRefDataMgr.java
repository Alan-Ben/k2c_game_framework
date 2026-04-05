package NPGameRes.UsHotRefDataMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.DDAlert.DDAlert;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommFile;

import java.io.File;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Set;

public class ActivityHotRefDataMgr
{
    private static ActivityHotRefDataMgr _g_instance = new ActivityHotRefDataMgr();

    public static ActivityHotRefDataMgr getInstance()
    {
        return _g_instance;
    }

    private Map<Long, ActivityHotRefGroupInfo> _m_hotRefDataMap;
    private String _m_mainFolderPath = "./ref_patch/";
    private boolean _m_isInited = false;
    private MutexObject _m_mutex;
    private _IActivityRefFileDownloadTask _m_activityRefFileDownloadFunc;
    private DDAlert _m_ddAlert;

    public ActivityHotRefDataMgr()
    {
        _m_hotRefDataMap = new HashMap<>();
        _m_mutex = new MutexObject();
    }

    public String getMainFolderPath()
    {
        return _m_mainFolderPath;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public DDAlert getDDAlert()
    {
        return _m_ddAlert;
    }

    public _IActivityRefFileDownloadTask getActivityRefFileDownloadFunc()
    {
        return _m_activityRefFileDownloadFunc;
    }

    /**
     * 从本地文件夹初始化管理器
     * <p>
     * 功能描述：
     * 扫描本地热更文件夹，对于已通过regHotRefGroup注册的分组，
     * 检查本地文件夹中的文件并更新下载状态，避免重复下载已存在的文件
     * <p>
     * 注意：此方法应在其他管理器完成regHotRefGroup注册后调用
     */
    public void initFromLocalFolder(_IActivityRefFileDownloadTask _activityRefFileDownloadFunc, DDAlert _ddAlert)
    {
        _lock();
        try
        {
            if (_m_isInited)
                return;

            _m_isInited = true;
            _m_activityRefFileDownloadFunc = _activityRefFileDownloadFunc;
            _m_ddAlert = _ddAlert;

            // 收集已注册的分组ID集合
            Set<Long> registeredGroupIds = new HashSet<>();

            // 遍历检查已注册的分组是否已经下载完成
            for (Entry<Long, ActivityHotRefGroupInfo> entry : _m_hotRefDataMap.entrySet())
            {
                ActivityHotRefGroupInfo groupInfo = entry.getValue();
                registeredGroupIds.add(groupInfo.getGroupId());

                // 文件内容
                String fileContent = CommFile.getTextFromFile(groupInfo.getResFilePath());
                if (fileContent != null)
                    groupInfo.initFileFromLocal(fileContent);

                if (!groupInfo.isHadDownload())
                    ALSynTaskManager.getInstance().regTask(() -> groupInfo.startAsyncDownload(null));
            }

            // 清理未注册分组的本地文件夹
            cleanupUnregisteredFolders(registeredGroupIds);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清理未注册分组的本地空白文件夹
     * <p>
     * 功能描述：
     * 扫描热更主文件夹，删除所有未在已注册分组中且为空的文件夹，
     * 以释放磁盘空间并保持文件系统整洁
     *
     * 执行流程：
     * 1. 检查主文件夹是否存在
     * 2. 遍历主文件夹下的所有子目录
     * 3. 对于目录名为数字且未注册的分组ID，检查是否为空文件夹
     * 4. 删除空文件夹并记录日志
     *
     * @param _registeredGroupIds 已注册的分组ID集合
     */
    private void cleanupUnregisteredFolders(Set<Long> _registeredGroupIds)
    {
        // 检查主文件夹是否存在
        File mainFolder = new File(_m_mainFolderPath);
        if (!mainFolder.exists() || !mainFolder.isDirectory())
        {
            CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - main folder not found, skip cleanup, mainFolderPath={}", _m_mainFolderPath);
            return;
        }

        // 遍历主文件夹下的所有子目录
        File[] subFolders = mainFolder.listFiles();
        if (subFolders == null || subFolders.length == 0)
        {
            CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - no subfolders found, skip cleanup");
            return;
        }

        int cleanupCount = 0;
        for (File subFolder : subFolders)
        {
            // 跳过非目录文件
            if (!subFolder.isDirectory())
                continue;

            try
            {
                // 尝试将文件夹名解析为分组ID
                long folderId = Long.parseLong(subFolder.getName());

                // 如果该ID未注册，检查是否为空文件夹并删除
                if (!_registeredGroupIds.contains(folderId))
                {
                    // 检查文件夹是否为空
                    File[] files = subFolder.listFiles();
                    boolean isEmpty = (files == null || files.length == 0);

                    if (isEmpty)
                    {
                        boolean deleteSuccess = subFolder.delete();
                        if (deleteSuccess)
                        {
                            cleanupCount++;
                            CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - delete empty folder success, folderId={}, folderPath={}",
                                    folderId, subFolder.getAbsolutePath());
                        }
                        else
                        {
                            CommLog.warn("ActivityHotRefDataMgr.cleanupUnregisteredFolders - delete empty folder failed, folderId={}, folderPath={}",
                                    folderId, subFolder.getAbsolutePath());
                        }
                    }
                    else
                    {
                        CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - skip non-empty folder, folderId={}, folderPath={}, fileCount={}",
                                folderId, subFolder.getAbsolutePath(), files.length);
                    }
                }
            }
            catch (NumberFormatException e)
            {
                // 文件夹名不是数字，跳过（可能是其他用途的文件夹）
                CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - skip non-numeric folder, folderName={}", subFolder.getName());
            }
        }

        if (cleanupCount > 0)
        {
            CommLog.info("ActivityHotRefDataMgr.cleanupUnregisteredFolders - cleanup completed, cleanupCount={}", cleanupCount);
        }
    }

    /**
     * 获取指定分组的热更配表配置信息
     * @param groupId 配置组ID
     * @return 配置组信息对象，如果不存在则返回null
     */
    public ActivityHotRefGroupInfo lookupHotRefGroupInfo(long groupId)
    {
        _lock();
        try
        {
            return _m_hotRefDataMap.get(groupId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册热更配表配置组
     * <p>
     * 功能描述：
     * 将指定分组ID的热更配表文件信息注册到管理器中，用于后续的配表数据管理
     * @param _groupId     配置组ID，用于标识不同的配表分组
     * @param _resFileName 配表文件名
     * @param _resFileMd5  配表文件MD5
     * @param _resFileDir
     * @param _submitCount
     */
    public void regHotRefGroup(long _groupId, String _resFileName, String _resFileMd5, String _resFileDir, int _submitCount, boolean _isInit)
    {
        _lock();
        try{
            // 已经注册过 则比较submitCount 如果更大则更新
            ActivityHotRefGroupInfo activityHotRefGroupInfo = lookupHotRefGroupInfo(_groupId);
            if (activityHotRefGroupInfo != null)
            {
                if (_submitCount < activityHotRefGroupInfo.getSubmitCount())
                    return;

                activityHotRefGroupInfo.updateResFileInfo(_resFileName, _resFileMd5, _resFileDir, _submitCount);

            }else
            {
                // 创建配置组信息对象
                ActivityHotRefGroupInfo groupInfo = new ActivityHotRefGroupInfo(this, _groupId, _resFileName, _resFileMd5, _resFileDir, _submitCount);

                // 注册到管理器的映射表中
                _m_hotRefDataMap.put(_groupId, groupInfo);

                // 如果不是初始化阶段，立即开始异步下载
                if (!_isInit)
                    ALSynTaskManager.getInstance().regTask(() -> groupInfo.startAsyncDownload(null));

                CommLog.info("ActivityHotRefDataMgr reg hotRefGroup success, groupId:{}", _groupId);
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 注销热更配表配置组
     * <p>
     * 功能描述：
     * 移除指定分组的热更配表配置，清理相关资源
     * @param _groupId 配置组ID
     */
    public void unregHotRefGroup(long _groupId)
    {
        _lock();
        try{
            ActivityHotRefGroupInfo groupInfo = _m_hotRefDataMap.remove(_groupId);

            if (groupInfo == null)
                return;

            // 清理配置组资源
            groupInfo.dispose();

            CommLog.info("ActivityHotRefDataMgr unreg hotRefGroup success, groupId:{}", _groupId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 激活指定分组的热更配表
     *
     * 执行流程：
     * 1. 查找指定分组的配置信息
     * 2. 调用分组的 activate() 方法激活配表
     * 3. 返回激活结果
     *
     * @param _groupId 配置组ID
     * @return true表示激活成功，false表示激活失败（分组不存在或激活失败）
     */
    public boolean activateHotRefGroup(long _groupId)
    {
        _lock();
        try{
            // 查找分组配置信息
            ActivityHotRefGroupInfo groupInfo = _m_hotRefDataMap.get(_groupId);
            if (groupInfo == null)
            {
                CommLog.error("ActivityHotRefDataMgr activate hotRefGroup failed, group not found, groupId={}", _groupId);
                return false;
            }

            // 激活配表
            boolean result = groupInfo.activate(false);
            if (!result)
            {
                CommLog.error("ActivityHotRefDataMgr activate hotRefGroup failed, groupId={}", _groupId);
            }

            return result;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 重新激活已激活分组的热更配表
     */
    public void reActivateAllHotRefGroups()
    {
        _lock();
        try{
            for (Entry<Long, ActivityHotRefGroupInfo> entry : _m_hotRefDataMap.entrySet())
            {
                ActivityHotRefGroupInfo groupInfo = entry.getValue();
                if (groupInfo.isActivated())
                {
                    boolean result = groupInfo.activate(true);
                    if (result)
                    {
                        CommLog.info("re-activate hotRefGroup success, groupId={}", groupInfo.getGroupId());
                    }
                    else
                    {
                        CommLog.error("re-activate hotRefGroup failed, groupId={}", groupInfo.getGroupId());
                    }
                }
            }
        }finally
        {
            _unlock();
        }
    }
}
