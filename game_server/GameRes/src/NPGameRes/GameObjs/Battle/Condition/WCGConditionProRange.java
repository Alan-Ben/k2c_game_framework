package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionProRange extends _AWCGBasicSingleCondition
{
    /**
     * 属性类型枚举
     */
    private ENPPropertyType _m_ePropertyType;
    /**
     * 范围数据结构体
     */
    private WCGIntRange _m_rPropertyRange;

    public ENPPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    public WCGIntRange PropertyRange()
    {
        return _m_rPropertyRange;
    }


    protected WCGConditionProRange()
    {
        _m_ePropertyType = ENPPropertyType.NONE;
        _m_rPropertyRange = null;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.P_RNG;
    }


    public static WCGConditionProRange readCond(String _str)
    {
        WCGConditionProRange cond = new WCGConditionProRange();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 - Pro_Range example: enum:property:min:max Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_ePropertyType = ENPPropertyType.valueOf(strs[0].toUpperCase().trim());
            cond._m_rPropertyRange = new WCGIntRange(strs[1], strs[2]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - Pro_Range example: enum:property:min:max Error Str: " + _str);
            return null;
        }
    }
}
