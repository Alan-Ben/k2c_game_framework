package NPGameRes.Refs.Inn;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;

@RefTable(tableName = "inn_medal_level")
public class RefInnMedalLevel extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnMedalLevelMgr _g_mgr = new RefInnMedalLevelMgr();

    // 2. 静态方法
    public static RefInnMedalLevelMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnMedalLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnMedalLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnMedalLevel newRef = (RefInnMedalLevel) _newRef;
        level = newRef.level;
        need_inn_level = newRef.need_inn_level;
        bonus_prop_modifier = newRef.bonus_prop_modifier;
    }

    @Override
    public long Id()
    {
        return level;
    }

    // 4. 管理器内部类
    public static class RefInnMedalLevelMgr extends RefTableContainer<RefInnMedalLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public int level; // 勋章等级（主键）
    public int need_inn_level; // 所需旅店等级
    public PlayerBonusPropertyModifier bonus_prop_modifier; // 加成
}