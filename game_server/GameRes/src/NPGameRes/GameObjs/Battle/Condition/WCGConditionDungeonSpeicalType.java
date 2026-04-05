package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGDungeonSpecialType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


/**************
 * 指定OperationID的操作是否可行
 **/
public class WCGConditionDungeonSpeicalType extends _AWCGBasicSingleCondition
{
    /* 副本类型*/
    private long _m_lSecialType = 0;

    public long SecialType()
    {
        return _m_lSecialType;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.DNG_SPE_TAG;
    }

    /**************
     * a格式如下: 枚举:操作ID:操作类型
     **/
    public static WCGConditionDungeonSpeicalType readCond(String _infoStr)
    {
        WCGConditionDungeonSpeicalType obj = new WCGConditionDungeonSpeicalType();

        String[] strs = CommonFunc.charSplit(_infoStr, ':');

        try
        {

            for (int i = 0; i < strs.length; i++)
            {
                EWCGDungeonSpecialType specialType = EWCGDungeonSpecialType.valueOf(strs[i].trim().toUpperCase());
                obj._m_lSecialType |= 1L << (int) specialType.ordinal();

            }

            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - WCGConditionDungeonSpeicalType example: enum:EWCGDungeonSpecialType:EWCGDungeonSpecialType" + _infoStr);
            return null;
        }
    }


}
