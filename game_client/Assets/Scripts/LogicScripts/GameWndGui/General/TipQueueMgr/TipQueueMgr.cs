using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    
    /// <summary>
    /// 提示队列管理器
    /// </summary>
    public class TipQueueMgr
    {
        private static TipQueueMgr _g_instance = new TipQueueMgr();
        [NotNull] public static TipQueueMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new TipQueueMgr();

                return _g_instance;
            }
        }

        //这一轮需要展示的tip合集
        [NotNull] private TipQueueShowRound _m_tipShowRound;
        //等待展示的tip列表
        [NotNull] private List<_ABaseTipQueueDealer> _m_lWaitShowDealerList;
        //暂停展示的tip列表
        [NotNull] private List<_ABaseTipQueueDealer> _m_lPauseShowDealerList;
        //需要暂停的tip类型
        [NotNull] private HashSet<ETipQueueType> _m_lPauseType;
        //是否正在展示
        private bool _m_bIsShowingTip;

        public TipQueueMgr()
        {
            _m_lWaitShowDealerList = new List<_ABaseTipQueueDealer>();
            _m_lPauseShowDealerList = new List<_ABaseTipQueueDealer>();
            _m_lPauseType = new HashSet<ETipQueueType>();
            _m_tipShowRound = new TipQueueShowRound();
            _m_tipShowRound.onShowDone += _onRoundShowDone;
            _m_bIsShowingTip = false;
        }

        /// <summary>
        /// 添加一个集合的处理对象
        /// </summary>
        public void addDealer(params _ABaseTipQueueDealer[] _dealerList)
        {
            if (_dealerList == null || _dealerList.Length <= 0)
                return;

            //如果当前正在展示中，先添加tip到等待队列里
            if (_m_bIsShowingTip)
            {
                for (int i = 0; i < _dealerList.Length; i++)
                {
                    _m_lWaitShowDealerList.Add(_dealerList[i]);
                }

                //检查一下tip展示是否超时导致一直在展示中
                _m_tipShowRound.checkTimeout();
                return;
            }

            _ABaseTipQueueDealer tmp = null;
            for (int i = 0; i < _dealerList.Length; i++)
            {
                tmp = _dealerList[i];
                if (null == tmp)
                    continue;

                //如果是需要暂停展示的类型，先存放到暂停列表里
                if (_m_lPauseType.Contains(tmp.tipType))
                {
                    _m_lPauseShowDealerList.Add(tmp);
                    continue;
                }

                //添加到指定类型
                _m_tipShowRound.addDealer(tmp);
            }

            //在同一帧都添加完tip后下一帧再开始展示
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (!_m_bIsShowingTip)
                {
                    _m_bIsShowingTip = true;
                    _m_tipShowRound.startShowTip();
                }
            });
        }

        /// <summary>
        /// 暂停指定tip类型展示
        /// </summary>
        /// <param name="_type"></param>
        public void pauseShowType(ETipQueueType _type)
        {
            _m_lPauseType.Add(_type);
        }

        /// <summary>
        /// 恢复指定tip类型展示
        /// </summary>
        /// <param name="_type"></param>
        public void resumeShowType(ETipQueueType _type)
        {
            _m_lPauseType.Remove(_type);
            _ABaseTipQueueDealer[] dealerList = _m_lPauseShowDealerList.ToArray();
            _m_lPauseShowDealerList.Clear();
            addDealer(dealerList);
        }

        /// <summary>
        /// 展示完一轮回调
        /// </summary>
        private void _onRoundShowDone()
        {
            _m_bIsShowingTip = false;

            //如果全部展示完一轮了，添加等待队列里的tip开始进入展示
            if (_m_lWaitShowDealerList.Count > 0)
            {
                _ABaseTipQueueDealer[] dealerList = _m_lWaitShowDealerList.ToArray();
                _m_lWaitShowDealerList.Clear();
                addDealer(dealerList);
            }
        }
    }
}