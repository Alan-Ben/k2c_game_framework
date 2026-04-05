package NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj;

/*************
 * 参数公式对象接口类
 * @author mj
 *
 */
public interface _ITNPBasicVariable<E extends Enum<E>>
{
    /*************
     * 返回条件类型的序号，一般用enum.ordinal()处理
     * @return
     */
    public abstract E variableType();
}
