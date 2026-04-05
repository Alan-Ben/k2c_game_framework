package NPGameRes.Refs.Quest;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPEnum.ENpRewardShowType;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "quest_step")
public class RefQuestStep extends RefBase
{
    private static RefTableContainer<RefQuestStep> _g_mgr = new RefTableContainer<RefQuestStep>();

    public static RefTableContainer<RefQuestStep> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefQuestStep> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefQuestStep>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefQuestStep newRef = (RefQuestStep) _newRef;
        step_id = newRef.step_id;
        next_step_id = newRef.next_step_id;
        receive_give_item_list = newRef.receive_give_item_list;
        done_gain_item_list = newRef.done_gain_item_list;
        tip_reward = newRef.tip_reward;
        done_cost_item_list = newRef.done_cost_item_list;
        ext_done_effect = newRef.ext_done_effect;
        continued_sec_s = newRef.continued_sec_s;
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
        return step_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long step_id; //步骤 id
    public long next_step_id; //后置任务的id
    public ArrayList<NPCommonCostItem> receive_give_item_list = new ArrayList<>(); //开启步骤给予的物品列表
    public ArrayList<NPCommonCostItem> done_gain_item_list = new ArrayList<>(); //完成后奖励物品列表
    public ENpRewardShowType tip_reward;//是否使用tip样式展示奖励，针对item_list
    public ArrayList<NPCommonCostItem> done_cost_item_list = new ArrayList<>(); //完成任务扣除的物品列表
    public NPPlayerEffectListParse ext_done_effect; //完成后的额外效果
    public int continued_sec_s;

    @RefField(isIgnore = true)
    public List<RefQuestTarget> listTarget = new ArrayList<>();
    public void setListStep(List<RefQuestTarget> _listTarget) {listTarget = _listTarget;}

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /**
     * 获取步骤截至时间
     * @return
     */
    public int getStepExpireTimeS()
    {
        return continued_sec_s > 0 ? CommonFunc.getNowTimeSec() + continued_sec_s : 0;
    }
}
