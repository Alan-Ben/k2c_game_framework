package NPGameRes.Refs.Activity;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.GiftPack.RefGiftPackGroup;

import java.util.ArrayList;
import java.util.List;

/**
 * 通用活动配置
 */
@RefTable(tableName = "activity_main")
public class RefActivity extends RefBase
{
    private static RefActivityMgr _g_mgr = new RefActivityMgr();

    public static RefActivityMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityMgr) _mgr;
    }


    public static class RefActivityMgr extends RefTableContainer<RefActivity>
    {

        @Override
        public void _onTableLoaded()
        {
        }
    }
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivity newRef = (RefActivity) _newRef;
        activity_id = newRef.activity_id;
        type_id = newRef.type_id;
        name = newRef.name;
        rank_id_list = newRef.rank_id_list;
        step_reward_set_id_list = newRef.step_reward_set_id_list;
        exchange_shop_id = newRef.exchange_shop_id;
        crystal_gift_pack_group_id = newRef.crystal_gift_pack_group_id;
        cash_gift_pack_group_id_list = newRef.cash_gift_pack_group_id_list;
        is_game_logic = newRef.is_game_logic;
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
        return activity_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long activity_id;

    public int type_id;//活动类型id
    public String name;//活动名称
    public ArrayList<Long> rank_id_list = new ArrayList<>();
    public ArrayList<Long> step_reward_set_id_list = new ArrayList<>();

    public long exchange_shop_id;//兑换商店id
    public long crystal_gift_pack_group_id;//钻石礼包组id
    public List<Long> cash_gift_pack_group_id_list;// 礼包组id

    public boolean is_game_logic;//游戏主体是否放到game_logic服务器（服务端确认）

    public List<Long> getRelativeGiftPackIdList()
    {
        List<Long> relativeGiftPackIdList = new ArrayList<>();

        for (Long giftPackId : cash_gift_pack_group_id_list)
        {
            RefGiftPackGroup refGiftPackGroup = RefGiftPackGroup.getMgr().get(giftPackId);
            if (refGiftPackGroup == null)
                continue;

            relativeGiftPackIdList.addAll(refGiftPackGroup.gift_pack_id_list);
        }


        return relativeGiftPackIdList;
    }
}
