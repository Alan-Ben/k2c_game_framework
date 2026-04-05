package NPGameRes.Refs.Quest.DailyQuest;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENpRewardShowType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.ArrayList;


@RefTable(tableName = "daily_quest")
public class RefDailyQuest extends RefBase
{
    private static RefTableContainer<RefDailyQuest> _g_mgr = new RefTableContainer<RefDailyQuest>();

    public static RefTableContainer<RefDailyQuest> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDailyQuest> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDailyQuest>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyQuest newRef = (RefDailyQuest) _newRef;
        id = newRef.id;
        simple_unlock_id = newRef.simple_unlock_id;
        process_count = newRef.process_count;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        reward_item_list = newRef.reward_item_list;
        activation_item = newRef.activation_item;
        tip_reward = newRef.tip_reward;
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
    public long simple_unlock_id;//解锁id
    public long process_count;//进度条计数目标值
    public NPPlayerVariableGroupObj process_cur_count; //进度当前值（高级公式）(计数器的话可以不用配置)(进度条计数目标达到时就算完成)
    public NPCountRate trigger_count_rate;//触发器count累计到进度值的倍率
    public ArrayList<String> trigger_event = new ArrayList<>(); //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public ArrayList<NPCommonCostItem> reward_item_list = new ArrayList<>(); //奖励（未领取需要补发）
    public NPCommonCostItem activation_item;//完成获得活跃度
    public ENpRewardShowType tip_reward;//奖励展示样式（ENpRewardShowType）

}
