package NPCommon.Util.Delegate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;

import java.util.List;

/***
 * 1个参数的监听
 */
public class ADelegateOne<T> extends _ADelegateCommon<HandlerOne<T>>
{
    public ADelegateOne(Object _parent)
    {
        super(_parent);
    }

    public void onAsyncEvent(final T t)
    {
        List<HandlerOne<T>> handlers = getHandlerList();
        if (null == handlers)
            return;
        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (HandlerOne<T> theHandler : handlers)
            {
                try
                {
                    theHandler.handle(t);
                } catch (Throwable e)
                {
                    CommLog.error("deal handler caught Exception ", e);
                }
            }
        });
    }

    public void onEvent(final T t)
    {
        List<HandlerOne<T>> handlers = getHandlerList();
        if (null == handlers)
            return;
        for (HandlerOne<T> handler : handlers)
        {
            try
            {
                handler.handle(t);
            } catch (Throwable e)
            {
                CommLog.error("deal handler caught Exception ", e);
            }
        }
    }


}
