package NPGameRes.Refs.CommonEvent.SubClass;

import Common.EventEnum.ECommonEventType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "common_event_dispatch")
public class RefCommonEventDispatch extends _ARefCommonEvent
{
    private static RefTableContainer<RefCommonEventDispatch> _g_mgr = new RefTableContainer<RefCommonEventDispatch>();

    public static RefTableContainer<RefCommonEventDispatch> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEventDispatch> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEventDispatch>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEventDispatch newRef = (RefCommonEventDispatch) _newRef;
        id = newRef.id;
        condition_id_list = newRef.condition_id_list;
        dispatch_result_id_list = newRef.dispatch_result_id_list;
        event_reward_list = newRef.event_reward_list;
        hero_num = newRef.hero_num;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//唯一id
    public List<Long> condition_id_list;//条件列表
    public List<Long> dispatch_result_id_list;//派遣结果id列表
    public List<Long> event_reward_list;//事件奖励id列表(符合0.1.2.3个条件的奖励)
    public int hero_num;//可派遣骑士数量

    @Override
    public ECommonEventType getEventType()
    {
        return ECommonEventType.DISPATCH;
    }

    @RefField(isIgnore = true)
    public List<RefCommonEventDispatchCond> condList = new ArrayList<>();

    /**
     * 设置条件列表
     * @param _condList 条件列表
     */
    public void setCondList(List<RefCommonEventDispatchCond> _condList)
    {
        condList = _condList;
    }
}
