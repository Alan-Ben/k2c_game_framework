package NPGameRes.Refs.Achieve;

import CommonEnum.EAchieveType;
import NPCommon.CommonObj.NPCountRate;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;

/**
 * @author mark 通用成就配置
 */
@RefTable(tableName = "achieve")
public class RefAchieve extends RefBase
{
    private static RefAchieveMgr _g_mgr = new RefAchieveMgr();

    public static RefAchieveMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAchieveMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAchieveMgr) _mgr;
    }


    public static class RefAchieveMgr extends RefTableContainer<RefAchieve>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAchieve newRef = (RefAchieve) _newRef;
        achieve_id = newRef.achieve_id;
        achieve_type = newRef.achieve_type;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        is_set = newRef.is_set;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return achieve_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long achieve_id;
    public EAchieveType achieve_type = EAchieveType.NONE; //成就类型枚举（EAchieveType）
    public NPPlayerVariableGroupObj process_cur_count; //进度当前值（高级公式）(计数器的话可以不用配置)(进度条计数目标达到时就算完成)
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public String trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public boolean is_set; //默认false：使用变量，true-使用当前量

    /**
     * 获取触发事件ID
     * @return
     */
    public int getTriggerEventId()
    {
        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(trigger_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("Achieve logic_event_id is null, please check, achieve_id:{} event:{}", achieve_id, trigger_event, new Exception());
            return 0;
        }
        return eventMeta.getEventId();
    }

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
