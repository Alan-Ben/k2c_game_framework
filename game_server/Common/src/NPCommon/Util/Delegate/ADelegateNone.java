package NPCommon.Util.Delegate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;

import java.util.List;

/***
 * 0个参数的监听
 */
public class ADelegateNone extends _ADelegateCommon<HandlerNone>
{

    public ADelegateNone(Object _parent)
    {
        super(_parent);
    }

    public void onAsyncEvent()
    {
        List<HandlerNone> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (HandlerNone theHandler : handlers)
            {
                try
                {
                    theHandler.handle();
                } catch (Throwable e)
                {
                    CommLog.error("deal handler caught Exception ", e);
                }

            }
        });
    }

    public void onEvent()
    {
        List<HandlerNone> handlers = getHandlerList();
        if (null == handlers)
        {
            return;
        }
        for (HandlerNone handler : handlers)
        {
            try
            {
                handler.handle();
            } catch (Throwable e)
            {
                CommLog.error("deal handler caught Exception ", e);
            }
        }
    }

}
