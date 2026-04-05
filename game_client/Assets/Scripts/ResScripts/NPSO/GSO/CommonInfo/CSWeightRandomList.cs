using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 权重随机对象
    /// </summary>
    [Serializable]
    public class CSWeightRandomList : _IParseFromStringable
    {
        [SerializeField]
        private List<CSWeightItem> _m_lItems;
        private int _m_lTotalWeight;

        public CSWeightRandomList()
        {
            _m_lItems = new List<CSWeightItem>();
            _m_lTotalWeight = 0;
        }

        /// <summary>
        /// 从字符串解析权重配置
        /// 格式: "数据1id:权重1;数据2id:权重2"
        /// </summary>
        public void ParseFromString(string config)
        {
            _m_lItems.Clear();
            _m_lTotalWeight = 0;

            if (string.IsNullOrWhiteSpace(config))
            {
                return;
            }

            string[] pairs = config.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in pairs)
            {
                if (string.IsNullOrWhiteSpace(pair))
                {
                    continue;
                }

                string[] parts = pair.Split(':');
                if (parts.Length != 2)
                {
                    return;
                }

                try
                {
                    long dataId = long.Parse(parts[0].Trim());
                    int weight = int.Parse(parts[1].Trim());

                    AddItem(dataId, weight);
                }
                catch (Exception e)
                {
                    Debug.LogError($"CSWeightRandomList ParseFromString error:{e.ToString()}");
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// 添加权重项
        /// </summary>
        public void AddItem(long dataId, int weight)
        {
            if (weight <= 0)
            {
                Debug.LogWarning($"WeightRandomList addItem Invalid weight: {weight}");
                return;
            }

            _m_lItems.Add(new CSWeightItem(dataId, weight));
            _m_lTotalWeight += weight;
        }

        /// <summary>
        /// 使用权重随机选择一个数据ID
        /// </summary>
        public long RandomSelect(CSSyncRandom random)
        {
            if (_m_lItems.Count == 0)
                return 0;

            int randomValue = random.NextInt(TotalWeight);
            int currentWeight = 0;

            foreach (CSWeightItem item in _m_lItems)
            {
                currentWeight += item.weight;
                if (randomValue < currentWeight)
                {
                    return item.dataId;
                }
            }

            // 理论上不应该到达这里
            return _m_lItems[_m_lItems.Count - 1].dataId;
        }

        /// <summary>
        /// 获取所有权重项
        /// </summary>
        public List<CSWeightItem> Items
        {
            get { return new List<CSWeightItem>(_m_lItems); }
        }

        /// <summary>
        /// 获取总权重
        /// </summary>
        public int TotalWeight
        {
            get
            {
                if (_m_lTotalWeight <= 0)
                {
                    for (int i = 0; i < _m_lItems.Count; i++)
                    {
                        _m_lTotalWeight += _m_lItems[i].weight;
                    }
                }
                return _m_lTotalWeight;
            }
        }

        /// <summary>
        /// 检查是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _m_lItems.Count == 0; }
        }

        /// <summary>
        /// 清空所有权重项
        /// </summary>
        public void Clear()
        {
            _m_lItems.Clear();
            _m_lTotalWeight = 0;
        }
    }
} 