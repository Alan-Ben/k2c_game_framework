package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGBuffSpecialType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionBuffSpecialType extends _AWCGBasicSingleCondition
{
    /* 特殊标记列表*/
    private int _m_lSpecialTag;

    public int SpecialTag()
    {
        return _m_lSpecialTag;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_SPE_TAG;
    }

    public static WCGConditionBuffSpecialType readCond(String _str)
    {
        WCGConditionBuffSpecialType cond = new WCGConditionBuffSpecialType();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                EWCGBuffSpecialType specialTag = EWCGBuffSpecialType.valueOf(strs[i].toUpperCase().trim());
                cond._m_lSpecialTag |= 1 << (int) specialTag.ordinal();
            }

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - BUF_SPE_TAG example: enum:target:tag1:tag2.... " + _str);
            return null;
        }
    }
}
