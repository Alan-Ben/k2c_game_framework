package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGOperationType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

/**************
 * 指定OperationID的操作是否可行
 **/
public class WCGConditionActorOperationFeasibility extends _AWCGBasicSingleCondition
{
    //操作OperationID
    private int _m_lOperationID;
    /* 单位类型*/
    private EWCGOperationType _m_eOperationType;

    public int OperationID()
    {
        return _m_lOperationID;
    }

    public EWCGOperationType OperationType()
    {
        return _m_eOperationType;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.ACTOR_OPERATION_FEASIBILITY;
    }

    /**************
     * a格式如下: 枚举:操作ID:操作类型
     **/
    public static WCGConditionActorOperationFeasibility readCond(String _infoStr)
    {
        WCGConditionActorOperationFeasibility obj = new WCGConditionActorOperationFeasibility();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 2);

        try
        {
            obj._m_lOperationID = Integer.parseInt(strs[0]);
            obj._m_eOperationType = EWCGOperationType.valueOf(strs[1].toUpperCase().trim());

            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - ActorOperationFeasibility example: enum:operationid:operation_type" + _infoStr, e);
            return null;
        }
    }


}