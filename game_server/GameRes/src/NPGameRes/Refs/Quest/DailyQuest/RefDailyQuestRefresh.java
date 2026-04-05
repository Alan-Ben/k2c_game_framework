package NPGameRes.Refs.Quest.DailyQuest;

import Common.QuestEnum.EDailyQuestType;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;


@RefTable(tableName = "daily_quest_refresh")
public class RefDailyQuestRefresh extends RefBase
{
    private static RefTableContainer<RefDailyQuestRefresh> _g_mgr = new RefTableContainer<RefDailyQuestRefresh>();

    public static RefTableContainer<RefDailyQuestRefresh> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDailyQuestRefresh> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDailyQuestRefresh>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyQuestRefresh newRef = (RefDailyQuestRefresh) _newRef;
        daily_quest_type = newRef.daily_quest_type;
        daily_quest_id_list = newRef.daily_quest_id_list;
        random_group_id_list = newRef.random_group_id_list;
        refresh_clock = newRef.refresh_clock;
        fresh_item_list = newRef.fresh_item_list;
        mail_id = newRef.mail_id;
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
        return daily_quest_type.ordinal();
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public EDailyQuestType daily_quest_type = EDailyQuestType.NONE;//日常任务类型枚举
    public ArrayList<Long> daily_quest_id_list = new ArrayList<>();//固定任务id
    public ArrayList<Long> random_group_id_list = new ArrayList<>();//随机任务 id 组 (分组id;分组id)
    public NPRefreshTimeObj refresh_clock = new NPRefreshTimeObj();//刷新规则(ENPTimeRefreshType)
    public ArrayList<NPCommonItem> fresh_item_list = new ArrayList<>();//任务刷新时需要删除道具
    public long mail_id;//未领取补偿邮件id

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    @RefField(isIgnore = true)
    public List<RefDailyQuestActiveReward> _m_activeRewardList = new ArrayList<>();

    public void setActiveRewardList(List<RefDailyQuestActiveReward> _refList)
    {
        _m_activeRewardList = _refList;
    }
}
