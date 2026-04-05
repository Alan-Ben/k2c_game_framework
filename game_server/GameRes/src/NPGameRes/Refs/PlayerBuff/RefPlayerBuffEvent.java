package NPGameRes.Refs.PlayerBuff;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;


@RefTable(tableName = "player_buff_event")
public class RefPlayerBuffEvent extends RefBase
{
    private static RefTableContainer<RefPlayerBuffEvent> _g_mgr = new RefTableContainer<RefPlayerBuffEvent>();

    public static RefTableContainer<RefPlayerBuffEvent> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerBuffEvent> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerBuffEvent>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerBuffEvent newRef = (RefPlayerBuffEvent) _newRef;
        id = newRef.id;
        logic_event_list = newRef.logic_event_list;
        trigger_condition = newRef.trigger_condition;
        trigger_per_form = newRef.trigger_per_form;
        trigger_reduce_layers = newRef.trigger_reduce_layers;
        trigger_effect = newRef.trigger_effect;
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
        return id;
    }
    ////////////////////////

    public long id;
    public ArrayList<String> logic_event_list = new ArrayList<>(); //监控事件枚举列表
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public NPPlayerVariableGroupObj trigger_per_form; //触发几率（高级公式）
    public int trigger_reduce_layers; //触发时减少的层数
    public NPPlayerEffectListParse trigger_effect; //buff触发时的效果
}
