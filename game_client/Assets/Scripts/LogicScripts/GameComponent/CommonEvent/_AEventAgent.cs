using System;

namespace GOE
{
    /// <summary>
    /// 事件处理代理抽象类
    /// </summary>
    public abstract class _AEventAgent : _IEventDealAgent
    {
        protected bool _m_bIsDealingEvent;//是否正在处理事件
        protected bool _m_bIsReqDealEvent;//是否正在向服务器请求处理事件

        public bool isDealingEvent { get { return _m_bIsDealingEvent; } }
        public bool isReqDealEvent { get { return _m_bIsReqDealEvent; } }
        
        public abstract bool isEventDoneDataLevel { get; }

        public _AEventAgent()
        {
            _m_bIsDealingEvent = false;
            _m_bIsReqDealEvent = false;
        }

        #region 处理事件

        public virtual void dealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (isEventDoneDataLevel)//若事件已经完成, 返回
            {
                _dealDone?.Invoke();
                return;
            }

            if (_m_bIsDealingEvent)//若正在处理事件, 返回
            {
                Debug.LogError("[_AEventAgent dealEvent] 当前事件正在处理中, 但是又调用了一次dealEvent处理方法, 请注意进行操作屏蔽\n" +
                               "或者检查是否在本事件上次esc退出或者点击背景退出或者其他方式退出时有没有调用break回调");
                return;
            }

            _m_bIsDealingEvent = true;
            
            //调用子类处理方法
            _dealEvent(_dealAddInfo, _startDeal, ()=>
            {
                _m_bIsDealingEvent = false;
                
                _dealDone?.Invoke();
            }, ()=>
            {
                _m_bIsDealingEvent = false;
                
                _break?.Invoke();
            });
        }

        /// <summary>
        /// 子类重写的事件处理方法
        /// </summary>
        /// <param name="_dealDone"></param>
        protected abstract void _dealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break);
        
        /// <summary>
        /// 向服务器请求处理事件
        /// </summary>
        public virtual void reqDealEvent(byte[] _dealEventInfo, Action<bool, byte[]> _reqCallBack)
        {
            if (_m_bIsReqDealEvent)
            {
                Debug.LogError("[_ASimpleEventAgent reqDealEvent] 当前正在向服务器请求处理事件, 但是又调用了一次reqDealEvent请求处理事件, 请注意进行操作屏蔽");
                return;
            }

            _m_bIsReqDealEvent = true;

            _realReqDealEvent(_dealEventInfo, (_isSucc, _retMsg)=>
            {
                if (!_m_bIsDealingEvent)//若收到回包时已经不在处理事件了, 那么也不需要调用回调
                {
                    _m_bIsReqDealEvent = false;
                    return;
                }
                
                _reqCallBack?.Invoke(_isSucc, _retMsg);

                _m_bIsReqDealEvent = false;
            });
        }

        /// <summary>
        /// 具体的向服务器请求处理方法
        /// </summary>
        /// <param name="_dealEventInfo"></param>
        /// <param name="_reqCallBack"></param>
        protected abstract void _realReqDealEvent(byte[] _dealEventInfo, Action<bool, byte[]> _reqCallBack);

        #endregion

        #region 自动处理事件

        public virtual void autoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            if (isEventDoneDataLevel)//若事件已经完成, 返回
            {
                _dealDone?.Invoke();
                return;
            }

            if (_m_bIsDealingEvent)//若正在处理事件, 返回
            {
                Debug.LogError("[_AEventAgent dealEvent] 当前事件正在处理中, 但是又调用了一次autoDealEvent处理方法, 请注意进行操作屏蔽\n" +
                               "或者检查是否在本事件上次esc退出或者点击背景退出或者其他方式退出时有没有调用break回调");
                return;
            }

            if (!canAutoDealEvent)
            {
                Debug.LogError($"[autoDealEvent] 当前事件不支持自动处理, 若需要支持自动处理, 请重写canAutoDealEvent为true, 并且实现自动处理方法_autoDealEvent");
                _break?.Invoke();
            }
                
            _m_bIsDealingEvent = true;
            //调用子类处理方法
            _autoDealEvent(_dealAddInfo,()=>
            {
                _m_bIsDealingEvent = false;
                
                _dealDone?.Invoke();
            }, ()=>
            {
                _m_bIsDealingEvent = false;

                _break?.Invoke();
            });
        }
        
        /// <summary>
        /// 是否可以自动处理事件
        /// </summary>
        public abstract bool canAutoDealEvent { get; }
        
        /// <summary>
        /// 子类重写的事件自动处理方法
        /// </summary>
        /// <param name="_dealDone"></param>
        protected abstract void _autoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break);

        /// <summary>
        /// 向服务器请求自动处理事件
        /// </summary>
        public virtual void reqAutoDealEvent(Action<bool, byte[]> _reqCallBack)
        {
            if (_m_bIsReqDealEvent)
            {
                Debug.LogError("[_ASimpleEventAgent reqAutoDealEvent] 当前正在向服务器请求处理事件, 但是又调用了一次reqAutoDealEvent请求处理事件, 请注意进行操作屏蔽");
                return;
            }

            _m_bIsReqDealEvent = true;
            
            _realReqAutoDealEvent(getEventAutoDealInfoByteArray(), (_isSucc, _retMsg)=>
            {
                if (!_m_bIsDealingEvent)//若收到回包时已经不在处理事件了, 那么也不需要调用回调
                {
                    _m_bIsReqDealEvent = false;
                    return;
                }
                
                _reqCallBack?.Invoke(_isSucc, _retMsg);
                _m_bIsReqDealEvent = false;
            });
        }

        /// <summary>
        /// 向服务器请求自动处理事件，不需要操作数据
        /// </summary>
        public virtual void reqAutoDealEventWithNoOp(Action<bool, byte[]> _reqCallBack)
        {
            if (_m_bIsReqDealEvent)
            {
                Debug.LogError("[_ASimpleEventAgent reqAutoDealEvent] 当前正在向服务器请求处理事件, 但是又调用了一次reqAutoDealEvent请求处理事件, 请注意进行操作屏蔽");
                return;
            }

            _m_bIsReqDealEvent = true;

            _realReqAutoDealEventWithNoOp((_isSucc, _retMsg)=>
            {
                if (!_m_bIsDealingEvent)//若收到回包时已经不在处理事件了, 那么也不需要调用回调
                {
                    _m_bIsReqDealEvent = false;
                    return;
                }
                
                _reqCallBack?.Invoke(_isSucc, _retMsg);
                _m_bIsReqDealEvent = false;
            });
        }
        
        /// <summary>
        /// 获取事件自动处理信息byte数组
        /// </summary>
        /// <returns></returns>
        public abstract byte[] getEventAutoDealInfoByteArray();
        
        /// <summary>
        /// 具体的向服务器请求自动处理方法
        /// </summary>
        /// <param name="_dealEventInfo"></param>
        /// <param name="_reqCallBack"></param>
        protected abstract void _realReqAutoDealEvent(byte[] _dealEventInfo, Action<bool, byte[]> _reqCallBack);
        
        /// <summary>
        /// 具体的向服务器请求自动处理方法，不需要操作数据
        /// </summary>
        /// <param name="_reqCallBack"></param>
        protected abstract void _realReqAutoDealEventWithNoOp(Action<bool, byte[]> _reqCallBack);
        
        #endregion
    }
}