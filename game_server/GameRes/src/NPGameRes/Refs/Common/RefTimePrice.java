package NPGameRes.Refs.Common;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

@RefTable(tableName = "times_price")
public class RefTimePrice extends RefBase
{
    private static RefTimePriceMgr _g_mgr = new RefTimePriceMgr();

    public static RefTimePriceMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTimePriceMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTimePriceMgr) _mgr;
    }

    public static class PriceList
    {
        private RefTimePrice _m_max_time_ref = null;
        private long _m_max_times = 0;
        private List<RefTimePrice> _m_refList = new ArrayList<>();

        public void add(int times, RefTimePrice _ref)
        {
            if (_ref == null)
                return;

            if (null == _m_max_time_ref || _m_max_times < times)
            {
                _m_max_times = times;
                _m_max_time_ref = _ref;
            }
            _m_refList.add(_ref);

        }

        public RefTimePrice lookupByTimes(int _times)
        {
            if (_times <= 0)
            {
                CommLog.error("get price invalid times:{} max:{}", _times, _m_max_times, new Exception());
                return _m_max_time_ref;
            }
            if (_times >= _m_max_times)
            {
                return _m_max_time_ref;
            }

            RefTimePrice ret = null;
            for (RefTimePrice refTimePrice : _m_refList)
            {
                //从小到大遍历
                //无结果的时候取第一个，后续只有当次数在当前购买次数之下时才会设置
                //这里要求队列按照价格提升排序
                if (ret == null || refTimePrice.times <= _times)
                {
                    ret = refTimePrice;
                } else
                {
                    break;
                }
            }

            return ret;
        }

        public void sort()
        {
            _m_refList.sort(Comparator.comparingInt(o -> o.times));
        }
    }

    public static class RefTimePriceMgr extends RefTableContainer<RefTimePrice>
    {
        private HashMap<Integer, PriceList> _m_map = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            for (RefTimePrice ref : getList())
            {
                PriceList priceList = ensurePriceList(ref.type_id);
                priceList.add(ref.times, ref);
            }
            for (PriceList priceList : _m_map.values())
            {
                priceList.sort();
            }
        }

        private PriceList ensurePriceList(int _type)
        {
            PriceList ret = _m_map.get(_type);
            if (null == ret)
            {
                ret = new PriceList();
                _m_map.put(_type, ret);
            }
            return ret;
        }

        public RefTimePrice getPrice(int _type, int _times)
        {
            PriceList priceList = _m_map.get(_type);
            if (null == priceList)
                return null;
            return priceList.lookupByTimes(_times);

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTimePrice newRef = (RefTimePrice) _newRef;
        id = newRef.id;
        type_id = newRef.type_id;
        times = newRef.times;
        item = newRef.item;
        cost_item_formula = newRef.cost_item_formula;
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
    public int type_id;
    public int times; //第几次购买及之后是这个价格
    public NPCommonItem item;//物品类型
    public NPPlayerVariableGroupObj cost_item_formula = new NPPlayerVariableGroupObj();//物品数量高级计算公式

}
