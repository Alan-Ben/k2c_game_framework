package NPGameRes;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommFile;

import java.io.File;
import java.util.List;

public class ServerVersionInfo
{
    private static ServerVersionInfo _g_instance = new ServerVersionInfo();

    public static ServerVersionInfo getInstance()
    {
        return _g_instance;
    }

    private String _m_sVersion;
    //是否做了初始化
    private boolean _m_bIsInited = false;
    private boolean _m_bInitRes = false;
    private MutexAtom _m_mutex = new MutexAtom();

    /**
     * 加载服务器资源版本
     * @return
     */
    public boolean initLoadVersion()
    {
        _m_mutex.lock();
        try{
            if(_m_bIsInited)
                return _m_bInitRes;

            _m_bIsInited = true;

            //初始化资源配置
            if (!GameResConf.getInstance().init())
            {
                _m_bInitRes = false;
                return false;
            }

            //读取资源版本
            String filePath = String.format("%s%c%s.txt", GameResConf.getInstance().getRefPath(), File.separatorChar, "version_num");
            List<String> lines = CommFile.getLinesFromFile(filePath);
            if (lines.isEmpty())
            {
                CommLog.error("cant not read server version from file : " + filePath);
                _m_bInitRes = false;
                return false;
            }
            _m_sVersion = lines.get(0);

            //输出资源版本日志
            CommLog.info("ServerVersionInfo loadVersion success, Game Res version:{}", _m_sVersion);

            _m_bInitRes = true;
            return true;
        }finally
        {
            _m_mutex.unlock();
        }
    }

    public String getVersion()
    {
        return _m_sVersion;
    }

    /**
     * 获取待加载配表版本
     * @return
     */
    public String getReadyRefVersion()
    {
        String filePath = String.format("%s%c%s.txt", GameResConf.getInstance().getRefPath(), File.separatorChar, "version_num");
        List<String> lines = CommFile.getLinesFromFile(filePath);
        if (lines.isEmpty())
        {
            CommLog.error("cant not read new server version from file : " + filePath);
            return null;
        }
        return lines.get(0);
    }

    /**
     * 获取待加载配表版本
     * @return
     */
    public void setSucLoadRefVersion(String _refVersion)
    {
        _m_sVersion = _refVersion;

        //输出资源版本日志
        CommLog.info("ServerVersionInfo reloadVersion success, Game Res version:{}", _m_sVersion);
    }
}
