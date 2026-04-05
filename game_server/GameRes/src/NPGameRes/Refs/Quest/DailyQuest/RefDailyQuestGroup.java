package NPGameRes.Refs.Quest.DailyQuest;

import NPCommon.Game.WeightLongValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;


@RefTable(tableName = "daily_quest_group")
public class RefDailyQuestGroup extends RefBase
{
    private static RefTableContainer<RefDailyQuestGroup> _g_mgr = new RefTableContainer<RefDailyQuestGroup>();

    public static RefTableContainer<RefDailyQuestGroup> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDailyQuestGroup> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDailyQuestGroup>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyQuestGroup newRef = (RefDailyQuestGroup) _newRef;
        id = newRef.id;
        num = newRef.num;
        refresh_daily_quest_wei_list = newRef.refresh_daily_quest_wei_list;
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
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public int num;
    public WeightLongValueList refresh_daily_quest_wei_list = new WeightLongValueList();

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
