package NPGameRes.Refs.PushGift;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 推送礼包配置表
 * <p>
 * 对应配置表：push_gift_pack
 * 字段：push_gift_id, gift_pack_id, continue_time,
 * next_trigger_push_gift_id, after_buy_auto_trigger_next
 */
@RefTable(tableName = "push_gift_pack")
public class RefPushGiftPack extends RefBase
{
    // 静态管理器
    private static RefPushGiftPackMgr _g_mgr = new RefPushGiftPackMgr();

    public static RefPushGiftPackMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefPushGiftPackMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefPushGiftPackMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPushGiftPack newRef = (RefPushGiftPack) _newRef;
        push_gift_id = newRef.push_gift_id;
        gift_pack_id = newRef.gift_pack_id;
        continue_time = newRef.continue_time;
        downgrade_trigger_push_gift_id = newRef.downgrade_trigger_push_gift_id;
        next_trigger_push_gift_id = newRef.next_trigger_push_gift_id;
        after_buy_auto_trigger_next = newRef.after_buy_auto_trigger_next;
    }

    /**
     * 管理器类
     */
    public static class RefPushGiftPackMgr extends RefTableContainer<RefPushGiftPack>
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
        return push_gift_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置表字段

    // 推送礼包id
    public long push_gift_id;

    // 关联的礼包id（指向gift_pack表）
    public long gift_pack_id;

    // 持续时间（秒）
    public long continue_time;

    // 不购买降档的推送礼包id（0表示无）
    public long downgrade_trigger_push_gift_id;

    // 下一个触发的推送礼包id（0表示无下一个）
    public long next_trigger_push_gift_id;

    // 购买后是否自动触发下一个
    public boolean after_buy_auto_trigger_next;
}
