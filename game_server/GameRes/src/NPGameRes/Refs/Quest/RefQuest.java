package NPGameRes.Refs.Quest;

import Common.QuestEnum.EQuestType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENpRewardShowType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "quest")
public class RefQuest extends RefBase
{
    private static RefTableContainer<RefQuest> _g_mgr = new RefTableContainer<RefQuest>();

    public static RefTableContainer<RefQuest> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefQuest> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefQuest>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefQuest newRef = (RefQuest) _newRef;
        quest_id = newRef.quest_id;
        next_quest_id = newRef.next_quest_id;
        first_step_id = newRef.first_step_id;
        start_cost_item_list = newRef.start_cost_item_list;
        drop_cost_item_list = newRef.drop_cost_item_list;
        ext_drop_effect = newRef.ext_drop_effect;
        is_drop = newRef.is_drop;
        done_count = newRef.done_count;
        done_gain_item_list = newRef.done_gain_item_list;
        tip_reward = newRef.tip_reward;
        start_condition = newRef.start_condition;
        quest_type = newRef.quest_type;
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
        return quest_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long quest_id; //任务Id
    public long next_quest_id; //下一个任务
    public long first_step_id; //第一步骤 id
    public ArrayList<NPCommonCostItem> start_cost_item_list = new ArrayList<>(); //开启任务扣除的物品列表
    public ArrayList<NPCommonCostItem> drop_cost_item_list = new ArrayList<>(); //放弃任务扣除的物品列表
    public NPPlayerEffectListParse ext_drop_effect; //放弃任务的额外效果
    public boolean is_drop; //是否可以主动放弃
    public int done_count; //可以完成的次数
    public ArrayList<NPCommonCostItem> done_gain_item_list = new ArrayList<>(); //完成最后一个step的时候获得的奖励，与step奖励合并展示
    public ENpRewardShowType tip_reward;//是否使用tip样式展示奖励，针对done_gain_item_list
    public NPPlayerConditionGroupObj start_condition; //开启条件
    public EQuestType quest_type = EQuestType.NONE;

    @RefField(isIgnore = true)
    public List<RefQuestStep> listStep = new ArrayList<>();
    public void setListStep(List<RefQuestStep> _listStep) {listStep = _listStep;}

    /**
     * 根据stepId获取对应的步骤配置，若没有找到对应stepId的配置，则返回stepId之前最近的一个步骤配置
     * @param _step
     * @return
     */
    public RefQuestStep getNearStepRef(long _step)
    {
        List<RefQuestStep> questStepRefList = this.listStep;
        if(null == questStepRefList || questStepRefList.isEmpty())
            return null;

        long maxStepId = 0;
        RefQuestStep targetStepRef = null;
        for(int i = 0; i < questStepRefList.size(); i++)
        {
            RefQuestStep stepRef = questStepRefList.get(i);
            if(null == stepRef)
                continue;

            if(maxStepId < stepRef.step_id)
                maxStepId = stepRef.step_id;

            if(null == targetStepRef || _step > stepRef.step_id)
                targetStepRef = stepRef;
            else
                break;
        }
        //如果传入的stepId大于配置表里最大的stepId，则返回最大的stepId对应的配置
        if(_step > maxStepId && null == targetStepRef)
        {
            targetStepRef = questStepRefList.get(questStepRefList.size() - 1);
        }

        return targetStepRef;
    }

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
