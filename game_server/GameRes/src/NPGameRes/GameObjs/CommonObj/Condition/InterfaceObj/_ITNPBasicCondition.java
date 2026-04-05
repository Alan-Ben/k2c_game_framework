package NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj;

/****************
 * 条件的基类对象
 * @author mj
 *
 */
public interface _ITNPBasicCondition<E extends Enum<E>>
{
    /*************
     * 返回条件类型的序号，一般用enum.ordinal()处理
     * @return
     */
    public abstract E conditionType();
}
