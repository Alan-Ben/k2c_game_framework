package NPGameRes.Refs.Museum;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

@RefTable(tableName = "museum_item_level")
public class RefMuseumItemLevel extends RefBase
{
    // 1. 静态管理器实例
    private static RefMuseumItemLevelMgr _g_mgr = new RefMuseumItemLevelMgr();

    // 2. 静态方法
    public static RefMuseumItemLevelMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefMuseumItemLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMuseumItemLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMuseumItemLevel newRef = (RefMuseumItemLevel) _newRef;
        id = newRef.id;
        museum_id = newRef.museum_id;
        level = newRef.level;
        bonus = newRef.bonus;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefMuseumItemLevelMgr extends RefTableContainer<RefMuseumItemLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 唯一ID（主键）
    public long museum_id; // 珍宝ID
    public int level; // 等级
    public UnionBonus bonus; // 加成(收益/子嗣/妃子)
}