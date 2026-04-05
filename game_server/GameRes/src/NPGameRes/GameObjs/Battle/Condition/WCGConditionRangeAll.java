package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


//范围内指定条件的敌人判断
//满足一个则达成条件
public class WCGConditionRangeAll extends _AWCGBasicSingleCondition
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
        return EWCGSingleConditionType.RALL;
    }

    public static WCGConditionRangeAll read(String _infoStr)
    {
        WCGConditionRangeAll obj = new WCGConditionRangeAll();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 2)
        {
            CommLog.error("条件配置错误 - range example: enum:range:relationType Error Str: " + _infoStr);
            return null;
        }
        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1]);

            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - range example: enum:range:relationType Error Str: " + _infoStr);
            return null;
        }
    }
}
