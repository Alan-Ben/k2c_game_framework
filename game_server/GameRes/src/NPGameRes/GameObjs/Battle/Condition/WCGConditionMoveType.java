package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGMoveType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionMoveType extends _AWCGBasicSingleCondition
{
    private int _m_iMoveType;

    public int MoveType()
    {
        return _m_iMoveType;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.MOVE_TYPE;
    }

    public static WCGConditionMoveType readCond(String _str)
    {
        WCGConditionMoveType cond = new WCGConditionMoveType();

        String[] strs = CommonFunc.charSplit(_str, ':');

        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                EWCGMoveType moveType = EWCGMoveType.valueOf(strs[i].toUpperCase().trim());
                cond._m_iMoveType |= 1 << moveType.ordinal();
            }
            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件读取错误 - 移动类型 example: move_type:... 错误信息: " + _str, e);
            return null;
        }
    }
}
