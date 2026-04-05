package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "hero_step")
public class RefHeroStep extends RefBase
{
    private static RefTableContainer<RefHeroStep> _g_mgr = new RefTableContainer<RefHeroStep>();

    public static RefTableContainer<RefHeroStep> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroStep> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroStep>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroStep newRef = (RefHeroStep) _newRef;
        step = newRef.step;
        level_limit = newRef.level_limit;
        cost_item_list = newRef.cost_item_list;
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
        return step;
    }

    public int step;//阶段
    public int level_limit;//骑士等级上限
    public List<NPCommonCostItem> cost_item_list;//升到当前阶消耗物品列表
}
