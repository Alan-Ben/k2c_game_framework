package NPGameRes.Refs.Title;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;


@RefTable(tableName = "player_title_suffix")
public class RefPlayerTitleSuffix extends RefBase
{
    private static RefTableContainer<RefPlayerTitleSuffix> _g_mgr = new RefTableContainer<RefPlayerTitleSuffix>();

    public static RefTableContainer<RefPlayerTitleSuffix> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerTitleSuffix> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerTitleSuffix>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerTitleSuffix newRef = (RefPlayerTitleSuffix) _newRef;
        id = newRef.id;
        unlock_condition = newRef.unlock_condition;
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
    ////////////////////////

    public long id;
    public NPPlayerConditionGroupObj unlock_condition;
}
