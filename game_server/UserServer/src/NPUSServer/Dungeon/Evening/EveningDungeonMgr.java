package NPUSServer.Dungeon.Evening;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import USDB.Bo.EveningDungeonBO;
import USDB.Bo.EveningDungeonDefeatLogBO;

import java.util.List;

public class EveningDungeonMgr
{
    private NPUserServer _m_server;
    private EveningDungeonInfo _m_info;

    public EveningDungeonMgr(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    public EveningDungeonInfo getInfo()
    {
        return _m_info;
    }

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        //初始化晚间副本数据
        if (!__initDungeonInfo())
            return false;

        //初始化晚间副本日志
        if (!__initDungeonLog())
            return false;

        return true;
    }

    /**
     * 初始化晚间副本数据
     * @return
     */
    private boolean __initDungeonInfo()
    {
        List<EveningDungeonBO> boList = getServer().getBM().getBM(EveningDungeonBO.class).s_findAll();
        if (boList == null)
            return false;

        if (boList.isEmpty())
        {
            EveningDungeonBO bo = new EveningDungeonBO();
            bo.setRoundPreviewTimeMs(getServer().getBM(), -1);
            bo.setRoundStartTimeMs(getServer().getBM(), -1);
            bo.setRoundEndTimeMs(getServer().getBM(), -1);
            bo.setRebornTimes(getServer().getBM(), -1);
            bo.setBaseHp(getServer().getBM(), RefGeneral.Ref().evening_dungeon_boss_initial_blood);
            bo.setTotalHp(getServer().getBM(), 0);
            bo.setDeductedHp(getServer().getBM(), 0);
            bo.insert(getServer().getBM());

            _m_info = new EveningDungeonInfo(EveningDungeonMgr.this, bo);
        }else
        {
            _m_info = new EveningDungeonInfo(EveningDungeonMgr.this, boList.get(0));
        }

        return true;
    }

    /**
     * 初始化晚间副本日志
     * @return
     */
    private boolean __initDungeonLog()
    {
        List<EveningDungeonDefeatLogBO> boList = getServer().getBM().getBM(EveningDungeonDefeatLogBO.class).s_findAll();
        if (boList == null)
            return false;

        for (EveningDungeonDefeatLogBO logBo : boList)
        {
            _m_info.getAttackLogMgr().initDefeatLog(logBo);
        }

        return true;
    }

    /**
     * tick
     */
    public void tick1Sec()
    {
        //tick逻辑
        _m_info.tick1Sec();
    }

    public void startTick()
    {
        ALSynTaskManager.getInstance().regTask(new EveningDungeonTickTask(getServer()), 1000);
    }

    @Override
    public String toString()
    {
        return _m_info.toString();
    }
}
