package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionRange extends _AWCGBasicSingleCondition
{

    private int _m_iRange;//范围  厘米

    private int _m_eRelationType;

    public int Range()
    {
        return _m_iRange;
    }

    public int RelationType()
    {
        return _m_eRelationType;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RANGE;
    }

    public static WCGConditionRange read(String _infoStr)
    {
        WCGConditionRange obj = new WCGConditionRange();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 2)
        {
            CommLog.error("条件配置错误 - Range_Enemy example: enum:range:relationType Error Str: " + _infoStr);
            return null;
        }
        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());

            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1]);

            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - Range_Enemy example: enum:range:relationType Error Str: " + _infoStr);
            return null;
        }
    }
}

