package NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionDealerMgr;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.HeroCondition.HeroConditionGroupObj;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.Dealer.*;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

public class HeroConditionDealerMgr
        extends _ATNPBasicConditionDealerMgr<EHeroConditionType, _ABasicHeroCondition, HeroConditionGroupObj, HeroInfo>
{
    public static HeroConditionDealerMgr _g_instance = new HeroConditionDealerMgr();

    public static HeroConditionDealerMgr getInstance()
    {
        return _g_instance;
    }

    public HeroConditionDealerMgr()
    {
        super(EHeroConditionType.class);

        //注册处理对象
        _regDealer(new HeroConditionDealer_NONE());
        _regDealer(new HeroConditionDealer_CS_STAR());
        _regDealer(new HeroConditionDealer_CS_ATTR());
        _regDealer(new HeroConditionDealer_CS_LEVEL());
        _regDealer(new HeroConditionDealer_CS_STEP());
    }

    ///////////////////////////////////////////////////////////////

    public static boolean IsEnable(HeroConditionGroupObj _conditionGroupObj, HeroInfo _heroInfo, NPVarInfo _varVariableInfo)
    {
        return getInstance().judgeEnable(_conditionGroupObj, _heroInfo, _varVariableInfo);
    }

    public static boolean isRange(long _value, long _minValue, long _maxValue)
    {
        //如果预设的值已经是-1，表明出现错误（比如配置不存在等）
        if (-1 == _value)
            return false;

        if (-1 != _minValue && _value < _minValue)
            return false;

        if (-1 != _maxValue && _value > _maxValue)
            return false;

        return true;
    }
}
