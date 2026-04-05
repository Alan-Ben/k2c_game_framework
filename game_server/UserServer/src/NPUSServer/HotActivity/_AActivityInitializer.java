package NPUSServer.HotActivity;


/******
 * 活动初始化对象负责初始化活动的逻辑，包括注册事件处理，通用排行榜等，待扩展
 */
public abstract class _AActivityInitializer
{
    protected abstract int getActivityType();

    public boolean init()
    {
        return true;
    }
}
