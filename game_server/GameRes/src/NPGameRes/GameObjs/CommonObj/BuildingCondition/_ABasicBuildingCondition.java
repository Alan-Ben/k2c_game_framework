package NPGameRes.GameObjs.CommonObj.BuildingCondition;

import ALServerLog.ALServerLog;
import Common.ConditionEnum.EBuildingConditionType;
import NPCommon.CommonObj.NPStringReader;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj.BuildingCondition_CS_BUILDING_ID;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj.BuildingCondition_CS_SPEC_ATTR_TYPE;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj.BuildingCondition_NONE;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;

abstract public class _ABasicBuildingCondition implements _ITNPBasicCondition<EBuildingConditionType>
{
    /**************
     * 将字符串转化为本对象
     **/
    public static _ABasicBuildingCondition readCondition(String _str)
    {
        NPStringReader stringReader = new NPStringReader(_str);
        //读取类型字符串
        String condTypeS = stringReader.readItem(':');

        if (null == condTypeS)
        {
            ALServerLog.Error("空的条件字符串:" + _str);
            return null;
        }

        EBuildingConditionType condType = EBuildingConditionType.NONE;
        try
        {
            //读取类型枚举
            condType = EBuildingConditionType.valueOf(condTypeS.toUpperCase());
        } catch (Exception e)
        {
        }

        if (condType == EBuildingConditionType.NONE)
        {
            ALServerLog.Error("错误的条件类型:" + _str);
            return null;
        }

        return readCondition(condType, stringReader);
    }

    /********************
     * 从节点中读取相关信息
     */
    public static _ABasicBuildingCondition readCondition(EBuildingConditionType _conditionType, NPStringReader _reader)
    {
        switch (_conditionType)
        {
            case NONE:
                return new BuildingCondition_NONE();
            case CS_BUILDING_ID:
                return BuildingCondition_CS_BUILDING_ID.readCond(_reader);
            case CS_SPEC_ATTR_TYPE:
                return BuildingCondition_CS_SPEC_ATTR_TYPE.readCond(_reader);
            default:
                return new BuildingCondition_NONE();
        }
    }
}
