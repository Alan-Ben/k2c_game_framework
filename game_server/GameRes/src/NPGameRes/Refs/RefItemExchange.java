package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPEnum.EExchangeItemDealType;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.HashMap;

@RefTable(tableName = "item_exchange")
public class RefItemExchange extends RefBase
{
    private static RefItemExchangeMgr _g_mgr = new RefItemExchangeMgr();

    public static RefItemExchangeMgr getMgr()
    {
        return _g_mgr;
    }


    public static class RefItemExchangeMgr extends RefListContainer<RefItemExchange>
    {

        public static class RefItemExchangeSubMap
        {
            //<生效时机， refList>
            private HashMap<EExchangeItemDealType, ArrayList<RefItemExchange>> _m_itemExchangeMap = new HashMap<>();

            public void add(RefItemExchange _ref)
            {
                //遍历所有生效时机，在生效时机对应的list中添加这个ref
                for (EExchangeItemDealType dealType : _ref.deal_type)
                {
                    if (dealType == null)
                    {
                        continue;
                    }
                    //放入查询map中
                    ArrayList<RefItemExchange> refList = _m_itemExchangeMap.computeIfAbsent(dealType, k -> new ArrayList<>());
                    refList.add(_ref);
                }
            }

            /**
             * 根据处理类型查询所有兑换配置
             * @param _dealType
             * @return
             */
            public ArrayList<RefItemExchange> lookup(EExchangeItemDealType _dealType)
            {
                ArrayList<RefItemExchange> refList = _m_itemExchangeMap.get(_dealType);
                if (refList == null)
                {
                    return new ArrayList<>();
                }
                return refList;
            }
        }

        /**
         * 快速检索map <RefItemExchange::makeKey , ref>
         */
        private HashMap<Long, RefItemExchangeSubMap> _m_itemExchangeMap = new HashMap<>();

        @Override
        public void onLoaded()
        {
            //构造快速检索map ，key值用合成key，
            for (RefItemExchange ref : getList())
            {
                if (ref == null)
                {
                    continue;
                }
                long key = makeKey(ref.ori_item.getItemType().ordinal(), ref.ori_item.getItemId());
                RefItemExchangeSubMap subMap = _m_itemExchangeMap.get(key);
                if (subMap == null)
                {
                    subMap = new RefItemExchangeSubMap();
                    _m_itemExchangeMap.put(key, subMap);
                }
                subMap.add(ref);
            }
        }


        /**
         * 构造查询key
         */
        private long makeKey(int _itemType, long _itemId)
        {
            return _itemId * 10000L + _itemType;
        }

        /**
         * 查找转换配置
         * @param _type   物品类型
         * @param _itemId 物品id
         * @return NPCommonItem
         */
        public ArrayList<RefItemExchange> lookupExchange(ENPItemType _type, long _itemId, EExchangeItemDealType _deal_type)
        {
            ArrayList<RefItemExchange> retList = new ArrayList<>();
            //构造查询key
            long key = makeKey(_type.ordinal(), _itemId);
            //用key获取subMap
            RefItemExchangeSubMap subMap = _m_itemExchangeMap.get(key);
            if (subMap == null)
            {
                return retList;
            }

            return subMap.lookup(_deal_type);
        }
    }


    //////////////////////////////
    @Override
    public RefItemExchangeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefItemExchangeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefItemExchange newRef = (RefItemExchange) _newRef;
        id = newRef.id;
        ori_item = newRef.ori_item;
        cond = newRef.cond;
        target_item = newRef.target_item;
        deal_type = newRef.deal_type;
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


    //////////////////////////////
    public long id;
    public NPCommonItem ori_item;//原物品
    public NPPlayerConditionGroupObj cond;//转换条件
    public NPCommonCostItem target_item;//新物品
    public ArrayList<EExchangeItemDealType> deal_type = new ArrayList<>();//转换生效时机

    ///////////////////////////////
}
