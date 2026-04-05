package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

/*******************
 * 通用条件类型对象
 **/
public abstract class _AWCGBasicTeamCondition
{
    /******************
     * 获取条件类型
     */
    public abstract EWCGTeamConditionType conditionType();

    /********************
     * 从节点中读取相关信息
     */
    public static _AWCGBasicTeamCondition readCondition(EWCGTeamConditionType _conditionType, String _infoStr)
    {
        _AWCGBasicTeamCondition ret = null;
        switch (_conditionType)
        {
            case NONE:
                return null;
            case TEAM_P_R:
                ret = WCGTeamConditionTeamProRange.readCond(_infoStr);
                break;
            case TEAM_V_R:
                ret = WCGTeamConditionTeamValueRange.readCond(_infoStr);
                break;
            case TEAM_S_R:
                ret = WCGTeamConditionTeamSCRange.readCond(_infoStr);
                break;
            case TEAM_S_COMP:
                ret = WCGTeamConditionSeniorCompare.readCond(_infoStr);
                break;
            case TEAM_BATTLE_TASK:
                ret = WCGTeamConditionTeamBattleTaskStateJudge.readCond(_infoStr);
                break;
            case SHARE_T_S_R:
                ret = WCGTeamConditionShareTeamSCRange.readCond(_infoStr);
                break;
            case ALL_ACT_S_R:
                ret = WCGTeamConditionAllActorSCRange.readCond(_infoStr);
                break;

            default:
                return null;
        }
        ret._m_src = _infoStr;
        return ret;
    }

    private String _m_src = "";

    public String getSrc()
    {
        return _m_src;
    }
}