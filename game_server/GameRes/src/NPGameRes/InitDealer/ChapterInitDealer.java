package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommClass;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;
import NPGameRes.Refs.Chapter.RefChapterEvent;

import java.lang.reflect.Method;
import java.util.List;

public class ChapterInitDealer extends _ABasicInitDealer
{
    @Override
    @SuppressWarnings("unchecked")
    public void dealInit()
    {
        //反射获取 _ARefCommonEvent 配表对象
        List<Class<?>> clazzs = CommClass.getAllClassByInterface(_ARefChapterEvent.class, _ARefChapterEvent.class.getPackage().getName());
        for (Class<?> clazz : clazzs)
        {
            try
            {
                //获取静态方法 getMgr
                Method method = clazz.getMethod("getMgr");
                //静态方法执行时 invoke 方法调用对象传null
                RefTableContainer<? extends _ARefChapterEvent> refContainer = (RefTableContainer<? extends _ARefChapterEvent>) method.invoke(null);
                //获取配表管理器中的所有配置对象
                for (_ARefChapterEvent ref : refContainer.getList())
                {
                    //将配置对象放置在event对象上
                    RefChapterEvent refEvent = RefChapterEvent.getMgr().get(ref.Id());
                    if (refEvent == null)
                    {
                        CommLog.error("ChapterInitDealer refEvent is null, className:{} refId:{}", ref.getClass().getSimpleName(), ref.Id());
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
