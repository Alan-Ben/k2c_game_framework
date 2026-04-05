package NPGameRes.Refs.BagItem;

import ALServerLog.ALServerLog;
import NPCommon.Enum.ERefType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENpRewardShowType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Parse.NPItemListParse;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;

@RefTable(tableName = "bag_item_use")
public class RefBagItemUse extends RefBase
{
    private static RefTableContainer<RefBagItemUse> _g_mgr = new RefTableContainer<RefBagItemUse>();

    public static RefTableContainer<RefBagItemUse> getMgr()
    {
        return _g_mgr;
    }

    public static ERefType getRefTypeStatic()
    {
        return ERefType.bag_item_use;
    }

    @Override
    public RefTableContainer<RefBagItemUse> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBagItemUse>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBagItemUse newRef = (RefBagItemUse) _newRef;
        id = newRef.id;
        use_not_cost = newRef.use_not_cost;
        cost_item_list = newRef.cost_item_list;
        option_item_list = newRef.option_item_list;
        option_count = newRef.option_count;
        get_reward_id = newRef.get_reward_id;
        is_pre_high_pri = newRef.is_pre_high_pri;
        pre_use_cond = newRef.pre_use_cond;
        pre_effects = newRef.pre_effects;
        use_cond = newRef.use_cond;
        effects = newRef.effects;
        batch_effects = newRef.batch_effects;
        tip_reward = newRef.tip_reward;
        tip_reward_multiple = newRef.tip_reward_multiple;
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

    public long id;
    public boolean use_not_cost; //使用时是否不消耗自身
    public NPItemListParse cost_item_list = new NPItemListParse(); //使用消耗

    public NPItemListParse option_item_list = new NPItemListParse(); //可选奖励列表
    public int option_count; //可选数量

    public long get_reward_id;//使用获得的reward_id

    public boolean is_pre_high_pri; //是否前置执行效果具有更高优先级，如无则两者一定都会执行，true则优先判断前置是否执行
    public NPPlayerConditionGroupObj pre_use_cond; //前置使用效果的使用条件
    public NPPlayerEffectListParse pre_effects; //前置使用效果

    public NPPlayerConditionGroupObj use_cond; //使用条件
    public NPPlayerEffectListParse effects; //使用效果
    public NPPlayerEffectListParse batch_effects; //批量使用效果
    public ENpRewardShowType tip_reward;//是否使用tip样式展示奖励
    public ENpRewardShowType tip_reward_multiple;//多个使用表现tip样式展示奖励 即大于等于general表bag_auto_use_max_count的使用次数时

    public boolean optionCheck(ArrayList<Integer> _list)
    {
        if (option_item_list.getItemTypeObjList().size() <= 1)
            return true;

        //需要策划检查配置
        if (option_count <= 0)
        {
            ALServerLog.Error("Can not use bag item for option [id:" + id + ", optionCount:" + option_count + "]");
            return false;
        }

        if (_list.size() != option_count)
            return false;

        //检查是否出现重复数据
        for (int i = 0; i < _list.size(); i++)
        {
            int cur = _list.get(i);
            for (int j = i + 1; j < _list.size(); j++)
            {
                if (cur == _list.get(j))
                    return false;
            }
        }

        //检查对应物品是否存在
        for (int i = 0; i < _list.size(); i++)
        {
            if (_list.get(i) < 0 || _list.get(i) >= option_item_list.getItemTypeObjList().size())
                return false;
        }

        return true;
    }
}
