package NPGameRes.UsHotRefDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CommonEnum.EPlatParamType;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.PHPParam.BSPHPParamMgr;
import NPCommon.Security.MD5;
import NPCommon.Util.CallBack._ICallBack;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommFile;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.Map.Entry;

public class ActivityHotRefGroupInfo
{
    private ActivityHotRefDataMgr _m_mgr;
    private long _m_usGroupId;
    private String _m_resFileName;
    private String _m_resFileMd5;
    private String _m_resFileDir;

    //文件目录
    private String _m_resFilePath;
    //文件内容
    private String _m_resFileContent;
    //解析后的文件数据
    private ActivityHotRefTableGroup _m_tableGroup;

    //是否正在下载
    private boolean _m_isDownloading;
    //是否下载完成
    private boolean _m_hadDownload;
    //是否已激活
    private boolean _m_isActivated;
    //提交次数
    private int _m_submitCount;

    private MutexAtom _m_mutex;

    public ActivityHotRefGroupInfo(ActivityHotRefDataMgr _mgr, long _usGroupId, String _resFileName, String _resFileMd5, String _resFileDir, int _submitCount)
    {
        _m_mgr = _mgr;
        _m_usGroupId = _usGroupId;
        _m_resFileName = _resFileName;
        _m_resFileMd5 = _resFileMd5;
        _m_resFileDir = _resFileDir;
        _m_resFilePath = getMgr().getMainFolderPath() + _usGroupId + "/" +_m_resFileName;
        _m_isDownloading = false;
        _m_hadDownload = false;
        _m_isActivated = false;
        _m_submitCount = _submitCount;
        _m_mutex = new MutexAtom();
    }

    public ActivityHotRefDataMgr getMgr()
    {
        return _m_mgr;
    }

    public long getGroupId()
    {
        return _m_usGroupId;
    }

    public String getResFilePath()
    {
        return _m_resFilePath;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public boolean isDownloading()
    {
        return _m_isDownloading;
    }

    public boolean isHadDownload()
    {
        return _m_hadDownload;
    }

    public int getSubmitCount()
    {
        return _m_submitCount;
    }

    /**
     * 从本地文件初始化
     * @param _fileData
     */
    public void initFileFromLocal(String _fileData)
    {
        _lock();
        try{
            // 解析并验证文件数据
            ActivityHotRefTableGroup tableGroup = parseTableGroup(_fileData);
            if (tableGroup == null)
            {
                CommLog.error("ActivityHotRefGroupInfo initFileFromLocal file:{} failed, parse or validation failed", _m_resFileName);
                return;
            }

            _m_hadDownload = true;
            _m_resFileContent = _fileData;
            _m_tableGroup = tableGroup;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 异步下载热更文件
     */
    public void startAsyncDownload(_ICallBack _callback)
    {
        // 判断热更文件下载地址是否正确
        String hotRefUrlBase = BSPHPParamMgr.getInstance().getParam(EPlatParamType.HOT_REF_URL_BASE);

        _lock();
        try{
            //判断是否已经下载完成
            if (_m_hadDownload)
                return;

            if (_m_isDownloading)
                return;

            _m_isDownloading = true;

            // 判断热更文件下载地址是否正确
            if (hotRefUrlBase == null || hotRefUrlBase.isEmpty())
            {
                CommLog.warn("ActivityHotRefGroupInfo startAsyncDownload hotRefUrlBase is empty, delay download hot ref file in 3 seconds");
                _m_isDownloading = false;
                ALSynTaskManager.getInstance().regTask(() -> startAsyncDownload(_callback), 3000);
                return;
            }

            //下载地址
            String urlPath = hotRefUrlBase + _m_resFileDir + _m_resFileName;
            CommLog.info("ActivityHotRefGroupInfo start download hot ref file from url:{}, usGroupId:{}", urlPath, _m_usGroupId);

            _m_mgr.getActivityRefFileDownloadFunc().downloadFile(urlPath, new _ICallBackResultT<String>()
            {
                @Override
                public void onRunOver(Result _result, String _fileData)
                {
                    _lock();
                    try{
                        if (!_result.isSucc())
                        {
                            getMgr().getDDAlert().err("ActivityHotRefGroupInfo.startAsyncDownload",
                                    "ActivityHotRefGroupInfo download file failed, usGroupId={}, fileName={}, urlPath={}, result={}",
                                    _m_usGroupId, _m_resFileName, urlPath, _result);
                            _m_isDownloading = false;
                            return;
                        }

                        CommLog.info("ActivityHotRefGroupInfo download file:{} success, usGroupId:{}", urlPath, _m_usGroupId);

                        // 校验文件MD5值
                        String fileDataMd5 = MD5.md5(_fileData);
                        if (!fileDataMd5.equals(_m_resFileMd5))
                        {
                            getMgr().getDDAlert().err("ActivityHotRefGroupInfo.startAsyncDownload",
                                    "ActivityHotRefGroupInfo download file MD5 not match, usGroupId={}, fileName={}, urlPath={}, expectMd5={}, actualMd5={}",
                                    _m_usGroupId, _m_resFileName, urlPath, _m_resFileMd5, fileDataMd5);
                            _m_isDownloading = false;
                            return;
                        }

                        // 解析并验证文件数据
                        ActivityHotRefTableGroup tableGroup = parseTableGroup(_fileData);
                        if (tableGroup == null)
                        {
                            getMgr().getDDAlert().err("ActivityHotRefGroupInfo.startAsyncDownload",
                                    "ActivityHotRefGroupInfo parse file data failed, usGroupId={}, fileName={}, urlPath={}",
                                    _m_usGroupId, _m_resFileName, urlPath);
                            _m_isDownloading = false;
                            return;
                        }

                        _m_isDownloading = false;
                        _m_hadDownload = true;
                        _m_resFileContent = _fileData;
                        _m_tableGroup = tableGroup;

                        CommLog.info("ActivityHotRefGroupInfo startAsyncDownload download and parse file success, usGroupId={}, fileName={}",
                                _m_usGroupId,  _m_resFileName);

                        try
                        {
                            CommFile.WriteWithMkdirs(_m_resFilePath, _fileData);
                        } catch (Exception e)
                        {
                            getMgr().getDDAlert().err("ActivityHotRefGroupInfo.startAsyncDownload",
                                    "ActivityHotRefGroupInfo write file to local failed, usGroupId={}, fileName={}, filePath={}, error={}",
                                    _m_usGroupId, _m_resFileName, _m_resFilePath, e.getMessage());
                        }

                        if (_callback != null)
                            _callback.onRunOver();
                    }finally
                    {
                        _unlock();
                    }
                }
            });
        }finally
        {
            _unlock();
        }
    }

    /**
     * 解析并验证文件数据
     *
     * 执行流程：
     * 1. 检查文件内容不为空
     * 2. 解析 JSON 根对象
     * 3. 遍历每个表，同时进行验证和数据提取
     * 4. 验证 fields 和 data 格式
     * 5. 验证字段数量匹配
     * 6. 返回解析结果或 null（验证失败时）
     *
     * @return ActivityHotRefTableGroup 解析成功的数据，失败返回 null
     */
    public ActivityHotRefTableGroup parseTableGroup(String _fileContent)
    {
        // 检查文件内容不为空
        if (_fileContent == null || _fileContent.isEmpty())
        {
            CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - file content validation failed: content is null or empty, usGroupId={}, fileName={}",
                    _m_usGroupId, _m_resFileName);
            return null;
        }

        try
        {
            // 解析 JSON 根对象
            JsonParser parser = new JsonParser();
            JsonObject rootObj = parser.parse(_fileContent).getAsJsonObject();
            if (rootObj == null)
            {
                CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - JSON parse failed: root object is null (invalid JSON format), usGroupId={}, fileName={}",
                        _m_usGroupId, _m_resFileName);
                return null;
            }

            ActivityHotRefTableGroup fileData = new ActivityHotRefTableGroup();

            // 遍历每个表，同时进行验证和数据提取
            for (Entry<String, JsonElement> entry : rootObj.entrySet())
            {
                String tableName = entry.getKey();
                JsonObject tableObj = entry.getValue().getAsJsonObject();

                // 验证：表对象有效性
                if (tableObj == null)
                {
                    CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table validation failed: table '{}' is not a valid JSON object, usGroupId={}, fileName={}",
                            tableName, _m_usGroupId, _m_resFileName);
                    return null;
                }

                // 验证：fields 字段
                if (!tableObj.has("fields"))
                {
                    CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table structure validation failed: table '{}' missing required 'fields' property, usGroupId={}, fileName={}",
                            tableName, _m_usGroupId, _m_resFileName);
                    return null;
                }

                JsonArray fieldsArray = tableObj.getAsJsonArray("fields");
                if (fieldsArray == null || fieldsArray.size() == 0)
                {
                    CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table structure validation failed: table '{}' has empty or invalid 'fields' array, usGroupId={}, fileName={}",
                            tableName, _m_usGroupId, _m_resFileName);
                    return null;
                }

                int fieldCount = fieldsArray.size();

                // 验证：data 字段
                if (!tableObj.has("data"))
                {
                    CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table structure validation failed: table '{}' missing required 'data' property, usGroupId={}, fileName={}",
                            tableName, _m_usGroupId, _m_resFileName);
                    return null;
                }

                JsonArray dataArray = tableObj.getAsJsonArray("data");
                if (dataArray == null)
                {
                    CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table structure validation failed: table '{}' has invalid 'data' array, usGroupId={}, fileName={}",
                            tableName, _m_usGroupId, _m_resFileName);
                    return null;
                }

                // 创建表数据对象
                ActivityHotRefTableInfo tableData = new ActivityHotRefTableInfo(tableName);

                // 解析 fields
                for (int i = 0; i < fieldsArray.size(); i++)
                {
                    tableData.addField(fieldsArray.get(i).getAsString());
                }

                // 解析并验证 data
                for (int i = 0; i < dataArray.size(); i++)
                {
                    JsonArray rowArray = dataArray.get(i).getAsJsonArray();

                    // 验证：行数据格式
                    if (rowArray == null)
                    {
                        CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table data validation failed: table '{}' row {} is not a valid array, usGroupId={}, fileName={}",
                                tableName, i, _m_usGroupId, _m_resFileName);
                        return null;
                    }

                    // 验证：字段数量匹配
                    if (rowArray.size() != fieldCount)
                    {
                        CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - table data validation failed: table '{}' row {} field count mismatch" +
                                        ", expected {} fields but got {} fields, usGroupId={}, fileName={}",
                                tableName, i, fieldCount, rowArray.size(), _m_usGroupId, _m_resFileName);
                        return null;
                    }

                    // 解析行数据
                    List<String> rowData = new ArrayList<>();
                    for (int j = 0; j < rowArray.size(); j++)
                    {
                        rowData.add(rowArray.get(j).getAsString());
                    }
                    tableData.addDataRow(rowData);
                }

                fileData.addTable(tableData);
            }

            CommLog.info("ActivityHotRefGroupInfo.parseTableGroup - parse success: parsed {} tables, usGroupId={}, fileName={}",
                    fileData.getTableCount(), _m_usGroupId, _m_resFileName);
            return fileData;
        }
        catch (Exception e)
        {
            CommLog.error("ActivityHotRefGroupInfo.parseTableGroup - JSON parse exception: exception occurred during parsing, usGroupId={}, fileName={}",
                    _m_usGroupId, _m_resFileName, e);
            return null;
        }
    }

    /**
     * 获取解析后的文件数据
     *
     * @return ActivityHotRefFileData 对象，如果未解析则返回null
     */
    public ActivityHotRefTableGroup getFileData()
    {
        _lock();
        try{
            return _m_tableGroup;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取是否已激活
     *
     * @return true表示已激活，false表示未激活
     */
    public boolean isActivated()
    {
        _lock();
        try{
            return _m_isActivated;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 激活热更配表
     *
     * 执行流程：
     * 1. 检查是否已下载完成
     * 2. 检查是否已激活
     * 3. 调用 ActivityHotRefLoaderMgr 加载配表数据
     * 4. 标记为已激活
     *
     * @param _isForce 是否强制激活
     * @return true表示激活成功，false表示激活失败
     */
    public boolean activate(boolean _isForce)
    {
        _lock();
        try{
            // 检查是否已激活
            if (_m_isActivated && !_isForce)
                return true;

            // 检查是否已下载完成
            if (!_m_hadDownload)
            {
                CommLog.error("ActivityHotRefGroupInfo.activate - activate failed: file not downloaded yet, usGroupId={}, fileName={}",
                        _m_usGroupId, _m_resFileName);
                return false;
            }

            // 检查数据是否已解析
            if (_m_tableGroup == null)
            {
                CommLog.error("ActivityHotRefGroupInfo.activate - activate failed: table group is null (parse failed), usGroupId={}, fileName={}",
                        _m_usGroupId, _m_resFileName);
                return false;
            }

            // 调用加载器管理器加载配表数据
            int successCount = ActivityHotRefLoaderMgr.getInstance().loadGroupTables(this);
            if (successCount <= 0)
            {
                CommLog.error("activate failed, load tables failed, groupId={}, successCount={}",
                        _m_usGroupId, successCount);
                return false;
            }

            // 标记为已激活
            _m_isActivated = true;

            CommLog.info("ActivityHotRefGroupInfo.activate - activate done: usGroupId={}, fileName={}",
                    _m_usGroupId, _m_resFileName);
            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 释放文件资源并删除本地文件
     *
     * 功能描述：
     * 清理文件占用的资源，删除已下载的本地文件
     */
    public void dispose()
    {
        _lock();
        try{
            // 删除本地文件
            if (_m_resFilePath != null && !_m_resFilePath.isEmpty() && _m_hadDownload)
            {
                File localFile = new File(_m_resFilePath);
                if (localFile.exists())
                {
                    try
                    {
                        if (localFile.delete())
                        {
                            CommLog.info("ActivityHotRefFileInfo dispose delete file success, file:{}",
                                    _m_resFilePath);
                        }
                        else
                        {
                            CommLog.warn("ActivityHotRefFileInfo dispose delete file failed, file:{}",
                                    _m_resFilePath);
                        }
                    }
                    catch (Exception e)
                    {
                        CommLog.warn("ActivityHotRefFileInfo dispose delete file exception, file:{}, error:{}",
                                _m_resFilePath, e.getMessage());
                    }
                }
            }

            // 重置状态
            _m_hadDownload = false;
            _m_isActivated = false;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 更新资源文件信息
     * @param _resFileName
     * @param _resFileMd5
     * @param _resFileDir
     * @param _submitCount
     */
    public void updateResFileInfo(String _resFileName, String _resFileMd5, String _resFileDir, int _submitCount)
    {
        _lock();
        try{
            boolean oriIsActivated = _m_isActivated;

            // 尝试释放资源
            dispose();

            _m_resFileName = _resFileName;
            _m_resFileMd5 = _resFileMd5;
            _m_resFileDir = _resFileDir;
            _m_resFilePath = getMgr().getMainFolderPath() + _m_usGroupId + "/" +_m_resFileName;
            _m_submitCount = _submitCount;

            ALSynTaskManager.getInstance().regTask(() ->
                    startAsyncDownload(new _ICallBack()
                    {
                        @Override
                        public void onRunOver()
                        {
                            if (!oriIsActivated)
                                return;

                            activate(true);
                        }
                    }));
        } finally
        {
            _unlock();
        }
    }
}
