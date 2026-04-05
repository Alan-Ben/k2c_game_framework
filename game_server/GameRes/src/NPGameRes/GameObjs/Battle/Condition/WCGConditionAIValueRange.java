package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


/**************
 * ai对应索引值的范围(idx索引暂定0-99)，min,max如为-1表示无限制
 **/
public class WCGConditionAIValueRange extends _AWCGBasicSingleCondition
{
    private int _m_iIndex;

    private WCGIntRange _m_CountRange;

    public int Index()
    {
        return _m_iIndex;
    }

    public WCGIntRange Range()
    {
        return _m_CountRange;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.AI_V_RNG;
    }

    /**************
     * ai_value_range:idx:min:max - ai对应索引值的范围(idx索引暂定0-99)，min,max如为-1表示无限制
     **/
    public static WCGConditionAIValueRange read(String _infoStr)
    {
        WCGConditionAIValueRange obj = new WCGConditionAIValueRange();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 3)
        {
            CommLog.error("ai ai_value_range:idx:min:max 配置错误，_infoStr：" + _infoStr);
            return null;
        }
        try
        {
            obj._m_iIndex = Integer.parseInt(strs[0].trim());
            obj._m_CountRange = new WCGIntRange(strs[1], strs[2]);
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai ai_value_range:idx:min:max 配置错误，_infoStr：" + _infoStr, e);
            return null;
        }
    }
}