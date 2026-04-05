package NPGameRes.Refs.Shop;

import NPCommon.Game.PropValueList;
import NPCommon.Game.WeightValueList;
import NPCommon.Log.CommLog;

import java.util.ArrayList;
import java.util.List;

public class ShopItemGroupDrop
{
    private RefShopItemGroup _m_ref;//配表
    private long _m_baseDropNum;//基础掉落数量
    private long _m_baseDiscountNum;//基础折扣数量

    private PropValueList<RefShopItem> _m_discountPropValueList = new PropValueList<>(); //概率掉落列表
    private WeightValueList<RefShopItem> _m_discountWeightValueList = new WeightValueList<>(); //权重掉落列表

    private PropValueList<RefShopItem> _m_propValueList = new PropValueList<>(); //概率掉落列表
    private WeightValueList<RefShopItem> _m_weightValueList = new WeightValueList<>(); //权重掉落列表

    public ShopItemGroupDrop(RefShopItemGroup _ref, long _baseDropNum, long _baseDiscountNum)
    {
        _m_ref = _ref;
        _m_baseDropNum = _baseDropNum;
        _m_baseDiscountNum = _baseDiscountNum;
    }

    /**
     * 初始化内部掉落列表
     * @return 是否初始化成功
     */
    public boolean init()
    {
        //检查概率掉落列表和权重掉落列表是否合法
        if (getRef().pro_shop_item_id_list.size() != getRef().pro_list.size() && getRef().shop_item_id_list.size() != getRef().wei_list.size())
        {
            CommLog.error("ShopItemGroupDrop.init() error, size not equal, RefShopItemGroup id:{}", getRef().group_id);
            return false;
        }

        for (int i = 0; i < getRef().pro_shop_item_id_list.size(); i++)
        {
            long itemId = getRef().pro_shop_item_id_list.get(i);
            RefShopItem refShopItem = RefShopItem.getMgr().get(itemId);
            if (refShopItem == null)
            {
                CommLog.error("ShopItemGroupDrop init prop list error, refShopItem not found refId:{}", itemId);
                return false;
            }

            if (i < getRef().pro_list.size())
            {
                _m_propValueList.add(refShopItem, getRef().pro_list.get(i));
                if (refShopItem.discount_group_id != 0)
                {
                    _m_discountPropValueList.add(refShopItem, getRef().pro_list.get(i));
                }
            }
        }

        for (int i = 0; i < getRef().shop_item_id_list.size(); i++)
        {
            long itemId = getRef().shop_item_id_list.get(i);
            RefShopItem refShopItem = RefShopItem.getMgr().get(itemId);
            if (refShopItem == null)
            {
                CommLog.error("ShopItemGroupDrop init weight list error, refShopItem not found refId:{}", itemId);
                return false;
            }

            if (i < getRef().wei_list.size())
            {
                _m_weightValueList.add(refShopItem, getRef().wei_list.get(i));
                if (refShopItem.discount_group_id != 0)
                {
                    _m_discountWeightValueList.add(refShopItem, getRef().wei_list.get(i));
                }
            }
        }

        return true;
    }

    public RefShopItemGroup getRef()
    {
        return _m_ref;
    }

    public long getBaseDropNum()
    {
        return _m_baseDropNum;
    }

    public long getBaseDiscountNum()
    {
        return _m_baseDiscountNum;
    }

    /**
     * 执行掉落逻辑
     * @param _extDropNum      额外需要掉落的次数
     * @param _discountItemList
     * @param _normalItemList
     */
    public void drop(long _cid, long _extDropNum, long _extDiscountNum, List<RefShopItem> _discountItemList, List<RefShopItem> _normalItemList)
    {
        //需要掉落的次数
        long needDropTimes = _extDropNum + getBaseDropNum();
        //需要折扣的次数
        long needDiscountNum = _extDiscountNum + getBaseDiscountNum();
        //折扣次数不能超过掉落次数
        if (needDiscountNum > needDropTimes)
        {
            CommLog.warn("ShopItemGroupDrop drop cid:{} needDiscountNum > needDropTimes, _extDropNum:{}, _extDiscountNum:{}", _cid, _extDropNum, _extDiscountNum);
            needDiscountNum = needDropTimes;
        }

        //先抽取折扣物品
        //复制一份掉落列表
        PropValueList<RefShopItem> discountPropValueList = _m_discountPropValueList.duplicate();
        WeightValueList<RefShopItem> discountWeightValueList = _m_discountWeightValueList.duplicate();
        for (int i = 0; i < needDiscountNum; i++)
        {
            //如果概率和权重都没有了，就不掉落了
            if (discountPropValueList.isEmpty() && discountWeightValueList.isEmpty())
                break;

            //进行一次掉落逻辑
            RefShopItem dropItem = _dropOnce(discountPropValueList, discountWeightValueList);
            if (dropItem != null)
            {
                _discountItemList.add(dropItem);
            }
        }

        //普通掉落物品列表
        //复制一份掉落列表
        PropValueList<RefShopItem> propValueList = _m_propValueList.duplicate();
        WeightValueList<RefShopItem> weightValueList = _m_weightValueList.duplicate();
        List<RefShopItem> tempDropItemList = new ArrayList<>();
        //掉落需要次数
        for (int i = 0; i < needDropTimes; i++)
        {
            //如果概率和权重都没有了，就不掉落了
            if (propValueList.isEmpty() && weightValueList.isEmpty())
                break;

            //进行一次掉落逻辑
            RefShopItem dropItem = _dropOnce(propValueList, weightValueList);
            if (dropItem != null)
            {
                tempDropItemList.add(dropItem);
            }
        }

        //从掉落列表中移除折扣物品
        for (RefShopItem discountItem : _discountItemList)
        {
            tempDropItemList.remove(discountItem);
        }

        //填充商品列表到所需数量
        _normalItemList.addAll(tempDropItemList.subList(0, (int) Math.min(Math.max(0, needDropTimes - needDiscountNum), tempDropItemList.size())));
    }

    /**
     * 按规则掉落物品列表,先随机掉落，根据结果，然后权重掉落
     * @return 掉落物品
     */
    private RefShopItem _dropOnce(PropValueList<RefShopItem> _propValueList, WeightValueList<RefShopItem> _weightValueList)
    {
        //先按概率掉落
        RefShopItem dropItem = _propValueList.randomAndRemove();
        if (dropItem != null)
            return dropItem;

        //权重掉落，不重复
        return _weightValueList.randomAndRemove();
    }
}
