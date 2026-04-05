package NPUSServer.NPUSUserMgr.UserComp.ClockRewardComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Battle.RefClockReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerClockRewardBO;
import USLOGDB.Bo.LogClockRewardSendBO;

/**
 * @description: 玩家定时奖励发放管理数据
 * @author: ricci
 * @date: 2022-10-24 13:41:51
 */
public class ClockRewardObj
{

    /**
     * 定时下发奖励组件
     */
    private ClockRewardComponent _m_clockRewardComp;
    /**
     * 定时下发奖励配置
     */
    private RefClockReward _m_ref;
    /**
     * 数据库数据
     */
    private long _m_lDbid;
    private long _m_lNextRefreshTimeMs;
    private int _m_iNum;

    public ClockRewardObj(ClockRewardComponent _comp, RefClockReward _ref, PlayerClockRewardBO _bo)
    {
        _m_clockRewardComp = _comp;

        _m_ref = _ref;

        _m_lDbid = _bo.getId();
        _m_lNextRefreshTimeMs = _bo.getNextRefreshTimeMs();
        _m_iNum = _bo.getNum();
    }

    public long getRefId()
    {
        return _m_ref.id;
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    private long getNextSendClockRewardTimeMs()
    {
        return _m_lNextRefreshTimeMs;
    }

    private int getSendNum()
    {
        return _m_iNum;
    }

    public NPUSUserData getUserData()
    {
        return _m_clockRewardComp.getUserData();
    }

    /**
     * 尝试下发定时奖励
     * @param _nowTimeMs 执行逻辑的时机
     */
    public void trySendClockReward(long _nowTimeMs)
    {
        //检查下一次下发时间时间
        if (getNextSendClockRewardTimeMs() < _nowTimeMs)
        {
            return;
        }
        //检查下发次数
        if (getSendNum() >= _m_ref.num)
        {
            return;
        }
        //TODO：需求疑问：此时能通过条件判断，不代表之前也能通过条件判断，这时候结算当前还是要结算历史经过会与需求有关，暂时没有详细的需求，这里等待完善
        //检查发放条件
        if (!NPPlayerConditionDealerMgr.getInstance()
                .judgeEnable(_m_ref.cond, getUserData(), null))
        {
            return;
        }
        //发放奖励
        sendClockReward(_nowTimeMs);
    }

    /**
     * 发放奖励，并更新时间
     * @param _nowTimeMs 当前时间
     */
    public void sendClockReward(long _nowTimeMs)
    {
        //记录发放次数
        _m_iNum += 1;
        //计算下次发放时间
        _m_lNextRefreshTimeMs = _m_ref.refresh_clock.getNextFreshTimeTagMS(_nowTimeMs);

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("next_refresh_time_ms", _m_lNextRefreshTimeMs);
        updateValue.addValueObj("num", _m_iNum);
        getUserData().getUSServer().getBM().getBM(PlayerClockRewardBO.class).update("id", _m_lDbid, updateValue);

        //发放奖励
        NPPlayerContext _context = NPPlayerContext.createNew(ENPGameEvent.CLOCK_REWARD);
        getUserData().gainItemList(_m_ref.item_list, _context);
        
        //日志数据
        LogClockRewardSendBO logBo = new LogClockRewardSendBO();
        logBo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
        logBo.setNum(getUserData().getUSServer().getBM(), _m_iNum);
        logBo.setNextMs(getUserData().getUSServer().getBM(), _m_lNextRefreshTimeMs);
        CommLogDB.log(getUserData().getUSServer().getBM(), logBo, _context);
    }
}
