package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;

@RefTable(tableName = "hero_recommend")
public class RefHeroRecommend extends RefBase
{
    private static RefTableContainer<RefHeroRecommend> _g_mgr = new RefTableContainer<RefHeroRecommend>();
    public static RefTableContainer<RefHeroRecommend> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroRecommend> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroRecommend>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroRecommend newRef = (RefHeroRecommend) _newRef;
        id = newRef.id;
        hero_pool = newRef.hero_pool;
        choose_condition = newRef.choose_condition;
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
        return id;
    }

    public long id;// 唯一id
    public ArrayList<Long> hero_pool = new ArrayList<>();//大臣随机池
    public NPPlayerConditionGroupObj choose_condition;//选择大臣条件（条件通过表示可以获取大臣）
}
