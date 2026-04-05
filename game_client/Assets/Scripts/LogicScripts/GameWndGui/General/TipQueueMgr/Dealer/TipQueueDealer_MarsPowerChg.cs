using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星实力提示队列信息
    /// </summary>
    public class TipQueueDealer_MarsPowerChg : _ABaseTipQueueDealer
    {
        //队列销毁接力时间
        private float _m_queueTime = 0.25f;
        //旧值
        private long _m_lOriValue;
        //新值
        private long _m_lCurValue;

        public TipQueueDealer_MarsPowerChg(long _oriValue, long _curValue)
        {
            _m_lOriValue = _oriValue;
            _m_lCurValue = _curValue;
            QueueDealerTipsRefObj queueDealerTipsRefObj = GRefdataCoreMgr.instance.queueDealerTipMap.getRef((long)ETipQueueType.MARS_POWER);
            if (queueDealerTipsRefObj != null)
                _m_queueTime = queueDealerTipsRefObj.space_time;
        }

        /// <summary>
        /// 队列类型
        /// </summary>
        public override ETipQueueType tipType { get { return ETipQueueType.MARS_POWER; } }
        /// <summary>
        /// 当队列里有多个相关类型tip时是否需要将展示的值合并，true的话需要重写_dealMarge
        /// </summary>
        public override bool needMarge { get { return true; } }
        /// <summary>
        /// 旧值
        /// </summary>
        public long oriValue { get => _m_lOriValue; }
        /// <summary>
        /// 新值
        /// </summary>
        public long curValue { get => _m_lCurValue; }


        /// <summary>
        /// 开始处理展示
        /// </summary>
        protected override void _onStart()
        {
            NPGUIAddSceneCenterTip.instance.showMarsPower(_m_lOriValue, _m_lCurValue);
            ALCommonTaskController.CommonActionAddMonoTask(setDealDone, _m_queueTime);
        }

        /// <summary>
        /// 处理合并操作
        /// </summary>
        /// <param name="_dealerList"></param>
        protected override void _dealMarge(List<_ABaseTipQueueDealer> _dealerList)
        {
            if (_dealerList == null)
                return;

            for (int i = 0; i < _dealerList.Count; i++)
            {
                TipQueueDealer_MarsPowerChg marsPowerChgDealer = _dealerList[i] as TipQueueDealer_MarsPowerChg;
                if(marsPowerChgDealer == null)
                    continue;

                if (_m_lOriValue > marsPowerChgDealer.oriValue)
                    _m_lOriValue = marsPowerChgDealer.oriValue;

                if (_m_lCurValue < marsPowerChgDealer.curValue)
                    _m_lCurValue = marsPowerChgDealer.curValue;
            }
        }

        /// <summary>
        /// 完成处理的事件处理函数
        /// </summary>
        protected override void _onDealDone()
        {
        }
    }
}
