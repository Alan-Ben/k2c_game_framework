package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableSkillLvl extends _AWCGBasicVariableObj
{
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 技能Id
     */
    private long _m_lSkillId;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public long skillId()
    {
        return _m_lSkillId;
    }

    protected WCGVariableSkillLvl()
    {
        _m_eTargetType = EWCGEffectTargetType.TAR;
        _m_lSkillId = 0;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    @Override
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.SKILL_LVL;
    }


    public static WCGVariableSkillLvl readVariable(String _str)
    {
        WCGVariableSkillLvl variableObj = new WCGVariableSkillLvl();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - SKILL_LVL example: enum:target:skill_id Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_lSkillId = Long.parseLong(strs[1].trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - SKILL_LVL example: enum:target:skill_id Error Str: " + _str, e);
            return null;
        }
    }
}