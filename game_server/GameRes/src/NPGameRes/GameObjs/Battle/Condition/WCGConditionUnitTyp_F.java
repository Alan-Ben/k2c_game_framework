package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGActorUnitType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


/// <summary>
/// 角色特殊标记,反向判定， 目标若是（XX里头的任何一个）则失效, 注意区别于 SPE_TAG_T
/// </summary>
public class WCGConditionUnitTyp_F extends _AWCGBasicSingleCondition
{

    /* 特殊标记列表 */
    private int _m_lUnitType;

    public int unitType()
    {
        return _m_lUnitType;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.UNIT_TYP_F;
    }

    public static WCGConditionUnitTyp_F readCond(String _str)
    {
        WCGConditionUnitTyp_F cond = new WCGConditionUnitTyp_F();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                EWCGActorUnitType unityTYpe = EWCGActorUnitType.valueOf(strs[i].toUpperCase().trim());
                cond._m_lUnitType |= 1 << (int) unityTYpe.ordinal();
            }

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - unit_typ_f example: unit_typ_f:tag1:tag2.... " + _str, e);
            return null;
        }
    }
}
