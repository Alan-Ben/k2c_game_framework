package NPGameRes.Refs.CommonEvent.SubClass;

import Common.EventEnum.ECommonEventType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;

@RefTable(tableName = "common_event_mini_game")
public class RefCommonEventMiniGame extends _ARefCommonEvent
{
    private static RefTableContainer<RefCommonEventMiniGame> _g_mgr = new RefTableContainer<RefCommonEventMiniGame>();

    public static RefTableContainer<RefCommonEventMiniGame> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEventMiniGame> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEventMiniGame>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEventMiniGame newRef = (RefCommonEventMiniGame) _newRef;
        id = newRef.id;
        event_reward_id = newRef.event_reward_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//唯一id
    public long event_reward_id;//事件奖励id

    @Override
    public ECommonEventType getEventType()
    {
        return ECommonEventType.MINI_GAME;
    }
}
