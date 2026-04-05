package NPCommon.Util.Delegate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;

import java.util.List;

/*****
 * 3个参数的监听
 * @param <T1>
 * @param <T2>
 * @param <T3>
 */
public class ADelegateThree<T1, T2, T3> extends _ADelegateCommon<HandlerThree<T1, T2, T3>>
{

    public ADelegateThree(Object _parent)
    {
        super(_parent);
    }


    public void onAsyncEvent(final T1 t1, final T2 t2, final T3 t3)
    {
        List<HandlerThree<T1, T2, T3>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (HandlerThree<T1, T2, T3> theHandler : handlers)
            {
                try
                {
                    theHandler.handle(t1, t2, t3);
                } catch (Throwable e)
                {
                    CommLog.error("deal handler caught Exception ", e);
                }
            }
        });
    }

    public void onEvent(final T1 t1, final T2 t2, final T3 t3)
    {
        List<HandlerThree<T1, T2, T3>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        for (HandlerThree<T1, T2, T3> handler : handlers)
        {
            try
            {
                handler.handle(t1, t2, t3);
            } catch (Throwable e)
            {
                CommLog.error("deal handler caught Exception ", e);
            }
        }
    }
}
