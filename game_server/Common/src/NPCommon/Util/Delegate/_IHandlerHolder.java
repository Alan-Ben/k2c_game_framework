package NPCommon.Util.Delegate;

/*******
 * 该接口用于标识为可以持有某个HandlerBase,Delegate通过该接口对象来清除指定Handler
 * 避免把匿名类作为Handler的Owner加入Delegate而无法清除
 */
public interface _IHandlerHolder
{
}
