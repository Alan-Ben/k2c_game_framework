package NPGameRes.GameObjs.CommonObj.Condition;

import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPConditionDealerData;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;

/**************
 * 条件的处理基类
 * @author mj
 *
 */
public interface _ITNPBasicConditionDealer
        <E extends Enum<E>
                , C extends _ITNPBasicCondition<E>
                , D extends _ITNPConditionDealerData>
{
    /*************
     * 返回条件类型的序号，一般用enum.ordin()处理
     * @return
     */
    E conditionType();

    /********************
     * 判断条件是否匹配
     * @param _cond
     * @param _data
     * @param _varVariableInfo
     * @return
     */
    boolean isEnable(C _cond, D _data, NPVarInfo _varVariableInfo);
}
