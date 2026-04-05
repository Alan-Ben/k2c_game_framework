package NPGameRes.GameObjs.CommonObj.Variable;

import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPBasicVariable;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPVariableDealerData;

/**************
 * 计算公式的公式处理类
 * @author mj
 *
 * @param <E>
 * @param <V>
 * @param <D>
 */
public interface _ITNPBasicVariableDealer<E extends Enum<E>, V extends _ITNPBasicVariable<E>, D extends _ITNPVariableDealerData<E>>
{
    /************
     * 获取计算公式处理的公式类型
     * @return
     */
    public abstract E VariableType();

    /******************
     * 处理计算公式的计算操作
     * @param _userData
     * @param _variableObj
     * @param _variableInfo
     * @return
     */
    public abstract long PlayerVariableValue(D _userData, V _variableObj, NPVarInfo _variableInfo);
}
