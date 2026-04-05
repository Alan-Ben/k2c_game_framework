package NPGameRes.Refs.PushGift;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * 推送礼包组配置表
 * <p>
 * 对应配置表：push_gift_group
 * 字段：group_id, gift_pack_type, need_reset_buy_count, trigger_condition,
 * next_trigger_need_seconds, first_trigger_gift_id
 */
@RefTable(tableName = "push_gift_group")
public class RefPushGiftGroup extends RefBase
{
    // 静态管理器
    private static RefPushGiftGroupMgr _g_mgr = new RefPushGiftGroupMgr();

    public static RefPushGiftGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefPushGiftGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefPushGiftGroupMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPushGiftGroup newRef = (RefPushGiftGroup) _newRef;
        group_id = newRef.group_id;
        need_reset_buy_count = newRef.need_reset_buy_count;
        trigger_condition = newRef.trigger_condition;
        next_trigger_need_seconds = newRef.next_trigger_need_seconds;
        first_trigger_gift_id = newRef.first_trigger_gift_id;
    }

    /**
     * 管理器类
     */
    public static class RefPushGiftGroupMgr extends RefTableContainer<RefPushGiftGroup>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配置表加载完成后的处理
        }
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return group_id;
    }

    // 配置表字段

    // 礼包组id
    public long group_id;

    // 是否需要重置购买次数
    public boolean need_reset_buy_count;

    // 触发条件（玩家条件组）
    public NPPlayerConditionGroupObj trigger_condition;

    // 下次触发需要的时间间隔（秒）
    public long next_trigger_need_seconds;

    // 首次触发的礼包ID
    public long first_trigger_gift_id;
}
