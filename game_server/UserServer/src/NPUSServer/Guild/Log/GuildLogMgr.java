package NPUSServer.Guild.Log;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildEnum.EGuildLogType;
import Common.GuildObj.Guild_LogInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import USDB.Bo.GuildLogBO;

import java.util.ArrayList;
import java.util.List;

public class GuildLogMgr
{
    private GuildInfo _m_guildInfo;
    private List<GuildLogInfo> _m_logList;
    private MutexAtom _m_mutex;

    public GuildLogMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_logList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    /**
     * 初始化申请
     * @param _bo
     */
    public void initLog(GuildLogBO _bo)
    {
        GuildLogInfo logInfo = new GuildLogInfo(_bo);
        _m_logList.add(logInfo);
    }

    /**
     * 添加日志
     * @param _msgType
     * @param _proto
     */
    public void addLog(EGuildLogType _msgType, _IALProtocolStructure _proto)
    {
        GuildLogBO bo = new GuildLogBO();
        bo.setGuildId(_m_guildInfo.getGuildMgr().getServer().getBM(), _m_guildInfo.getGuildId());
        bo.setType(_m_guildInfo.getGuildMgr().getServer().getBM(), _msgType.ordinal());
        bo.setData(_m_guildInfo.getGuildMgr().getServer().getBM(), _proto.makePackage().array());
        bo.setSendTimeMs(_m_guildInfo.getGuildMgr().getServer().getBM(), CommonFunc.getNowTimeMS());
        bo.insert(_m_guildInfo.getGuildMgr().getServer().getBM());

        _lock();
        try
        {
            _m_logList.add(new GuildLogInfo(bo));

            while (_m_logList.size() > 500)
            {
                GuildLogInfo logInfo = _m_logList.remove(0);
                logInfo.discard(this);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取日志列表
     * @return
     */
    public List<Guild_LogInfo> makeProto(long _lastDbId, int _num)
    {
        List<Guild_LogInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (int i = _m_logList.size() - 1; i >= 0; --i)
            {
                GuildLogInfo log = _m_logList.get(i);
                if (log.getDbId() <= _lastDbId)
                    break;

                list.add(log.makeProto());

                if (list.size() >= _num)
                    break;
            }
        } finally
        {
            _unlock();
        }
        return list;
    }

    public void discard()
    {
        _lock();
        try{
            _m_logList.clear();
        }finally
        {
            _unlock();
        }

        getGuildInfo().getGuildMgr().getServer().getBM().getBM(GuildLogBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
    }
}
