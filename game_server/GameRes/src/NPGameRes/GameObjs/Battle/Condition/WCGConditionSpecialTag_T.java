package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGActorSpecialType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionSpecialTag_T extends _AWCGBasicSingleCondition
{
    /* 特殊标记列表*/
    private int _m_lSpecialTag;

    public int SpecialTag()
    {
        return _m_lSpecialTag;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SPE_TAG_T;
    }

    public static WCGConditionSpecialTag_T readCond(String _str)
    {
        WCGConditionSpecialTag_T cond = new WCGConditionSpecialTag_T();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                EWCGActorSpecialType specialTag = EWCGActorSpecialType.valueOf(strs[i].toUpperCase().trim());
                cond._m_lSpecialTag |= 1 << specialTag.ordinal();
            }

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - Special_Tag example: enum:target:tag1:tag2.... " + _str);
            return null;
        }
    }
}
