package NPGameRes.Refs.Reward;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "reward_sub")
public class RefRewardSub extends RefBase
{
    private static RefRewardSubMgr _g_mgr = new RefRewardSubMgr();

    public static RefRewardSubMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefRewardSubMgr extends RefTableContainer<RefRewardSub>
    {
        /**
         * 配表加载完成后的回调
         *
         * 为所有 RefRewardSub 对象初始化权重掉落列表
         */
        @Override
        protected void _onTableLoaded()
        {
            for (RefRewardSub refSub : getList())
            {
                refSub._initWeightValueList();
            }
        }
    }

    @Override
    public RefRewardSubMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRewardSubMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRewardSub newRef = (RefRewardSub) _newRef;
        reward_sub_id = newRef.reward_sub_id;
        reward_id = newRef.reward_id;
        pro_item_count_list = newRef.pro_item_count_list;
        pro_list = newRef.pro_list;
        only_pro_suc_wei = newRef.only_pro_suc_wei;
        wei_count = newRef.wei_count;
        wei_item_count_list = newRef.wei_item_count_list;
        wei_list = newRef.wei_list;
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
        return reward_sub_id;
    }

    ////////////////////////
    // 配表字段

    public long reward_sub_id;//奖励子id
    public long reward_id;//奖励id

    public List<NPCommonCostItem> pro_item_count_list = new ArrayList<>();//概率掉落-物品数量列表
    public List<Long> pro_list = new ArrayList<>();//"有效概率  配置数字为万分比,概率为0的按照权重计算  一个group中的概率是累加的，多个概率相加为10000则不会进入权重计算"

    public boolean only_pro_suc_wei;//是否只有成功的时候才走wei掉落（默认失败才走wei）

    public int wei_count;//权重掉落个（不重复掉落N选X)

    public List<NPCommonCostItem> wei_item_count_list = new ArrayList<>();//权重掉落-物品数量列表

    public List<Long> wei_list = new ArrayList<>(); //权重掉落-掉落权重列表

    ////////////////////////
    // 权重掉落列表（配表数据的衍生数据）
    @RefField(isIgnore = true)
    private WeightValueList<NPCommonCostItem> _m_weightValueList = new WeightValueList<>();

    /**
     * 初始化权重掉落列表
     *
     * 从配表数据构建权重列表，供 drop() 方法使用
     * 在配表加载和热更新时调用
     */
    private void _initWeightValueList()
    {
        WeightValueList<NPCommonCostItem> weightValueList = new WeightValueList<>();
        for (int i = 0; i < wei_item_count_list.size(); i++)
        {
            if (i < wei_list.size())
            {
                weightValueList.add(wei_item_count_list.get(i), wei_list.get(i));
            }
        }

        _m_weightValueList = weightValueList;
    }

    /**
     * 按规则掉落物品列表
     *
     * 掉落规则：
     * 1. 先按概率掉落（pro_list）
     * 2. 根据 only_pro_suc_wei 标志决定是否继续权重掉落
     * 3. 权重掉落不重复（N选X）
     *
     * @param _dropOutList 输出参数，掉落的物品会添加到此列表中
     */
    public void drop(List<NPCommonCostItem> _dropOutList)
    {
        if (null == _dropOutList)
        {
            return;
        }

        // 先按概率掉落
        long rand = CommonFunc.randomInt(10000);
        int selectedIndex = -1;
        for (int i = 0; i < pro_list.size(); i++)
        {
            long pro = pro_list.get(i);
            if (pro < rand)
            {
                rand -= pro;
            }
            else // 随机数落在第i个物品，掉落成功
            {
                selectedIndex = i;
                break;
            }
        }

        if (selectedIndex >= 0) // 掉落成功了
        {
            _dropOutList.add(pro_item_count_list.get(selectedIndex));

            // 如果设定不是只有成功才进行权重，则返回
            if (!only_pro_suc_wei)
                return;
        }
        else if (only_pro_suc_wei)
        {
            // 如果设定是只有成功才进行权重，但概率掉落失败了，则返回
            return;
        }

        // 权重掉落，不重复
        WeightValueList<NPCommonCostItem> weightValueList = _m_weightValueList.duplicate();
        for (int i = 0; i < wei_count; i++)
        {
            NPCommonCostItem item = weightValueList.randomAndRemove();
            if (null == item)
            {
                break;
            }
            _dropOutList.add(item);
        }
    }
}
