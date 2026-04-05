package NPUSServer.NPUSUserMgr.UserComp.GuildCooperateComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GuildCooperateObj.GuildCooperate_HeroUseInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.GuildCooperateErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import USDB.Bo.PlayerGuildCooperateHeroUseRecordBO;

public class GuildCooperateHeroUseRecord
{
    private GuildCooperateComponent _m_comp;

    private long _m_dbId;
    private long _m_heroId;
    private int _m_useCount;
    private int _m_recoverCount;
    private long _m_lastRefreshTimeMs;

    public GuildCooperateHeroUseRecord(GuildCooperateComponent _comp, PlayerGuildCooperateHeroUseRecordBO _bo)
    {
        _m_comp = _comp;
        _m_dbId = _bo.getId();
        _m_heroId = _bo.getHeroId();
        _m_useCount = _bo.getUseCount();
        _m_recoverCount = _bo.getRecoverCount();
        _m_lastRefreshTimeMs = _bo.getLastRefreshTimeMs();
    }

    /**
     * 获取大臣ID
     * @return 大臣ID
     */
    public long getHeroId()
    {
        return _m_heroId;
    }

    private void _lock()
    {
        _m_comp.getUserData().lockUser();
    }

    private void _unlock()
    {
        _m_comp.getUserData().unlockUser();
    }

    /**
     * 是否可以使用
     * @return
     */
    public boolean canUse()
    {
        _lock();
        try{
            return _m_recoverCount + 1 > _m_useCount;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否需要刷新
     */
    private boolean _checkNeedRefresh()
    {
        _lock();
        try{
            //检查是否需要刷新
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
            if (todayZeroClockMS > _m_lastRefreshTimeMs)
            {
                _m_useCount = 0;
                _m_recoverCount = 0;
                _m_lastRefreshTimeMs = todayZeroClockMS;

                return true;
            }

            return false;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 记录使用
     * @return
     */
    public Result recordUse(BM _bmObj)
    {
        _lock();
        try{
            //检查是否需要刷新
            boolean needRefresh = _checkNeedRefresh();

            //检查是否可以使用
            if (!canUse())
                return GuildCooperateErr.HERO_ALREADY_USED;

            //增加计数
            _m_useCount++;

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("use_count", _m_useCount);
            if (needRefresh)
            {
                updateValue.addValueObj("recover_count", _m_recoverCount);
                updateValue.addValueObj("last_refresh_time_ms", _m_lastRefreshTimeMs);
            }
            _bmObj.getBM(PlayerGuildCooperateHeroUseRecordBO.class).update("id", _m_dbId, updateValue);

            _onDataChgPush();

            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 恢复大臣使用计数
     * @param _bmObj
     * @return
     */
    public Result recoverUse(BM _bmObj)
    {
        _lock();
        try{
            //检查是否需要刷新
            boolean needRefresh = _checkNeedRefresh();

            if (canUse())
                return GuildCooperateErr.HERO_NOT_USED;

            _m_recoverCount++;

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("recover_count", _m_recoverCount);
            if (needRefresh)
            {
                updateValue.addValueObj("use_count", _m_useCount);
                updateValue.addValueObj("last_refresh_time_ms", _m_lastRefreshTimeMs);
            }
            _bmObj.getBM(PlayerGuildCooperateHeroUseRecordBO.class).update("id", _m_dbId, updateValue);

            _onDataChgPush();

            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 回滚大臣使用记录
     * 
     * 用于当记录使用后，后续操作失败时减少使用次数。
     * 执行流程：
     * 1. 验证使用次数大于0（确实有使用记录可以回滚）
     * 2. 减少使用次数并更新数据库
     * 3. 推送数据变更通知
     * 
     * @param _bmObj 数据库管理器
     * @return 回滚结果
     */
    public void rollbackUse(BM _bmObj)
    {
        _lock();
        try{
            // 验证是否有使用记录可以回滚
            if (_m_useCount <= 0)
                return;

            // 减少使用次数
            _m_useCount--;

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("use_count", _m_useCount);
            _bmObj.getBM(PlayerGuildCooperateHeroUseRecordBO.class).update("id", _m_dbId, updateValue);

            // 推送数据变更通知
            _onDataChgPush();
        }finally
        {
            _unlock();
        }
    }

    /**
     * 数据变更推送
     */
    private void _onDataChgPush()
    {
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_032_GuildOp.make_079_OnHeroUseInfoChange(makeProto()));
    }

    /**
     * 构造协议
     * @return
     */
    public GuildCooperate_HeroUseInfo makeProto()
    {
        _lock();
        try{
            GuildCooperate_HeroUseInfo info = new GuildCooperate_HeroUseInfo();
            info.setHeroId(_m_heroId);
            info.setUseCount(_m_useCount);
            info.setRecoveredCount(_m_recoverCount);
            info.setLastRefreshTimeMs(_m_lastRefreshTimeMs);
            return info;
        }finally
        {
            _unlock();
        }
    }
}
