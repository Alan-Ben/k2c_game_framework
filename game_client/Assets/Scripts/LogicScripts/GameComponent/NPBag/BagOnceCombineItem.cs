using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using NPEnum;
using NPCommon;
using GOE;


//一键合成数据
public class BagOnceCombineItem
{
    //目标物品
    private NPCommonCostItem _m_targetItem;

    //总原材料
    private List<NPCommonCostItem> _m_oriCostItemList;
    //总资源
    private List<NPCommonCostItem> _m_resCostItemList;

    public BagOnceCombineItem()
    {
    }

    public NPCommonCostItem targetItem { get { return _m_targetItem; } }

    public List<NPCommonCostItem> oriCostItemList { get { return _m_oriCostItemList; } }

    public List<NPCommonCostItem> resCostItemList { get { return _m_resCostItemList; } }

    public void setTargetItem(NPCommonCostItem _targetItem)
    {
        _m_targetItem = _targetItem;
    }
    /// <summary>
    /// 设置原材料消耗
    /// </summary>
    /// <param name="_oriCostItemList"></param>
    public void setOriCostItemList(List<NPCommonCostItem> _oriCostItemList)
    {
        _m_oriCostItemList = _oriCostItemList;
    }

    /// <summary>
    /// 设置资源消耗
    /// </summary>
    /// <param name="_oriCostItemList"></param>
    public void setResCostItemList(List<NPCommonCostItem> _resCostItemList)
    {
        _m_resCostItemList = _resCostItemList;

    }

    public NPCommon_SingleItemConvert toSingleConvert()
    {
        NPCommon_SingleItemConvert item = new NPCommon_SingleItemConvert();
        item.setTargetItem(_m_targetItem.toCommon_ItemInfo());

        NPCommonCostItem temp = null;
        for (int i = 0; i < _m_oriCostItemList.Count; i++)
        {
            temp = _m_oriCostItemList[i];
            if (null == temp)
                continue;
            item.addOriginItemList(temp.toCommon_ItemInfo());
        }
        return item;
    }
}