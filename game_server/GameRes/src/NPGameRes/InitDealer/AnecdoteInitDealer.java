package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommClass;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPGameRes.Refs.Anecdote._ARefAnecdoteEvent;

import java.lang.reflect.Method;
import java.util.List;

public class AnecdoteInitDealer extends _ABasicInitDealer
{
    @Override
    @SuppressWarnings("unchecked")
    public void dealInit()
    {
        //反射获取 _ARefCommonEvent 配表对象
        List<Class<?>> clazzs = CommClass.getAllClassByInterface(_ARefAnecdoteEvent.class, _ARefAnecdoteEvent.class.getPackage().getName());
        for (Class<?> clazz : clazzs)
        {
            try
            {
                //获取静态方法 getMgr
                Method method = clazz.getMethod("getMgr");
                //静态方法执行时 invoke 方法调用对象传null
                RefTableContainer<? extends _ARefAnecdoteEvent> refContainer = (RefTableContainer<? extends _ARefAnecdoteEvent>) method.invoke(null);
                //获取配表管理器中的所有配置对象
                for (_ARefAnecdoteEvent ref : refContainer.getList())
                {
                    //将配置对象放置在event对象上
                    RefAnecdoteEvent refEvent = RefAnecdoteEvent.getMgr().get(ref.Id());
                    if (refEvent == null)
                    {
                        CommLog.error("AnecdoteInitDealer refEvent is null, className:{} refId:{}", ref.getClass().getSimpleName(), ref.Id());
                        continue;
                    }

                    refEvent.setDetailRef(ref);
                }
            } catch (Exception e)
            {
                CommLog.error("", e);
            }
        }

    }
}
