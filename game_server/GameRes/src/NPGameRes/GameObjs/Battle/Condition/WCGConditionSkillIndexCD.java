package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

/// <summary>
///  AI节点的条件:  从配置的技能Id集合中选择对应下标的技能Id,判断是否在cd中，在则不释放，不在则释放
/// </summary>
public class WCGConditionSkillIndexCD extends _AWCGBasicSingleCondition
{
    private int _m_iSkillIdx;

    public int SkillIdx()
    {
        return _m_iSkillIdx;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SK_IDX_OK;
    }

    public static WCGConditionSkillIndexCD read(String _infoStr)
    {
        WCGConditionSkillIndexCD obj = new WCGConditionSkillIndexCD();
        try
        {
            obj._m_iSkillIdx = Integer.parseInt(_infoStr.trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - SKILL_IDX_CD_OK example: enum:skill_Idx Error Str: " + _infoStr);
            return null;
        }
    }
}
