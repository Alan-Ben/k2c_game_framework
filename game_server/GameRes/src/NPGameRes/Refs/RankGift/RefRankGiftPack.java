package NPGameRes.Refs.RankGift;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPDateTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

/**
 * 冲榜礼包配置表
 *
 * 对应配置表：rank_gift_pack
 * 字段：id, ui_res_path_id, sale, buy_limit, cost_list, reward_item_list, end_time_ms
 *
 * 设计说明：
 * - 该配表定义冲榜礼包的基础配置
 * - 实际运行时数据会被缓存到数据库，避免热更配表导致数据异常
 */
@RefTable(tableName = "rank_gift_pack")
public class RefRankGiftPack extends RefBase
{
    // 静态管理器
    private static RefRankGiftPackMgr _g_mgr = new RefRankGiftPackMgr();

    public static RefRankGiftPackMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRankGiftPackMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRankGiftPackMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRankGiftPack newRef = (RefRankGiftPack) _newRef;
        id = newRef.id;
        ui_res_path_id = newRef.ui_res_path_id;
        sale = newRef.sale;
        buy_limit = newRef.buy_limit;
        name = newRef.name;
        cost = newRef.cost;
        ori_cost = newRef.ori_cost;
        reward_item_list = newRef.reward_item_list;
        end_time = newRef.end_time;
    }

    /**
     * 管理器类
     */
    public static class RefRankGiftPackMgr extends RefTableContainer<RefRankGiftPack>
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
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置表字段

    // 礼包ID
    public long id;

    // 礼包页面加载资源id
    public long ui_res_path_id;

    // 折扣(万分比)
    public int sale;

    // 购买次数限制
    public int buy_limit;

    // 名称
    public String name;

    // 消耗道具
    public NPCommonCostItem cost;

    // 原价消耗道具
    public NPCommonCostItem ori_cost;

    // 奖励道具列表
    public List<NPCommonCostItem> reward_item_list = new ArrayList<>();

    // 截止时间（格式：YYYY-MM-DD HH:mm:ss，例如：2026-01-20 14:30:45）
    public NPDateTimeObj end_time = new NPDateTimeObj();
}