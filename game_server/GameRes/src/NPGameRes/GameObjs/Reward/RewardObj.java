package NPGameRes.GameObjs.Reward;

import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.Reward.RefReward;
import NPGameRes.Refs.Reward.RefRewardSub;

import java.util.ArrayList;
import java.util.List;

public class RewardObj
{
    private RefReward _m_ref; //配表
    private List<RefRewardSub> _m_subDropList = new ArrayList<>();//子掉落列表

    public RewardObj(RefReward _ref)
    {
        _m_ref = _ref;
    }

    public RefReward getRef()
    {
        return _m_ref;
    }

    /***
     * 新增子掉落
     * @param _refSub 子掉落配表
     */
    public void addSubDrop(RefRewardSub _refSub)
    {
        if (null == _refSub)
            return;

        _m_subDropList.add(_refSub);

    }

    /******
     * 掉落物品
     * @return
     */
    public List<NPCommonCostItem> getItemList()
    {
        List<NPCommonCostItem> retList = new ArrayList<>();
        retList.addAll(getRef().certainly_drop_item_count_list);
        for (RefRewardSub refSub : _m_subDropList)
        {
            refSub.drop(retList);
        }
        return retList;
    }

    /**
     * 构造新的物品列表
     * @param _multi
     * @return
     */
    public List<NPCommonCostItem> makeNewItemList(int _multi)
    {
    	List<NPCommonCostItem> retList = new ArrayList<>();
    	//确定部分的奖励
    	for(int i = 0; i < getRef().certainly_drop_item_count_list.size(); i++)
    	{
    		NPCommonCostItem item = getRef().certainly_drop_item_count_list.get(i);
    		if(null == item)
    			continue;

    		retList.add(item.multi(_multi));
    	}
    	//掉落部分的奖励
    	List<NPCommonCostItem> dropRetList = new ArrayList<>();
        for (RefRewardSub refSub : _m_subDropList)
        {
            refSub.drop(dropRetList);
        }
        for(int i = 0; i < dropRetList.size(); i++)
        {
    		NPCommonCostItem item = dropRetList.get(i);
    		if(null == item)
    			continue;

    		retList.add(item.multi(_multi));
    	}

    	return retList;
    }
}
