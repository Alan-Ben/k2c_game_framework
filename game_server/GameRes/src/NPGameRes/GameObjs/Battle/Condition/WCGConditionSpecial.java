package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;
import WCGCommon.Enum.NPEnum.EWCGSpecialConditionType;

public class WCGConditionSpecial extends _AWCGBasicSingleCondition
{

    /*特殊条件的类型*/
    private EWCGSpecialConditionType _m_eSpecialConditionType;

    /*比较是否成立（1（默认）-成立时判断通过 0-不成立时判断通过）*/
    private int _m_iEnableValue;

    public EWCGSpecialConditionType SpecialConditionType()
    {
        return _m_eSpecialConditionType;
    }

    public int EnableValue()
    {
        return _m_iEnableValue;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SPECIAL;
    }

    public WCGConditionSpecial()
    {
        _m_iEnableValue = 1;
    }


    public static WCGConditionSpecial read(String _str)
    {
        WCGConditionSpecial obj = new WCGConditionSpecial();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        try
        {
            obj._m_eSpecialConditionType = EWCGSpecialConditionType.valueOf(strs[0].toUpperCase().trim());
            if (strs.length > 1)
            {
                obj._m_iEnableValue = Integer.parseInt(strs[1].trim());
            }
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - SPECIAL example: EWCGSingleConditionType.SPECIAL:EWCGSpecialConditionType:(1 or 0)  Error Str: " + _str);
            return null;
        }
    }
}
