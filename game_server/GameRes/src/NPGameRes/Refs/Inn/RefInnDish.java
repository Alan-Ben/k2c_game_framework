package NPGameRes.Refs.Inn;

import CommonEnum.ESpecAttrType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "inn_dish")
public class RefInnDish extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnDishMgr _g_mgr = new RefInnDishMgr();

    // 2. 静态方法
    public static RefInnDishMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnDishMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnDishMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnDish newRef = (RefInnDish) _newRef;
        id = newRef.id;
        unlock_condition = newRef.unlock_condition;
        need_station_id_list = newRef.need_station_id_list;
        upgrade_group_id = newRef.upgrade_group_id;
        basic_attr = newRef.basic_attr;
        bonus_prop_modifier_per_level = newRef.bonus_prop_modifier_per_level;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnDishMgr extends RefTableContainer<RefInnDish>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 菜品ID（主键）
    public NPPlayerConditionGroupObj unlock_condition; // 解锁条件
    public List<Long> need_station_id_list; // 菜品关联设施ID列表
    public long upgrade_group_id; // 菜品升级组ID
    public ESpecAttrType basic_attr; // 菜品相性
    public PlayerBonusPropertyModifier bonus_prop_modifier_per_level; // 每级加成值
}