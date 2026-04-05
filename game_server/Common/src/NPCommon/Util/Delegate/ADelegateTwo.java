package NPCommon.Util.Delegate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;

import java.util.List;

/****
 * 2个参数的监听
 * @param <T1>
 * @param <T2>
 */
public class ADelegateTwo<T1, T2> extends _ADelegateCommon<HandlerTwo<T1, T2>>
{
    public ADelegateTwo(Object _parent)
    {
        super(_parent);
    }


    public void onAsyncEvent(final T1 t1, final T2 t2)
    {
        List<HandlerTwo<T1, T2>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (HandlerTwo<T1, T2> theHandler : handlers)
            {
                try
                {
                    theHandler.handle(t1, t2);
                } catch (Throwable e)
                {
                    CommLog.error("deal handler caught Exception ", e);
                }

            }
        });
    }

    public void onEvent(final T1 t1, final T2 t2)
    {
        List<HandlerTwo<T1, T2>> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        for (HandlerTwo<T1, T2> handler : handlers)
        {
            try
            {
                handler.handle(t1, t2);
            } catch (Throwable e)
            {
                CommLog.error("deal handler caught Exception ", e);
            }

        }
    }

    public ADelegateTwo<T1, T2> duplicate(Object _parent)
    {
        ADelegateTwo<T1, T2> ret = new ADelegateTwo<>(_parent);
        ret._m_entryList.addAll(_m_entryList);
        return ret;
    }
}
