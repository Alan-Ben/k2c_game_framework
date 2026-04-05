package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommClass;
import NPGameRes.Refs.CommonEvent.RefCommonEvent;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventChoice;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventDispatch;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventDispatchCond;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;

import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;

public class CommonEventInitDealer extends _ABasicInitDealer
{
    @Override
    @SuppressWarnings("unchecked")
    public void dealInit()
    {
        //反射获取 _ARefCommonEvent 配表对象
        List<Class<?>> clazzs = CommClass.getAllClassByInterface(_ARefCommonEvent.class, _ARefCommonEvent.class.getPackage().getName());
        for (Class<?> clazz : clazzs)
        {
            try
            {
                //获取静态方法 getMgr
                Method method = clazz.getMethod("getMgr");
                //静态方法执行时 invoke 方法调用对象传null
                RefTableContainer<? extends _ARefCommonEvent> refContainer = (RefTableContainer<? extends _ARefCommonEvent>) method.invoke(null);
                //获取配表管理器中的所有配置对象
                for (_ARefCommonEvent ref : refContainer.getList())
                {
                    //将配置对象放置在event对象上
                    RefCommonEvent refEvent = RefCommonEvent.getMgr().get(ref.getCommonEventRefId());
                    if (refEvent == null)
                    {
                        continue;
                    }
                    refEvent.setDetailRef(ref);
                }
            } catch (Exception e)
            {
                CommLog.error("", e);
            }
        }

        //处理章节派遣事件的条件列表
        for (RefCommonEventDispatch refDispatch : RefCommonEventDispatch.getMgr().getList())
        {
            List<RefCommonEventDispatchCond> condList = new ArrayList<>();
            //遍历事件对应的条件id列表
            for (Long conditionId : refDispatch.condition_id_list)
            {
                RefCommonEventDispatchCond refCond = RefCommonEventDispatchCond.getMgr().get(conditionId);
                if (refCond == null)
                {
                    CommLog.error("ChapterInitDealer can not find RefCommonEventDispatchCond refId:{} ", conditionId);
                    continue;
                }

                condList.add(refCond);
            }
            //将条件列表放置在事件对象上
            refDispatch.setCondList(condList);

            //判断奖励配置数量是否合理
            if (refDispatch.event_reward_list.size() <= refDispatch.condition_id_list.size())
                CommLog.error("ChapterInitDealer RefCommonEventDispatch refId:{} event_reward_list.size <= condition_id_list.size", refDispatch.id);
        }

        //检查章节事件选项配置
        for (RefCommonEventChoice refChoice : RefCommonEventChoice.getMgr().getList())
        {
            if (refChoice.option_id_list.size() != refChoice.event_reward_id_list.size())
                CommLog.error("ChapterInitDealer RefCommonEventChoice refId:{} option_id_list.size != event_reward_id_list.size", refChoice.id);
        }
    }
}
