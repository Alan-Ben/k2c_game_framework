using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    
    /**********************
     * UI提示信息管理对象
     **/
    public class RewardQueueMgr
    {
        private static RewardQueueMgr _g_instance = new RewardQueueMgr();
        public static RewardQueueMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new RewardQueueMgr();

                return _g_instance;
            }
        }
        //处理队列
        [NotNull] private List<_ARewardQueueDealer> _m_lDealerList;
        //是否正在处理普通队列的节点中，注意！这个变量不包含优先级节点
        private bool _m_bIsDealing;

        //是否在校验重
        private bool _m_isInCheck;
        //是否暂停处理（暂停时dealer仍入列但不触发处理）
        private bool _m_bIsPaused;
        //暂停期间是否有dealer完成过（用于恢复时区分"dealer已完成等待续链"和"dealer仍在执行中"）
        private bool _m_bDealerDoneDuringPause;
        //处理完成之后的后续处理对象
        private Action _m_dDoneAllNoticeDelegate;
        
        /// <summary>
        /// 是否有notice正在处理中
        /// </summary>
        public bool isDealing { get { return _m_bIsDealing; } }
        
        protected RewardQueueMgr()
        {
            _m_lDealerList = new List<_ARewardQueueDealer>();
            _m_dDoneAllNoticeDelegate = null;
            _m_bIsDealing = false;
            _m_isInCheck = false;
            _m_bIsPaused = false;
            _m_bDealerDoneDuringPause = false;
        }

        public void reset()
        {
            _m_lDealerList = new List<_ARewardQueueDealer>();
            _m_dDoneAllNoticeDelegate = null;
            _m_bIsDealing = false;
            _m_isInCheck = false;
            _m_bIsPaused = false;
            _m_bDealerDoneDuringPause = false;
        }
        
        /// <summary>
        /// 设置暂停状态，暂停时dealer仍入列但不触发处理，恢复时自动触发缓存的dealer
        /// </summary>
        public void setPaused(bool _paused)
        {
            if (_m_bIsPaused == _paused)
                return;

            _m_bIsPaused = _paused;
            if (_m_bIsPaused)
                return;

            bool wasDoneDuringPause = _m_bDealerDoneDuringPause;
            _m_bDealerDoneDuringPause = false;
            if (_m_lDealerList.Count > 0)
                _dealTryPopNoticeNextFrame();
            else if (wasDoneDuringPause)
                _checkDone();
        }

        public bool isPaused { get { return _m_bIsPaused; } }

        /// <summary>
        /// 尝试增加新的处理对象
        /// </summary>
        /// <param name="_dealer"></param>
        public void addDealer(_ARewardQueueDealer _dealer)
        {
            if(null == _dealer)
                return;

            _m_lDealerList.Add(_dealer);
            //暂停时只缓存不触发
            if (_m_bIsPaused)
                return;
            //尝试弹出提示
            _dealTryPopNoticeNextFrame();
        }
        
        /// <summary>
        /// 根据实例从队列中删除
        /// </summary>
        public void removeDealer(_ARewardQueueDealer _dealer)
        {
            if(null == _dealer)
                return;
            
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                if(_m_lDealerList[i] == _dealer)
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// 根据字符串从队列中删除
        /// </summary>
        public void removeDealerByTag(string _strTag)
        {
            if(string.IsNullOrEmpty(_strTag))
                return;
            
            _ARewardQueueDealer tmpDealer = null;
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                tmpDealer = _m_lDealerList[i];
                if(null == tmpDealer || string.IsNullOrEmpty(tmpDealer.noticeTag))
                    continue;
                
                if(tmpDealer.noticeTag == _strTag)
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// 清空所有未处理的dealer
        /// </summary>
        public void clearAll()
        {
            _m_lDealerList.Clear();
        }

        /*****************
         * 设置结束的处理
         **/
        public void addDoneDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            if (null == _m_dDoneAllNoticeDelegate)
                _m_dDoneAllNoticeDelegate = _delegate;
            else
                _m_dDoneAllNoticeDelegate += _delegate;
        }
        
        protected void _dealTryPopNoticeNextFrame()
        {
            if(_m_isInCheck)
                return;

            _m_isInCheck = true;
            //下一帧才处理展示
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                _tryPopNotice();
                _m_isInCheck = false;
            });
        }
        
        protected void _tryPopNotice()
        {
            //判断是否在处理过程，如在则不进行处理
            if (_m_bIsDealing)
                return;
            //暂停时不弹出
            if (_m_bIsPaused)
                return;
            //弹出提示
            __popNextNotice();

            //检查是否全部完成
            _checkDone();
        }
        
        /// <summary>
        /// 在优先节点完成的时候调用的展示下一个节点的处理
        /// </summary>
        protected void _onNoticeDone()
        {
            //暂停时中断链式执行，不再弹出下一个
            if (_m_bIsPaused)
            {
                _m_bIsDealing = false;
                _m_bDealerDoneDuringPause = true;
                return;
            }
            __popNextNotice();
            _checkDone();
        }
        
        /** 弹出提示信息 */
        private bool __popNextNotice()
        {
            _ARewardQueueDealer dealer = null;
            for(int i = 0; i < _m_lDealerList.Count; )
            {
                dealer = _m_lDealerList[i];

                //如果数据无效则删除
                if (null == dealer || dealer.isDone)
                {
                    ALLog.Error($"Notice: {dealer} in list is done!");
                    _m_lDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }

                //如果数据无效则删除
                if (!dealer.isEnable)
                {
                    _m_lDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }

                //找到不为空，且真正被允许在此时弹出的窗口才跳出
                if (dealer != null && dealer.canCurShow)
                {
                    _m_lDealerList.RemoveAt(i);
                    break;
                }

                //全部轮完都没有选到，dealer置空，否则会选最后一个
                dealer = null;

                //累加下标
                i++;
            }

            //进行处理
            if(null != dealer)
            {
                //设置在处理中
                _m_bIsDealing = true;
                dealer.showNotice();
                return true;
            }
            else
            {
                //设置不在处理中
                _m_bIsDealing = false;
                return false;
            }
        }

        /** 检查是否完成所有处理 */
        private void _checkDone()
        {
            if(!_m_bIsDealing)
            {
                //如果无需要处理的则处理完成
                Action doneDelegate = _m_dDoneAllNoticeDelegate;
                _m_dDoneAllNoticeDelegate = null;
                if(null != doneDelegate)
                    doneDelegate();
            }
        }
        
        /// <summary>
        /// 游戏中UI提示信息处理对象
        /// </summary>
        public abstract class _ARewardQueueDealer
        {
            //判断notice状态是否变更
            private long _m_lNoticeDealSerialize;

            //是否已经完结
            private bool _m_bIsDone = false;

            public long noticeDealSerialize { get { return _m_lNoticeDealSerialize; } }

            //返回是否已完成
            public bool isDone { get { return _m_bIsDone; } }

            /// <summary>
            ///  设置本处理过程完结
            /// </summary>
            public void setDealerDone()
            {
                setDealerDone(0);
            }
            public void setDealerDone(long _dealSerialize)
            {
                if (_m_bIsDone)
                    return;

                //判断序列号有效且是否一致，避免错误处理
                if (_dealSerialize > 0 && _dealSerialize != _m_lNoticeDealSerialize)
                    return;

                //设置已完成
                _m_bIsDone = true;
                //刷新显示序列号，表示问题处理完毕
                _m_lNoticeDealSerialize = ALSerializeOpMgr.next();
                //触发函数
                _dealHideNotice();
                
                RewardQueueMgr.instance._onNoticeDone();
            }

            //展示本节点的提示信息
            public void showNotice()
            {
                //刷新显示序列号，表示问题处理完毕
                _m_lNoticeDealSerialize = ALSerializeOpMgr.next();

                _dealShowNotice();
            }
            
            /// <summary>
            /// 是否还有效，如果无效会pop时候尝试删除
            /// </summary>
            public virtual bool isEnable { get { return true; } }
            /// <summary>
            /// 字符串标记，可以用来根据tag删除notice
            /// </summary>
            public virtual string noticeTag { get { return string.Empty; } }
            
            //是否现在可以从队列中提出来展示
            public abstract bool canCurShow { get; }
            
            //展示本节点的提示信息
            protected abstract void _dealShowNotice();
            //隐藏提示
            protected abstract void _dealHideNotice();
        }
    }
}
