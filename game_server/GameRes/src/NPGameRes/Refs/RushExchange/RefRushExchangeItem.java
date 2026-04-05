package NPGameRes.Refs.RushExchange;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.HashMap;

/**
 * 急速兑换道具钻石价值表
 *
 * 主要功能：
 * 1. 定义各类道具对应的钻石价值
 * 2. 支持礼包物品价值评估和对比
 * 3. 用于游戏经济系统的价值计算
 *
 * 容器类型：普通表（RefTableContainer）
 * 数据结构：包含物品对象和价值属性
 */
@RefTable(tableName = "rush_exchange_item")
public class RefRushExchangeItem extends RefBase
{
    private static RefRushExchangeItemMgr _g_mgr = new RefRushExchangeItemMgr();

    /**
     * 获取配置表管理器
     */
    public static RefRushExchangeItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRushExchangeItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRushExchangeItemMgr) _mgr;
    }

    /**
     * 道具钻石价值表管理器
     *
     * 继承RefTableContainer支持多条记录管理
     * _onTableLoaded()用于在表加载完成后进行初始化处理
     *
     * 设计特点：
     * - 使用HashMap索引加速物品查询
     * - 复合键 = itemId * 10000L + itemType 确保唯一性
     */
    public static class RefRushExchangeItemMgr extends RefTableContainer<RefRushExchangeItem>
    {
        /**
         * 快速检索map <makeKey(itemType, itemId), RefRushExchangeItem>
         * 用于根据物品类型和ID快速查找对应的钻石价值配置
         */
        private HashMap<Long, RefRushExchangeItem> _m_itemValueMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            // 构造快速检索map，建立物品到钻石价值的映射
            for (RefRushExchangeItem ref : getList())
            {
                if (ref == null || ref.item == null)
                {
                    continue;
                }

                // 构造复合键并存入map
                long key = makeKey(ref.item.getItemType().ordinal(), ref.item.getItemId());
                _m_itemValueMap.put(key, ref);
            }
        }

        /**
         * 构造查询key
         *
         * 复合键计算公式：itemId * 10000L + itemType
         * 确保不同物品类型的相同ID不会冲突
         *
         * @param _itemType 物品类型枚举序号
         * @param _itemId 物品ID
         * @return 复合键
         */
        private long makeKey(int _itemType, long _itemId)
        {
            return _itemId * 10000L + _itemType;
        }

        /**
         * 根据物品信息查找道具钻石价值配置
         *
         * 执行流程：
         * 1. 校验物品对象有效性
         * 2. 构造查询key（itemId * 10000 + itemType）
         * 3. 从缓存map中快速查找配置
         *
         * @param _item 物品信息（包含物品类型和ID）
         * @return 道具钻石价值配置，找不到返回null
         *
         * 线程安全：只读操作，无需加锁
         */
        public RefRushExchangeItem lookupByItem(NPCommonItem _item)
        {
            if (_item == null)
            {
                return null;
            }

            long key = makeKey(_item.getItemType().ordinal(), _item.getItemId());
            return _m_itemValueMap.get(key);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRushExchangeItem newRef = (RefRushExchangeItem) _newRef;
        id = newRef.id;
        item = newRef.item;
        gem_count = newRef.gem_count;
    }

    /**
     * 获取对象数据Id，用于唯一标识本条配置记录
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置字段定义
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /** 配置ID（主键） */
    public long id;

    /** 物品信息（包含物品类型和物品ID） */
    public NPCommonItem item;

    /** 物品对应的钻石价值（用于礼包价值评估和折扣计算） */
    public int gem_count;

}
