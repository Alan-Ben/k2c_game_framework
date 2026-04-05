using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 每种价格类型对应的第一个查找到的第一个timePrice数据
        /// 目标的timePrice id = (第一个查找到的第一个timePrice的id) + (times -1)
        /// </summary>
        private Dictionary<long, List<TimesPriceRefObj>> _m_timePriceDataDic = new Dictionary<long, List<TimesPriceRefObj>>();

        /// <summary>
        /// 整理相关的timeprice数据
        /// 分类创建队列
        /// </summary>
        /// <returns></returns>
        private void _initTimePriceRef()
        {
            //查找最后一个
            TimesPriceRefObj priceRef = null;
            for (int i = 0; i < timesPriceRefCore.refList.Count; ++i)
            {
                priceRef = timesPriceRefCore.refList[i];
                if (null == priceRef)
                    continue;

                List<TimesPriceRefObj> list;
                if(!_m_timePriceDataDic.TryGetValue(priceRef.type_id, out list))
                {
                    list = new List<TimesPriceRefObj>();
                    _m_timePriceDataDic.Add(priceRef.type_id, list);
                }

                //加入队列
                list.Add(priceRef);
            }
        }

        /// <summary>
        /// 根据类型id取对应的第一个查找到的第一个timePrice数据
        /// </summary>
        /// <returns></returns>
        private List<TimesPriceRefObj> _getTimePriceList(long _typeId)
        {
            List<TimesPriceRefObj> list;
            if (_m_timePriceDataDic.TryGetValue(_typeId, out list))
            {
                return list;
            }

            return null;
        }

        /// <summary>
        /// 根据类型id和次数获取对应的 价格表数据
        /// 获取次数对应的价格信息,如果次数超过最大次数，返回的是最大的次数的价格数据
        /// </summary>
        /// <param name="_typeId">类型id</param>
        /// <param name="_times">次数</param>
        /// <returns></returns>
        public TimesPriceRefObj getTimesPriceRefObj(long _typeId, long _times)
        {
            List<TimesPriceRefObj> priceList = _getTimePriceList(_typeId);
            if (priceList == null)
                return null;

            if (_times > 100)
                _times = 100;

            TimesPriceRefObj ret = null;
            TimesPriceRefObj priceRef = null;
            for (int i = 0; i < priceList.Count; i++)
            {
                priceRef = priceList[i];
                if (null == priceRef)
                    continue;

                //无结果的时候取第一个，后续只有当次数在当前购买次数之下时才会设置
                //这里要求队列按照价格提升排序
                if (null == ret || priceRef.times <= _times)
                    ret = priceRef;
            }

            return ret;
        }

        /// <summary>
        /// 根据类型id和次数获取对应的 CostItem
        /// 获取次数对应的价格信息,如果次数超过最大次数，返回的是最大的次数的价格数据
        /// </summary>
        /// <param name="_typeId"></param>
        /// <param name="_times"></param>
        /// <returns></returns>
        public NPCommonCostItem getTimesPriceCostItem(long _typeId, long _times)
        {
            TimesPriceRefObj refObj = getTimesPriceRefObj(_typeId, _times);

            NPCommonCostItem costItem = null;
            if (refObj != null)
            {
                costItem = new NPCommonCostItem();
                costItem.item = new NPCommonItem(refObj.item);
                costItem.count = refObj.cost_item_formula.CalculateVariableResult(null);
            }

            return costItem;
        }

        /// <summary>
        /// 获取批量购买时需要消耗的数量
        /// </summary>
        /// <param name="_typeId"></param>
        /// <param name="_startTimes">开始次数</param>
        /// <param name="_times">需要消耗的总次数</param>
        /// <returns></returns>
        public long getBatchTimePriceCostItem(long _typeId,int _startTimes, long _times)
        {
            TimesPriceRefObj refObj = null;
            //需要消耗的总数量
            long costCount = 0;
            for (int i = 0; i < _times; i++)
            {
                refObj = getTimesPriceRefObj(_typeId, _startTimes + i);
                if (null == refObj)
                    continue;

                costCount += refObj.cost_item_formula.CalculateVariableResult(null);
            }

            return costCount;
        }
        
        /// <summary>
        /// 获取某类型的所有价格表数据
        /// </summary>
        /// <param name="_typeId"></param>
        /// <returns></returns>
        public List<TimesPriceRefObj> getTimesPriceList(long _typeId)
        {
            return _getTimePriceList(_typeId);
        }
    }
}