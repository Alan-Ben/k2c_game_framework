package NPGameRes.Refs.Title;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;


@RefTable(tableName = "player_title_bg")
public class RefPlayerTitleBg extends RefBase
{
    private static RefTableContainer<RefPlayerTitleBg> _g_mgr = new RefTableContainer<RefPlayerTitleBg>();

    public static RefTableContainer<RefPlayerTitleBg> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerTitleBg> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerTitleBg>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerTitleBg newRef = (RefPlayerTitleBg) _newRef;
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
