package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Guarantee;

import Common.GachaObj.Gacha_GuaranteeInfo;
import NPGameRes.Refs.AvatarGacha.RefGachaGuarantee;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.GachaConditionDealerMgr;

public class GachaGuaranteeInfo
{
    private RefGachaGuarantee _m_refGuarantee;
    private int _m_guaranteeTimes;

    public GachaGuaranteeInfo(RefGachaGuarantee _refGuarantee)
    {
        _m_refGuarantee = _refGuarantee;
    }

    public void initFromDb(int _guaranteeTimes)
    {
        _m_guaranteeTimes = _guaranteeTimes;
    }

    public long getGuaranteeRuleId()
    {
        return _m_refGuarantee.id;
    }

    public RefGachaGuarantee getRef()
    {
        return _m_refGuarantee;
    }

    public boolean needOp()
    {
        return _m_refGuarantee.guarantee_times <= _m_guaranteeTimes + 1;
    }

    /**
     * 处理保底逻辑
     * @param _itemRef 抽卡结果
     */
    public void dealRollResult(NPUSUserData _userdata, RefGachaItem _itemRef)
    {
        //判断是否满足保底重置条件
        boolean fitResetCond = GachaConditionDealerMgr.getInstance().isEnable(_userdata, _m_refGuarantee.guarantee_reset_cond_list, _itemRef);
        //如果满足，则重置保底次数; 否则保底次数+1
        if (fitResetCond)
        {
            _m_guaranteeTimes = 0;
        }else
        {
            _m_guaranteeTimes++;
        }
    }

    /**
     * 重置保底次数
     */
    public void resetGuaranteeTimes()
    {
        _m_guaranteeTimes = 0;
    }

    /**
     * 获取还需要抽取多少次才能触发保底
     * @return 抽取次数
     */
    public int getPrepareTriggerTimes()
    {
        return Math.max(_m_refGuarantee.guarantee_times - _m_guaranteeTimes, 0);
    }

    @Override
    public String toString()
    {
        return _m_refGuarantee.id + ":" + _m_guaranteeTimes;
    }

    public Gacha_GuaranteeInfo makeProto()
    {
        Gacha_GuaranteeInfo guaranteeInfo = new Gacha_GuaranteeInfo();
        guaranteeInfo.setGuaranteeId(_m_refGuarantee.id);
        guaranteeInfo.setRemianTimes(getPrepareTriggerTimes());
        return guaranteeInfo;
    }
}
