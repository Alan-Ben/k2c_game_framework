package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.Variable.*;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

/*******************
 * 变量结构体基类
 **/
public abstract class _AWCGBasicVariableObj
{
    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public abstract EWCGVariableType variableType();


    /********************
     * 从节点中读取相关信息
     *
     * @author alzq.z
     * @time Jun 27, 2013 12:29:59 AM
     */
    public static _AWCGBasicVariableObj readVariable(EWCGVariableType _variableType, String _infoStr)
    {
        if (EWCGVariableType.NUM == _variableType)
            return WCGVariableNum.readVariable(_infoStr);
        if (EWCGVariableType.PRO == _variableType)
            return WCGVariablePro.readVariable(_infoStr);
        if (EWCGVariableType.BUF_STACK == _variableType)
            return WCGVariableBuffStack.readVariable(_infoStr);
        if (EWCGVariableType.VALUE == _variableType)
            return WCGVariableValue.readVariable(_infoStr);
        if (EWCGVariableType.SPECIAL == _variableType)
            return WCGVariableSpecial.readVariable(_infoStr);
        if (EWCGVariableType.BUF_TYPE_STACK == _variableType)
            return WCGVariableBuffTypeStack.readVariable(_infoStr);
        if (EWCGVariableType.TEAM_PRO == _variableType)
            return WCGVariableTeamPro.readVariable(_infoStr);
        if (EWCGVariableType.TEAM_VALUE == _variableType)
            return WCGVariableTeamValue.readVariable(_infoStr);
        if (EWCGVariableType.RND == _variableType)
            return WCGVariableRnd.readVariable(_infoStr);
        if (EWCGVariableType.AI_VALUE == _variableType)
            return WCGVariableAIValue.readVariable(_infoStr);
        if (EWCGVariableType.ACTOR_VALUE == _variableType)
            return WCGVariableActorValue.readVariable(_infoStr);
        if (EWCGVariableType.SP_ACTOR_VALUE == _variableType)
            return WCGVariableSpActorValue.readVariable(_infoStr);
        if (EWCGVariableType.BATTLE_VALUE == _variableType)
            return WCGVariableBattleValue.readVariable(_infoStr);
        if (EWCGVariableType.SKILL_LVL == _variableType)
            return WCGVariableSkillLvl.readVariable(_infoStr);

        if (EWCGVariableType.SPE_GROUP_PRO == _variableType)
            return WCGVariableSpecialGroupPro.readVariable(_infoStr);
        if (EWCGVariableType.SPE_GROUP_VALUE == _variableType)
            return WCGVariableSpecialGroupValue.readVariable(_infoStr);
        if (EWCGVariableType.SPE_GROUP_RES == _variableType)
            return WCGVariableTeamRes.readVariable(_infoStr);
        if (EWCGVariableType.CAMP_PRO == _variableType)
            return WCGVariableCampValue.readVariable(_infoStr);
        if (EWCGVariableType.CAMP_RES == _variableType)
            return WCGVariableCampRes.readVariable(_infoStr);
        if (EWCGVariableType.VAR_VALUE == _variableType)
            return WCGVariableVarValue.readVariable(_infoStr);
        if (EWCGVariableType.OWNER_V == _variableType)
            return WCGVariableOwnerValue.readVariable(_infoStr);
        return null;
    }
}
