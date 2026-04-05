package NPCommon.Util.Delegate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;

import java.util.List;

/***
 * 4个参数的监听
 * @param <T1>
 * @param <T2>
 * @param <T3>
 * @param <T4>
 */
public class ADelegateFour<T1, T2, T3, T4> extends _ADelegateCommon<HandlerFour<T1, T2, T3, T4>>
{
    public ADelegateFour(Object _parent)
    {
        super(_parent);
    }

    public void onAsyncEvent(final T1 t1, final T2 t2, final T3 t3, final T4 t4)
    {
        List<HandlerFour<T1, T2, T3, T4>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (HandlerFour<T1, T2, T3, T4> theHandler : handlers)
            {
                try
                {
                    theHandler.handle(t1, t2, t3, t4);
                } catch (Throwable e)
                {
                    CommLog.error("deal handler caught Exception ", e);
                }

            }
        });
    }

    public void onEvent(final T1 t1, final T2 t2, final T3 t3, final T4 t4)
    {
        List<HandlerFour<T1, T2, T3, T4>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        for (HandlerFour<T1, T2, T3, T4> handler : handlers)
        {
            try
            {
                handler.handle(t1, t2, t3, t4);
            } catch (Throwable e)
            {
                CommLog.error("deal handler caught Exception ", e);
            }

        }

    }
}
