package NPUSServer.Dungeon.Evening;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.DungeonObj.EveningDungeon_AttackLog;
import Common.DungeonObj.EveningDungeon_DefeatInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.EveningDungeonDefeatLogBO;

import java.util.ArrayList;
import java.util.List;

public class EveningDungeonAttackLogMgr
{
    private EveningDungeonInfo _m_info;
    private long _m_serial;
    private List<EveningDungeon_AttackLog> _m_attackLogList;
    private List<EveningDungeonDefeatLogBO> _m_defeatLogList;
    private MutexAtom _m_mutex;

    public EveningDungeonAttackLogMgr(EveningDungeonInfo _info)
    {
        _m_info = _info;
        _m_serial = 0;
        _m_attackLogList = new ArrayList<>();
        _m_defeatLogList = new ArrayList<>();
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

    /**
     * 初始化击杀日志
     * @param _logBo
     */
    public void initDefeatLog(EveningDungeonDefeatLogBO _logBo)
    {
        _m_defeatLogList.add(_logBo);
    }

    /**
     * 增加日志
     * @param _userdata
     * @param _data
     */
    public void addAttackLog(NPUSUserData _userdata, EveningDungeonAttackResult _data)
    {
        EveningDungeon_AttackLog log = new EveningDungeon_AttackLog();
        log.setSerial(++_m_serial);
        log.setCid(_userdata.getCid());
        log.setWave(_data.rebornTimes);
        log.setHarmHp(_data.harmHp);
        log.setPlayerName(_userdata.getPlayerComponent().getName());
        log.setIconId(_userdata.getPlayerComponent().getParamV(ENPPlayerParam.ICON));
        log.setTimestamp(CommonFunc.getNowTimeMS());
        log.setAttackHeroId(_data.attackHeroId);  // 设置攻击英雄ID

        _lock();
        try
        {
            _m_attackLogList.add(log);

            //如果日志超过50条，则删除最早的日志
            while (_m_attackLogList.size() > 50)
                _m_attackLogList.remove(0);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加日志
     * @param _userdata
     * @param _timeMs
     */
    public void addDefeatLog(NPUSUserData _userdata, long _timeMs)
    {
        EveningDungeonDefeatLogBO log = new EveningDungeonDefeatLogBO();
        log.setCid(_userdata.getUSServer().getBM(), _userdata.getCid());
        log.setTimeMs(_userdata.getUSServer().getBM(), _timeMs);
        log.insert(_userdata.getUSServer().getBM());

        _lock();
        try
        {
            _m_defeatLogList.add(log);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空日志
     */
    public void clearLog()
    {
        _lock();
        try
        {
            _m_attackLogList.clear();
            _m_serial = 0;

            _m_defeatLogList.clear();
            _m_info.getMgr().getServer().getBM().getBM(EveningDungeonDefeatLogBO.class).delAll();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造日志列表
     * @param _serial
     * @param _needNum
     * @return
     */
    public List<EveningDungeon_AttackLog> makeLogList(long _serial, int _needNum)
    {
        List<EveningDungeon_AttackLog> list = new ArrayList<>();

        //如果序列号大于当前序列号，则返回空
        if (_serial >= _m_serial)
            return list;

        _lock();
        try
        {
            for (int i = _m_attackLogList.size() - 1; i >= 0; --i)
            {
                EveningDungeon_AttackLog log = _m_attackLogList.get(i);
                if (log.getSerial() <= _serial)
                    break;

                list.add(log);

                if (list.size() >= _needNum)
                    break;
            }
        } finally
        {
            _unlock();
        }
        return list;
    }

    /**
     * 构造击杀日志列表
     * @return
     */
    public List<EveningDungeon_DefeatInfo> makeDefeatLogList()
    {
        List<EveningDungeon_DefeatInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (EveningDungeonDefeatLogBO logBO : _m_defeatLogList)
            {
                list.add(new EveningDungeon_DefeatInfo(logBO.getCid(), logBO.getTimeMs()));
            }
        } finally
        {
            _unlock();
        }
        return list;
    }
}
