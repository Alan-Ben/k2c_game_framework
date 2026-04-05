package NPUSServer.NPUSUserMgr.UserComp.InnComp.Station;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.InnObj.Inn_StationInfo;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Inn.RefInnStation;
import NPGameRes.Refs.Inn.RefInnStationLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent.DishBaseProfitInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import USDB.Bo.PlayerInnStationBO;

/**
 * 旅店设施信息
 */
public class InnStationInfo
{
    private final InnStationMgr _m_stationMgr;
    private final RefInnStation _m_refStation;
    private RefInnStationLevel _m_refStationLevel;

    // BO参数对应的成员变量
    private final long _m_dbId;
    private final long _m_stationId;
    private int _m_level;

    public InnStationInfo(InnStationMgr _stationMgr, PlayerInnStationBO _bo, RefInnStation _refStation)
    {
        _m_stationMgr = _stationMgr;
        _m_refStation = _refStation;
        _m_refStationLevel = _m_refStation.getLevelRef(_bo.getLevel());

        // 读取BO参数到成员变量
        _m_dbId = _bo.getId();
        _m_stationId = _bo.getStationId();
        _m_level = _bo.getLevel();
    }

    /**
     * 获取配表对象
     */
    public RefInnStation getRefStation()
    {
        return _m_refStation;
    }

    /**
     * 做菜人气加成
     * @return
     */
    public void dealProfit(DishBaseProfitInfo profitInfo)
    {
        RefInnStationLevel levelRef = _m_refStationLevel;
        if (levelRef == null)
            return;

        profitInfo.addPopularity(levelRef.popularity_add);
        profitInfo.addFitness(levelRef.finesse_add);
        profitInfo.addAffection(levelRef.affection_add);
    }

    /**
     * 获取设施管理器
     */
    public InnStationMgr getMgr()
    {
        return _m_stationMgr;
    }

    /**
     * 获取设施ID
     */
    public long getStationId()
    {
        return _m_stationId;
    }

    /**
     * 获取等级
     */
    public int getLevel()
    {
        return _m_level;
    }

    /**
     * 获取数据库ID
     */
    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 升级设施
     */
    public Result upgrade(NPPlayerContext _context)
    {
        getMgr()._lock();
        try
        {
            if (_m_refStationLevel == null)
                return CommErr.REF_NOT_FOUND;

            // 查询下一等级的配置
            RefInnStationLevel nextLevelRef = _m_refStation.getLevelRef(_m_level + 1);
            if (nextLevelRef == null)
                return InnErr.INN_STATION_LEVEL_REACH_MAX;

            // 检查道具是否足够
            if (!getMgr().getComp().getUserData().hasCostItemList(_m_refStationLevel.upgrade_cost))
                return CommErr.ITEM_NOT_ENOUGH;

            // 记录升级前等级
            int beforeLevel = _m_level;

            // 扣除消耗物品
            if (!getMgr().getComp().getUserData().spendCostItemList(_m_refStationLevel.upgrade_cost, _context))
                return CommErr.CONSUME_FAIL;

            _chgLevel(_m_level + 1, nextLevelRef);

            // 记录日志
            MJEventLog.logInnUpdate(getMgr().getComp().getUserData(), _m_stationId,
                    beforeLevel, _m_level);

            return Result.SUCC;
        } finally
        {
            getMgr()._unlock();
        }
    }

    /**
     * 作弊设置等级
     * @param _level
     */
    public Result cheatSetLevel(int _level)
    {
        getMgr()._lock();
        try
        {
            if (_level <= 0)
                return CommErr.PARAM_ERROR;

            // 查询下一等级的配置
            RefInnStationLevel nextLevelRef = _m_refStation.getLevelRef(_level);
            if (nextLevelRef == null)
                return InnErr.INN_STATION_LEVEL_REACH_MAX;

            _chgLevel(_level, nextLevelRef);

            return Result.SUCC;
        } finally
        {
            getMgr()._unlock();
        }
    }

    /**
     * 修改等级
     * @param _level
     * @param _nextLevelRef
     */
    private void _chgLevel(int _level, RefInnStationLevel _nextLevelRef)
    {
        getMgr()._lock();
        try
        {
            _m_level = _level;
            _m_refStationLevel = _nextLevelRef;

            // 推送到客户端
            _m_stationMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_053_OnInnStationChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("level", _m_level);
            getMgr().getComp().getUserData().getUSServer().getBM().getBM(PlayerInnStationBO.class).update("id", _m_dbId, updateValue);
        } finally
        {
            getMgr()._unlock();
        }
    }

    /**
     * 构造信息
     * @return
     */
    public Inn_StationInfo makeProto()
    {
        Inn_StationInfo info = new Inn_StationInfo();
        info.setStationId(getStationId());
        info.setLevel(getLevel());
        return info;
    }
}
