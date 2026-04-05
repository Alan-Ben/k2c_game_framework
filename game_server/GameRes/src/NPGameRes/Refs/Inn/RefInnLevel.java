package NPGameRes.Refs.Inn;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "inn_level")
public class RefInnLevel extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnLevelMgr _g_mgr = new RefInnLevelMgr();

    // 2. 静态方法
    public static RefInnLevelMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnLevel newRef = (RefInnLevel) _newRef;
        level = newRef.level;
        need_popularity = newRef.need_popularity;
        receive_guest_limit = newRef.receive_guest_limit;
    }

    @Override
    public long Id()
    {
        return level;
    }

    // 4. 管理器内部类
    public static class RefInnLevelMgr extends RefTableContainer<RefInnLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public int level;                  // 等级（主键）
    public long need_popularity;        // 所需人气值
    public int receive_guest_limit;     // 接收客人上限（作用到玩家属性上，让lazyCd取值）
}