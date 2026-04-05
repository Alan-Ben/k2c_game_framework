package NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp;

import GS2GC.p021_PlayerInfo.GS2GC_021_052_OnFixedCDChged;
import NPCommon.DB.BM.BM;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.NPCommon_PlayerFixedCD;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefPlayerFixedCd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerFixedCdBO;

/**
 * @description: 固定刷新cd
 * @author: ricci
 * @date: 2022-10-24 16:44:45
 */
public class PlayerFixedCD
{
    /**
     * 定时cd组件
     */
    private PlayerFixedCdComp _m_comp;
    /**
     * 定时cd配置
     */
    private RefPlayerFixedCd _m_ref;
    /**
     * 数据库数据
     */
    private PlayerFixedCdBO _m_bo;

    public PlayerFixedCD(PlayerFixedCdComp _comp, RefPlayerFixedCd _ref, PlayerFixedCdBO _bo)
    {

        _m_comp = _comp;
        _m_ref = _ref;
        _m_bo = _bo;
    }

    public RefPlayerFixedCd getRef()
    {
        return _m_ref;
    }

    public PlayerFixedCdBO getBo()
    {
        return _m_bo;
    }

    /**
     * 获取最大上限
     * @return int
     */
    public int getMaxCount()
    {
        long addMaxCount = getUserData().getPlayerComponent().getPropertyMgr().getValue(getRef().property_add_count_max);
        return calMaxCount(getRef(), addMaxCount);
    }

    /**
     * 计算上限值
     */
    public static int calMaxCount(RefPlayerFixedCd ref, long _addMaxCount)
    {
        return (int) (ref.count_max + _addMaxCount);
    }

    //每次固定恢复点数
    private int getAddCountPerTime()
    {
        int incCount = (int) (getRef().add_count_per_time +
                getUserData().getPlayerComponent().getPropertyMgr().getValue(getRef().property_add_count_per_time));
        if (incCount == 0)
            incCount = 1;
        return incCount;
    }

    public int getCount()
    {
        //检测周期
        __applyCD();
        return _m_bo.getCount();
    }

    public int getRefId()
    {
        return _m_bo.getCdId();
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 设置count值并推送变更
     * @param _count   int
     * @param _context
     */
    public void setCount(int _count, NPPlayerContext _context)
    {
        _m_bo.saveCount(_m_comp.getUSServer().getBM(), _count);
        //推送数据
        __sendChgInfo();
    }


    /**
     * 补 满 cd 到最大上限
     */
    public void fill()
    {
        int maxCount = calMaxCount(_m_ref,
                getUserData().getPlayerComponent().getPropertyMgr().getValue(_m_ref.property_add_count_max));

        _m_bo.setCount(_m_comp.getUSServer().getBM(), maxCount);
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        _m_comp.logItem(_m_ref.id, 0, maxCount, NPCommonEnum.ELogItem_Type.INIT, getUserData().getPlayerInitContext());
    }

    /**
     * 玩家属性变更调用的事件函数
     * @param _propertyType 属性变更类型
     * @param _preValue     变化前的值
     * @param _value        变化后的值
     */
    public void _onPlayerPropertyChg(ENPPlayerPropertyType _propertyType, Long _preValue, Long _value)
    {
        //监测是否有关联
        if (__checkHasCDAboutProperty(_propertyType))
        {

            if (_propertyType == getRef().property_add_count_max)
            {
                int newMaxCount = calMaxCount(getRef(), _preValue);
                int prMaxCount = calMaxCount(getRef(), _value);

                //上限变动
                if (newMaxCount != prMaxCount)
                {
                    int diff = newMaxCount - prMaxCount;
                    //_oldValue = 0 等同于初始化状态，不增加当前值
                    if (diff > 0 && prMaxCount > 0)
                    {
                        addCountExt(diff);
                    }
                }
            } else
            {
                __applyCD();
                __sendChgInfo();
            }
        }
    }

    /***************
     * 计算定时恢复后当前实际值
     */
    private void __applyCD()
    {
        int maxCount = getMaxCount();
        int curCount = getBo().getCount();

        long nowTimeMs = CommonFunc.getNowTimeMS();
        long lastCalcTimeMs = getBo().getLastCalcTime();

        BM bmObj = _m_comp.getUSServer().getBM();

        //已经达到上限
        if (curCount >= maxCount)
        {
            getBo().saveLastCalcTime(bmObj, nowTimeMs);
            return;
        }

        //检查是否可以刷新
        long nextExtFreshTimeTagMS = _m_ref.refresh_rule.getNextFreshTimeTagMS(lastCalcTimeMs);
        if (nextExtFreshTimeTagMS > nowTimeMs) //下次刷新时间超过当前时间，不予刷新
        {
            return;
        }

        //计算需要增加的点数，需要根据实际周期数计算点数
        int addCountPerTime = getAddCountPerTime();
        int finalCount = curCount;
        /**
         * 计算固定CD，需要得到 周期内计数总和 & 最后一次刷新时间
         * 1. 如果增加的计数达到最大计数，则设置当前时间为最后一次刷新时间，并跳出
         * 2. 如果最后一次刷新时间超过当前时间，取上一次刷新时间为最后一次刷新时间，并跳出
         */
        do
        {
            finalCount = addCountPerTime + curCount;

            //超过上限但是无法超过，只能取上限值，同时设置刷新时间为当前时间
            if (finalCount >= maxCount)
            {
                finalCount = maxCount;
                nextExtFreshTimeTagMS = nowTimeMs;
                break;
            }

            //检查下次刷新时间
            long tmpNextExtFreshTimeTagMS = _m_ref.refresh_rule.getNextFreshTimeTagMS(nextExtFreshTimeTagMS);
            if (tmpNextExtFreshTimeTagMS >= nowTimeMs) //下次刷新时间超过了当前时间，所以退出循环
            {
                break;
            }
            //设置循环时间等于下次刷新时间
            nextExtFreshTimeTagMS = tmpNextExtFreshTimeTagMS;
        }
        while (true); //确定循环次数：最终总次数不能超过最大次数 && 下次刷新时间不能超过当前时间

        //保存数据
        getBo().setLastCalcTime(bmObj, nextExtFreshTimeTagMS);
        getBo().setCount(bmObj, finalCount);
        getBo().saveAllMarked(bmObj);

        _m_comp.logItem(_m_ref.id, curCount, finalCount, NPCommonEnum.ELogItem_Type.AUTO_RECOVER, null);
    }

    /**
     * 发送推送协议
     */
    private void __sendChgInfo()
    {
        GS2GC_021_052_OnFixedCDChged proto = new GS2GC_021_052_OnFixedCDChged();
        proto.setCdInfo(makeProto());
        getUserData().sendMsgToGC(proto);
    }

    /**
     * 构造协议数据
     * @return NPCommon_PlayerFixedCD
     */
    public NPCommon_PlayerFixedCD makeProto()
    {
        NPCommon_PlayerFixedCD proto = new NPCommon_PlayerFixedCD();
        proto.setCdId(getRefId());
        proto.setCount(getCount());
        proto.setLastCalTimeMS(getBo().getLastCalcTime());
        proto.setMaxCount(getMaxCount());
        proto.setAddCountPerTime(getAddCountPerTime());
        return proto;
    }

    /**
     * 外部额外增加计数，不改变cd，且可以超过上限
     * @param _diff 变化值
     * @return 实际变更值
     */
    public int addCountExt(int _diff)
    {
        if (_diff <= 0)
            return 0;

        //检测时间
        __applyCD();

        int curCount = getCount();
        if (curCount >= getMaxCount() && !getRef().is_can_excceed_limit)
        {
            return 0;
        }

        int finalCount = curCount + _diff;
        //如果不能超过上限，则最终值不能超过上限
        if (finalCount > getMaxCount() && !getRef().is_can_excceed_limit)
        {
            finalCount = getMaxCount();
        }
        //保存数据
        getBo().saveCount(_m_comp.getUSServer().getBM(), finalCount);

        //发送协议
        __sendChgInfo();

        return finalCount - curCount;
    }
    
    /********
     * 减少cd值
     * @param _iValue
     */
    public void descCount(int _iValue, NPPlayerContext _context)
    {
        if (_iValue <= 0)
            return;

        //检测时间
        __applyCD();

        int curCount = getCount() - _iValue;
        if (curCount < 0)
        {
            curCount = 0;
        }

        //保存值
        getBo().saveCount(_m_comp.getUSServer().getBM(), curCount);

        //发送信息
        __sendChgInfo();
    }

    /**
     * 作弊命令设置刷新时间
     */
    public void cmdSetFreshTime()
    {
        getBo().saveLastCalcTime(_m_comp.getUSServer().getBM(), 0);
        __applyCD();
        __sendChgInfo();
    }

    //判断属性变动是否会影响Cd
    private boolean __checkHasCDAboutProperty(ENPPlayerPropertyType _chgType)
    {
        return _chgType == getRef().property_add_count_max
                || _chgType == getRef().property_add_count_per_time;
    }

    @Override
    public String toString()
    {
        __applyCD();
        long nextFreshTime = getRef().refresh_rule.getNextFreshTimeTagMS(getBo().getLastCalcTime());

        return "NPPlayerFixedCD{" +
                "\n" + "refId: " + getRefId() + "\n" +
                "\n" + "maxCount: " + getMaxCount() + "\n" +
                "\n" + "count: " + getCount() + "\n" +
                "\n" + "lastTime: " + getBo().getLastCalcTime() + "\n" +
                "\n" + "nextFreshTimsMs: " + nextFreshTime + "\n" +
                "}";
    }

}
