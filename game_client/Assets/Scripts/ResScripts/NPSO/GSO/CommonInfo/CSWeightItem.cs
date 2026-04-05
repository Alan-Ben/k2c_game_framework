using System;

namespace GOE
{
    /// <summary>
    /// 单权重对象
    /// </summary>
    [Serializable]
    public class CSWeightItem
    {
        public long dataId;
        public int weight;

        public CSWeightItem(long _dataId, int _weight)
        {
            dataId = _dataId;
            weight = _weight;
        }
    }
}