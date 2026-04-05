package NPCommon.TickTask;

/****
 * 实现该接口的类可以被TickTask调用
 */
public interface _ITickingObj
{
    void tick(long nowMs);

}
