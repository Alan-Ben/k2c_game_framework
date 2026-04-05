package NPCommon.CommonObj;

import ALBasicServer.ALBasicMutex.MutexAtom;
import GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENpRewardShowType;

import java.util.ArrayList;
import java.util.List;

public class NPItemCollector
{
    private int _m_iContextId;
    //奖励列表
    private List<NPCommon_ItemInfo> _m_ItemList = new ArrayList<>();
    //非合并物品
    private List<NPCommon_ItemInfo> _m_noMergeItemList = new ArrayList<>();

    protected MutexAtom _m_mutex;

    public NPItemCollector(int _contextId)
    {
        _m_iContextId = _contextId;

        _m_mutex = new MutexAtom();
        //降低1000级，避免锁错误
        _m_mutex.reducePriority(1000);
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public int getContextId()
    {
        return _m_iContextId;
    }

    /**
     * 奖励是否为空
     * @return
     */
    public boolean isEmpty()
    {
        _lock();
        try
        {
            return _m_ItemList.isEmpty() && _m_noMergeItemList.isEmpty();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找物品
     * @param _itemType
     * @param _subId
     * @return
     */
    public NPCommon_ItemInfo lookupItem(int _itemType, long _subId)
    {
        _lock();
        try
        {
            for (NPCommon_ItemInfo _item : _m_ItemList)
            {
                if (_item.getItemType() == _itemType && _item.getSubId() == _subId)
                {
                    return _item;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保物品存在
     * @param _itemType
     * @param _subId
     * @return
     */
    private NPCommon_ItemInfo ensureItem(int _itemType, long _subId)
    {
        _lock();
        try
        {
            NPCommon_ItemInfo item = lookupItem(_itemType, _subId);
            if (null == item)
            {
                item = new NPCommon_ItemInfo(_itemType, _subId, 0, null);
                _m_ItemList.add(item);
            }
            return item;
        } finally
        {
            _unlock();
        }
    }

    public void addItem(int _itemType, long _subId, long _num)
    {
        _lock();
        try
        {
            NPCommon_ItemInfo item = ensureItem(_itemType, _subId);
            item.setCount(item.getCount() + _num);
        } finally
        {
            _unlock();
        }
    }

    public void addItem(ENPItemType _itemType, long _subId, long _num)
    {
        addItem(_itemType.ordinal(), _subId, _num);
    }
    
    public void addItem(NPCommonItem item, long _count)
    {
        addItem(item.getItemType(), item.getItemId(), _count);
    }

    public void addItem(NPCommonCostItem item)
    {
        addItem(item.getItemType(), item.getItemId(), item.getCount());
    }

    public void addItem(NPCommon_ItemInfo item)
    {
        addItem(item.getItemType(), item.getSubId(), item.getCount());
    }

    public void addItemList(List<NPCommonCostItem> _itemList)
    {
        for (NPCommonCostItem item : _itemList)
        {
            addItem(item);
        }
    }

    public void addItemListP(List<NPCommon_ItemInfo> _itemList)
    {
        for (NPCommon_ItemInfo item : _itemList)
        {
            addItem(item);
        }
    }

    public void addNoMergeItem(NPCommon_ItemInfo _itemInfo)
    {
        _lock();
        try
        {
            _m_noMergeItemList.add(_itemInfo);
        } finally
        {
            _unlock();
        }
    }

    /*******
     * 增加不堆叠物品
     * @param _eItemType
     * @param _subId
     * @param _num
     */
    public void addNoMergeItem(ENPItemType _eItemType, long _subId, long _num)
    {
        _lock();
        try
        {
            _m_noMergeItemList.add(new NPCommon_ItemInfo(_eItemType.ordinal(), _subId, _num, null));
        } finally
        {
            _unlock();
        }
    }

    public void addNoMergeItem(NPCommonCostItem _costItem)
    {
        _lock();
        try
        {
            _m_noMergeItemList.add(_costItem.toProto());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加不合并的道具
     * @param _itemList
     */
    public void addNoMergeItemList(List<NPCommonCostItem> _itemList)
    {
        for (NPCommonCostItem npCommonCostItem : _itemList)
        {
            addNoMergeItem(npCommonCostItem);
        }
    }

    /**************************
     * 封装奖励列表协议
     * @return
     */
    public GS2GC_007_050_OnGainItemList toProto()
    {
        return toProto(ENpRewardShowType.DEFAULT);
    }

    public GS2GC_007_050_OnGainItemList toProto(ENpRewardShowType _rewardShowType)
    {
        _lock();
        try
        {
            GS2GC_007_050_OnGainItemList proto = new GS2GC_007_050_OnGainItemList();
            if (isEmpty()) return proto;

            proto.setRewardShowType(_rewardShowType);
            proto.getItemList().addAll(_m_ItemList);
            proto.getItemList().addAll(_m_noMergeItemList);
            return proto;
        } finally
        {
            _unlock();
        }
    }

    /****
     * 填充协议列表
     * @param _recList
     */
    public void fillProtoList(List<NPCommon_ItemInfo> _recList)
    {
        _lock();

        try
        {
            if (null == _recList)
                return;

            if (isEmpty())
                return;

            _recList.addAll(_m_ItemList);
            _recList.addAll(_m_noMergeItemList);
        } finally
        {
            _unlock();
        }
    }

    /******
     * 返回所有物品列表
     * @return
     */
    public List<NPCommonCostItem> getAllItemList()
    {
        _lock();
        try
        {
            ArrayList<NPCommonCostItem> items = new ArrayList<>();
            items.addAll(CommonFunc.protoItemListToCostItemList(_m_ItemList));
            items.addAll(CommonFunc.protoItemListToCostItemList(_m_noMergeItemList));
            return items;
        } finally
        {
            _unlock();
        }
    }

    public void clear()
    {
        _lock();
        try
        {
            _m_ItemList.clear();
            _m_noMergeItemList.clear();
        } finally
        {
            _unlock();
        }
    }
}
