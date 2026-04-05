package NPUSServer.NPUSUserMgr.UserComp.ArenaComp.FightReport;

import Common.ArenaObj.Arena_BattleReport;
import Common.ArenaObj.Arena_FightBackInfo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.ArenaComponent;
import USDB.Bo.PlayerArenaFightReportBO;

import java.util.ArrayList;
import java.util.List;

public class ArenaFightReportMgr
{
    private ArenaComponent _m_comp;
    private List<ArenaFightReportInfo> _m_reportList;
    private List<ArenaFightReportInfo> _m_canFightBackList;

    public ArenaFightReportMgr(ArenaComponent _comp)
    {
        _m_comp = _comp;
        _m_reportList = new ArrayList<>();
        _m_canFightBackList = new ArrayList<>();
    }

    public ArenaComponent getComp()
    {
        return _m_comp;
    }

    private void _lock()
    {
        _m_comp.getUserData().lockUser();
    }

    private void _unlock()
    {
        _m_comp.getUserData().unlockUser();
    }

    public void init(List<PlayerArenaFightReportBO> _boList)
    {
        for (PlayerArenaFightReportBO bo : _boList)
        {
            ArenaFightReportInfo _info = new ArenaFightReportInfo(this, bo);
            if (bo.getFromCelebrityRand())
            {
                _m_canFightBackList.add(_info);
            } else
            {
                _m_reportList.add(_info);
            }
        }

        //排序
        _m_reportList.sort((o1, o2) -> Long.compare(o2.getDbId(), o1.getDbId()));
        _m_canFightBackList.sort((o1, o2) -> Long.compare(o2.getDbId(), o1.getDbId()));

        //检查是否超过限制
        _checkRemoveOverLimitData();
    }

    /**
     * 检查是否超过限制
     */
    private void _checkRemoveOverLimitData()
    {
        _lock();
        try
        {
            //删除多余的战报
            while (_m_reportList.size() > RefGeneral.Ref().arena_battle_report_limit)
            {
                ArenaFightReportInfo _info = _m_reportList.remove(_m_reportList.size() - 1);
                _info.discard();
            }
            while (_m_canFightBackList.size() > RefGeneral.Ref().arena_fight_back_limit)
            {
                ArenaFightReportInfo _info = _m_canFightBackList.remove(_m_canFightBackList.size() - 1);
                _info.discard();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 新增战报
     * @param _bo
     */
    public void addFightReport(PlayerArenaFightReportBO _bo)
    {
        _lock();
        try
        {
            ArenaFightReportInfo _info = new ArenaFightReportInfo(this, _bo);
            if (_bo.getFromCelebrityRand())
            {
                _m_canFightBackList.add(0, _info);
            } else
            {
                _m_reportList.add(0, _info);
            }

            //检查是否超过限制
            _checkRemoveOverLimitData();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询反击信息
     * @param _dbId
     */
    public ArenaFightReportInfo findFightReport(long _dbId)
    {
        _lock();
        try
        {
            for (ArenaFightReportInfo info : _m_canFightBackList)
            {
                if (info.getDbId() == _dbId)
                    return info;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取反击列表
     */
    public List<Arena_FightBackInfo> getFightBackList()
    {
        _lock();
        try
        {
            List<Arena_FightBackInfo> retList = new ArrayList<>();
            for (ArenaFightReportInfo info : _m_canFightBackList)
            {
                Arena_FightBackInfo fightBackInfo = new Arena_FightBackInfo();
                fightBackInfo.setDbId(info.getDbId());
                fightBackInfo.setShowInfo(info.makeProto());
                fightBackInfo.setHadFightBack(info.isFightBack());
                retList.add(fightBackInfo);
            }
            return retList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取战报列表
     */
    public List<Arena_BattleReport> getReportList()
    {
        _lock();
        try
        {
            List<Arena_BattleReport> retList = new ArrayList<>();
            for (ArenaFightReportInfo info : _m_reportList)
            {
                Arena_BattleReport fightBackInfo = new Arena_BattleReport();
                fightBackInfo.setDbId(info.getDbId());
                fightBackInfo.setShowInfo(info.makeProto());
                retList.add(fightBackInfo);
            }
            return retList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 标记反击已读
     * @param _fightReport
     */
    public void markFightBackRead(ArenaFightReportInfo _fightReport)
    {
        _lock();
        try
        {
            _fightReport.markFightBack();
        } finally
        {
            _unlock();
        }
    }
}
