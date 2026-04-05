package WCGCommon.XYLog;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import WCGCommon.XYLog.Entity.BaseXYLog;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.util.Calendar;
import java.util.HashMap;

public class XYLogManager
{
    public static class XYLogTask implements _IALAsynRunnableTask
    {
        private BaseXYLog _m_blLogObj;

        public XYLogTask(BaseXYLog _logObj)
        {
            _m_blLogObj = _logObj;
        }

        @Override
        public void run()
        {
            //记录日志
            OutputStreamWriter outWriter = XYLogManager.getInstance().popLogWriter(_m_blLogObj.getApi());
            if (null != outWriter)
            {
                try
                {
                    outWriter.write(_m_blLogObj.toString() + "\n");
                } catch (Exception e)
                {
                    e.printStackTrace();
                }
            }
        }
    }

    //日志文件管理对象
    public static class XYLogFileObj
    {
        //存储路径
        private String _m_sOutPutPath;
        //文件名头部
        private String _m_sFileHeader;
        //文件对应时间戳
        private long _m_lFileTag;

        //当前文件总路径
        private String _m_sCurFilePath;
        //文件控制对象
        private FileOutputStream _m_fsFileStream;
        private OutputStreamWriter _m_file;

        /*************
         * 初始化相关配置
         * @param _threadId
         * @param _path
         * @param _fileHeader
         */
        public XYLogFileObj(String _path, String _fileHeader)
        {
            _m_sOutPutPath = _path;
            _m_sFileHeader = _fileHeader;
        }

        public void tryCloseFile(long _timeTag)
        {
            if (_m_lFileTag != _timeTag)
            {
                return;
            }
            //释放原文件
            if (null == _m_file) return;
            try
            {
                if (null != _m_file)
                {
                    _m_file.flush();
                    _m_file.close();
                    _m_file = null;
                }

                if (null != _m_fsFileStream)
                {
                    _m_fsFileStream.flush();
                    _m_fsFileStream.close();
                    _m_fsFileStream = null;
                }
            } catch (Exception e)
            {
                e.printStackTrace();
            }

            //移动文件
            File file = new File(_m_sCurFilePath);
            if (file.exists())
            {
                file.renameTo(new File(_m_sCurFilePath.replaceAll(".temp", ".log")));
                //删除文件
                file.delete();
            }

        }

        /**
         * 获取日志记录对象
         */
        public OutputStreamWriter popLogWriter()
        {
            long timeTag = _getNowTimeTag();

            //判断是否需要创建新文件
            if (_m_lFileTag != timeTag)
            {
                tryCloseFile(_m_lFileTag);
            }

            if (null == _m_file)
            {
                //创建新文件
                try
                {
                    _m_sCurFilePath = _m_sOutPutPath + "/" + _m_sFileHeader + "/" + (timeTag / 10000) + "/" + _m_sFileHeader + "_" + (timeTag / 10000) + "_" + (timeTag % 10000) + ".temp";
                    File file = new File(_m_sCurFilePath);
                    File parentFile = file.getParentFile();
                    if (!parentFile.exists())
                    {
                        if (!parentFile.mkdirs())
                        {
                            CommLog.error("can not create dir for file:{}", _m_sCurFilePath);
                            return null;
                        }
                    }
                    if (!file.exists())
                    {
                        if (!file.createNewFile())
                        {
                            CommLog.error("can not create file:{}", _m_sCurFilePath);
                            return null;
                        }
                    }

                    _m_fsFileStream = new FileOutputStream(file);
                    _m_file = new OutputStreamWriter(_m_fsFileStream, "UTF-8");

                    _m_lFileTag = timeTag;


                    final long checkTag = timeTag;
                    final XYLogFileObj self = this;
                    ALSynTaskManager.getInstance().regTask(new _IALSynTask()
                    {
                        @Override
                        public void run()
                        {
                            XYLogManager.getInstance().tryCloseFile(self, checkTag);
                        }
                    }, 1000 * 60 + 10);
                } catch (Exception e)
                {
                    CommLog.error("create new file:{} failed!", _m_sCurFilePath, e);
                    try
                    {
                        if (null != _m_file)
                        {
                            _m_file.flush();
                            _m_file.close();
                            _m_file = null;
                        }

                        if (null != _m_fsFileStream)
                        {
                            _m_fsFileStream.flush();
                            _m_fsFileStream.close();
                            _m_fsFileStream = null;
                        }
                    } catch (Exception ex)
                    {
                    }
                }
            }

            return _m_file;
        }

        /**
         * 获取当前时间标记
         */
        protected long _getNowTimeTag()
        {
            Calendar calendar = Calendar.getInstance();

            long timeNum = calendar.get(Calendar.YEAR);
            timeNum = (timeNum * 100) + calendar.get(Calendar.MONTH) + 1;
            timeNum = (timeNum * 100) + calendar.get(Calendar.DAY_OF_MONTH);
            timeNum = (timeNum * 100) + calendar.get(Calendar.HOUR_OF_DAY);
            timeNum = (timeNum * 100) + calendar.get(Calendar.MINUTE);

            return timeNum;
        }
    }

    public static XYLogManager getInstance()
    {
        return _instance;
    }

    private static XYLogManager _instance = new XYLogManager();

    //log文件管理表
    private HashMap<String, XYLogFileObj> _m_hmLogFileMap;

    //是否初始化
    private boolean _m_bIsInit;
    //存储路径
    private String _m_sOutPutPath;
    //对应异步线程ID
    private int _m_ThreadId;

    public XYLogManager()
    {
        _m_bIsInit = false;
        _m_ThreadId = 0;
        _m_sOutPutPath = null;
        _m_hmLogFileMap = new HashMap<String, XYLogFileObj>();
    }

    /*************
     * 初始化相关配置
     * @param _threadId
     * @param _path
     * @param _fileHeader
     */
    public void init(int _threadId, String _path)
    {
        if (_m_bIsInit)
            return;

        _m_bIsInit = true;

        _m_ThreadId = _threadId;
        _m_sOutPutPath = _path;
    }

    /**
     * 添加日志
     */
    public void addLog(BaseXYLog _logObj)
    {
        if (!_m_bIsInit)
        {
            System.out.println("Add log when XYLogManager is not inited!");
            return;
        }

        //注册任务
        ALAsynTaskManager.getInstance().regTask(_m_ThreadId, new XYLogTask(_logObj));
    }

    /**
     * 获取日志记录对象
     */
    public OutputStreamWriter popLogWriter(String _api)
    {
        XYLogFileObj fileObj = _m_hmLogFileMap.get(_api);
        if (null == fileObj)
        {
            fileObj = new XYLogFileObj(_m_sOutPutPath, _api);
            _m_hmLogFileMap.put(_api, fileObj);
        }

        return fileObj.popLogWriter();
    }

    protected void tryCloseFile(final XYLogFileObj _fileObj, final long _checkTag)
    {
        ALAsynTaskManager.getInstance().regTask(_m_ThreadId, new _IALAsynRunnableTask()
        {
            @Override
            public void run()
            {
                _fileObj.tryCloseFile(_checkTag);
            }
        });
    }
}
