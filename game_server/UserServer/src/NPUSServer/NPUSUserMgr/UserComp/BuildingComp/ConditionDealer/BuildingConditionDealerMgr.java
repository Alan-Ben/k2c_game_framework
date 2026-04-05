package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer;

import Common.ConditionEnum.EBuildingConditionType;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.BuildingConditionGroupObj;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionDealerMgr;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer.BuildingConditionDealer_CS_BUILDING_ID;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer.BuildingConditionDealer_CS_SPEC_ATTR_TYPE;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer.BuildingConditionDealer_NONE;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp._IBuildingConditionProxy;

public class BuildingConditionDealerMgr
        extends _ATNPBasicConditionDealerMgr<EBuildingConditionType, _ABasicBuildingCondition, BuildingConditionGroupObj, _IBuildingConditionProxy>
{
    public static BuildingConditionDealerMgr _g_instance = new BuildingConditionDealerMgr();

    public static BuildingConditionDealerMgr getInstance()
    {
        return _g_instance;
    }

    public BuildingConditionDealerMgr()
    {
        super(EBuildingConditionType.class);

        //注册处理对象
        _regDealer(new BuildingConditionDealer_NONE());
        _regDealer(new BuildingConditionDealer_CS_SPEC_ATTR_TYPE());
        _regDealer(new BuildingConditionDealer_CS_BUILDING_ID());
    }

    ///////////////////////////////////////////////////////////////

    public static boolean IsEnable(BuildingConditionGroupObj _conditionGroupObj, _IBuildingConditionProxy _buildingInfo, NPVarInfo _varVariableInfo)
    {
        return getInstance().judgeEnable(_conditionGroupObj, _buildingInfo, _varVariableInfo);
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
