package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.List;

/**
 * 首充每日奖励配置表读取类 (first_recharge_day)
 *
 * 表字段说明：
 * 1. day                      第N天（激活后天数），作为主键使用
 * 2. special_item             特殊奖励（例如第N天额外领取的独立奖励），可能为空
 * 3. item_list                奖励列表（普通每日奖励列表）
 * 7. buff_id                  关联Buff配置Id，用于条件判断或外部系统引用
 * 8. condition                激活/领取条件表达式（示例：CS_VARIABLE:1:-1:CS_BUFF_HAD_ACTIVE_DAY@1001）
 *
 * 设计要点：
 * - 使用 day 作为唯一标识（Id）。
 * - 保留特殊奖励与普通奖励分离，提供组合方法方便业务侧统一获取。
 * - day_reward_desc_args 采用 List<String> 存储，便于支持多类型参数（数值 / 文本）。
 * - condition 原样存储，由外部条件系统进行解析。
 * - 所有字段均保持与原始表结构语义一致，避免提前假设解析逻辑。
 */
@RefTable(tableName = "first_recharge_day")
public class RefFirstRechargeDay extends RefBase
{
    /** 静态管理器实例 */
    private static RefFirstRechargeDayMgr _g_mgr = new RefFirstRechargeDayMgr();

    /** 获取静态管理器 */
    public static RefFirstRechargeDayMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefFirstRechargeDayMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefFirstRechargeDayMgr) _mgr;
    }

    /**
     * 管理器：用于容器加载完成后的二次处理（当前无额外逻辑，预留扩展）。
     */
    public static class RefFirstRechargeDayMgr extends RefTableContainer<RefFirstRechargeDay>
    {
        @Override
        public void _onTableLoaded()
        {
            // 当前无需二次处理；如后续需要建立索引或校验可在此扩展。
        }
    }

    /**
     * 重置引用对象（用于热更 / 重载）。
     * @param _newRef 新配置对象
     */
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefFirstRechargeDay newRef = (RefFirstRechargeDay) _newRef;
        day = newRef.day;
        special_item = newRef.special_item;
        item_list = newRef.item_list;
        buff_id = newRef.buff_id;
        condition = newRef.condition;
    }

    /**
     * 获取对象唯一Id（使用 day 字段）。
     * @return day 作为唯一标识
     */
    @Override
    public long Id()
    {
        return day;
    }

    // ============================= 配置字段 ============================= //
    public int day; // 第N天（主键）
    public NPCommonCostItem special_item; // 特殊奖励（可能为空）
    public List<NPCommonCostItem> item_list = new ArrayList<>(); // 普通每日奖励列表
    public int buff_id; // 关联Buff配置Id
    public NPPlayerConditionGroupObj condition = new NPPlayerConditionGroupObj(); // 条件表达式

    // ============================= 业务辅助方法 ============================= //

    /**
     * 判断是否存在特殊奖励。
     * @return true = 有特殊奖励；false = 无特殊奖励
     */
    public boolean hasSpecialItem()
    {
        return special_item != null && special_item.getItemId() > 0;
    }
}
