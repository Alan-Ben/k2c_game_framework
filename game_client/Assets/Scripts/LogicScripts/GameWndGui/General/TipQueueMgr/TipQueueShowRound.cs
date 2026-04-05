using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 每轮展示的类型队列管理对象
    /// </summary>
    public class TipQueueShowRound
    {
        //类型队列列表
        [NotNull] private List<TipQueueTypeInfo> _m_tipQueueTypeInfoList;
        //展示完成回调
        private Action _m_aOnShowDone;

        /// <summary>
        /// 展示完成回调
        /// </summary>
        public Action onShowDone
        {
            get { return _m_aOnShowDone; }
            set { _m_aOnShowDone = value; }
        }

        public TipQueueShowRound()
        {
            _m_tipQueueTypeInfoList = new List<TipQueueTypeInfo>();
            int typeCount = ALCommon.getEnumCount(typeof(ETipQueueType));
            for (int i = 0; i < typeCount; i++)
            {
                ETipQueueType type = (ETipQueueType) i;
                if(type == ETipQueueType.NONE)
                    continue;

                TipQueueTypeInfo info = new TipQueueTypeInfo((ETipQueueType)i);
                info.dealAllDoneAction += _setTypeDealerDone;
                _m_tipQueueTypeInfoList.Add(info);
            }

            //按照优先级从小到大排序
            _m_tipQueueTypeInfoList.Sort((_a, _b) =>
            {
                QueueDealerTipsRefObj aRef = GRefdataCoreMgr.instance.queueDealerTipMap.getRef((long)_a.tipType);
                QueueDealerTipsRefObj bRef = GRefdataCoreMgr.instance.queueDealerTipMap.getRef((long)_b.tipType);
                if (aRef == null || bRef == null)
                    return 0;

                return aRef.priority_id.CompareTo(bRef.priority_id);
            });
        }

        /// <summary>
        /// 开始展示tip
        /// </summary>
        public void startShowTip()
        {
            //寻找一个需要展示的类型开始展示
            for (int i = 0; i < _m_tipQueueTypeInfoList.Count; i++)
            {
                if (_m_tipQueueTypeInfoList[i] != null && !_m_tipQueueTypeInfoList[i].isAllDone)
                {
                    _m_tipQueueTypeInfoList[i].startShowTip();
                    return;
                }
            }

            //未找到可展示的说明全部展示完了，执行回调
            _m_aOnShowDone?.Invoke();
        }

        /// <summary>
        /// 添加tip
        /// </summary>
        /// <param name="_dealer"></param>
        public void addDealer(_ABaseTipQueueDealer _dealer)
        {
            //添加到指定类型
            TipQueueTypeInfo tipTypeInfo = _getTipTypeInfo(_dealer.tipType);
            tipTypeInfo.addDealer(_dealer);
        }

        /// <summary>
        /// 检查展示是否超时，超时会自动完成展示避免堵塞队列
        /// </summary>
        public void checkTimeout()
        {
            for (int i = 0; i < _m_tipQueueTypeInfoList.Count; i++)
            {
                if (_m_tipQueueTypeInfoList[i] != null)
                    _m_tipQueueTypeInfoList[i].checkTimeout();
            }
        }

        /// <summary>
        /// 获取提示类型
        /// </summary>
        /// <param name="_tipType"></param>
        /// <returns></returns>
        private TipQueueTypeInfo _getTipTypeInfo(ETipQueueType _tipType)
        {
            for (int i = 0; i < _m_tipQueueTypeInfoList.Count; i++)
            {
                if (_m_tipQueueTypeInfoList[i] != null && _m_tipQueueTypeInfoList[i].tipType == _tipType)
                    return _m_tipQueueTypeInfoList[i];
            }

            Debug.LogError($"【TipQueueMgr】未获取到{_tipType}类型数据，请检查queue_dealer_tips配表");
            return null;
        }

        /// <summary>
        /// 尝试处理下一个
        /// </summary>
        private void _setTypeDealerDone()
        {
            startShowTip();
        }
    }
}