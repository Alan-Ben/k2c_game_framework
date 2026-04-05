package NPGameRes.Refs.Museum;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

import java.util.List;

@RefTable(tableName = "museum_item")
public class RefMuseumItem extends RefBase
{
    // 1. 静态管理器实例
    private static RefMuseumItemMgr _g_mgr = new RefMuseumItemMgr();

    // 2. 静态方法
    public static RefMuseumItemMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefMuseumItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMuseumItemMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMuseumItem newRef = (RefMuseumItem) _newRef;
        id = newRef.id;
        upgrade_cost_group_id = newRef.upgrade_cost_group_id;
        quality = newRef.quality;
        name = newRef.name;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefMuseumItemMgr extends RefTableContainer<RefMuseumItem>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 礼物ID（主键）
    public long upgrade_cost_group_id; // 升级消耗组ID
    public EQuality quality; // 品质
    public String name; // 名称

    @RefField(isIgnore = true)
    private List<RefMuseumItemLevel> refLevelList; // 礼物等级列表

    public void setLevelList(List<RefMuseumItemLevel> _refLevelList)
    {
        refLevelList = _refLevelList;
    }

    /**
     * 根据等级查找对应的礼物等级配置
     * @param _level
     * @return
     */
    public RefMuseumItemLevel lookupLevelRef(int _level)
    {
        if (refLevelList == null)
            return null;

        for (RefMuseumItemLevel ref : refLevelList)
        {
            if (ref.level == _level)
            {
                return ref;
            }
        }
        return null;

    }
}