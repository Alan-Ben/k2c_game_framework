package NPGameRes.Refs.CommonEvent.SubClass;

import Common.EventEnum.ECommonEventType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;

import java.util.List;

@RefTable(tableName = "common_event_choice")
public class RefCommonEventChoice extends _ARefCommonEvent
{
    private static RefTableContainer<RefCommonEventChoice> _g_mgr = new RefTableContainer<RefCommonEventChoice>();

    public static RefTableContainer<RefCommonEventChoice> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEventChoice> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEventChoice>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEventChoice newRef = (RefCommonEventChoice) _newRef;
        id = newRef.id;
        option_id_list = newRef.option_id_list;
        event_reward_id_list = newRef.event_reward_id_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//唯一id
    public List<Long> option_id_list;//选项列表
    public List<Long> event_reward_id_list;//事件奖励id列表

    @Override
    public ECommonEventType getEventType()
    {
        return ECommonEventType.CHOICE;
    }
}
