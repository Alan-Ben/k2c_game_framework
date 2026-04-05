using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 推送礼包组信息
    /// </summary>
    public class PushGiftGroupInfo
    {
        private long _m_lPushGiftGroupId;//礼包组id
        private PushGiftGroupRefObj _m_rPushGiftGroupRefObj;//礼包组配表数据
        private long _m_lLastTriggerTimeMs;//上次触发时间毫秒
        private PushGiftPackInfo _m_curPushGiftPackInfo;//当前激活的推送礼包信息
        
        private bool _m_bIsReqingTriggerGiftPack;//是否正在请求触发礼包
        private Action _m_aOnAfterReqTriggerGiftPack;//请求触发礼包后回调
        
        public PushGiftGroupInfo(long _pushGiftGroupId)
        {
            _m_lPushGiftGroupId = _pushGiftGroupId;
            _m_lLastTriggerTimeMs = 0;
            _m_curPushGiftPackInfo = null;
            _m_bIsReqingTriggerGiftPack = false;
            _m_aOnAfterReqTriggerGiftPack = null;
        }

        #region 属性

        public long pushGiftGroupId { get { return _m_lPushGiftGroupId; } }
        
        public PushGiftGroupRefObj pushGiftGroupRefObj
        {
            get
            {
                if(_m_rPushGiftGroupRefObj == null || _m_rPushGiftGroupRefObj.group_id != _m_lPushGiftGroupId)
                    _m_rPushGiftGroupRefObj = GRefdataCoreMgr.instance.pushGiftGroupRefCore.getRef(_m_lPushGiftGroupId);

                return _m_rPushGiftGroupRefObj;
            }
        }

        public long lastTriggerTimeMs { get { return _m_lLastTriggerTimeMs; } }//上次触发时间毫秒
        /// <summary>
        /// 触发cd剩余时间
        /// </summary>
        public long triggerCDRemainMs
        {
            get
            {
                return _m_lLastTriggerTimeMs + (pushGiftGroupRefObj?.next_trigger_need_seconds ?? 0) * 1000 - FpsAndPingMgr.instance.serverTimeTag;
            }
        }
        
        /// <summary>
        /// 当前激活的推送礼包信息
        /// </summary>
        public PushGiftPackInfo curPushGiftPackInfo { get { return _m_curPushGiftPackInfo; } }

        /// <summary>
        /// 是否正在请求触发礼包
        /// </summary>
        public bool isReqingTriggerGiftPack { get { return _m_bIsReqingTriggerGiftPack; } }
        
        #endregion
        
        #region 设置数据方法

        public void setLastTriggerTimeMs(long _timeMs)
        {
            _m_lLastTriggerTimeMs = _timeMs;
        }
        
        /// <summary>
        /// 设置当前激活的推送礼包信息
        /// </summary>
        /// <param name="_pushGiftPackInfo"></param>
        public void setCurPushGiftPackInfo(PushGiftPackInfo _pushGiftPackInfo)
        {
            // 相同则不处理
            if(_m_curPushGiftPackInfo == _pushGiftPackInfo)
                return;
            
            _m_curPushGiftPackInfo = _pushGiftPackInfo;
            _m_curPushGiftPackInfo?.setPushGiftGroupInfo(this);
        }
        
        /// <summary>
        /// 设置是否正在请求触发礼包
        /// </summary>
        /// <param name="_isReqing"></param>
        private void _setIsReqingTriggerGiftPack(bool _isReqing)
        {
            if(_m_bIsReqingTriggerGiftPack == _isReqing)
                return;
            
            _m_bIsReqingTriggerGiftPack = _isReqing;

            // 如果请求结束，执行回调
            if (!_m_bIsReqingTriggerGiftPack)
            {
                Action action = _m_aOnAfterReqTriggerGiftPack;
                _m_aOnAfterReqTriggerGiftPack = null;
                action?.Invoke();
            }
        }
        
        /// <summary>
        /// 注册请求触发礼包后回调
        /// </summary>
        /// <param name="_action"></param>
        public void regAfterReqTriggerGiftPack(Action _action)
        {
            // 若不在请求中，直接执行回调
            if(!_m_bIsReqingTriggerGiftPack)
            {
                _action?.Invoke();
                return;
            }
            
            _m_aOnAfterReqTriggerGiftPack += _action;
        }
        
        #endregion
        
        /// <summary>
        /// 尝试触发推送礼包
        /// </summary>
        /// <param name="_isAfterBuyAutoTrigger">是否购买完成后自动触发下一个</param>
        /// <param name="_tryTriggerDone">尝试触发操作完成, 不管是否真的触发出推送礼包都调用</param>
        /// <returns></returns>
        public void tryTriggerPushGiftPack(bool _isAfterBuyAutoTrigger, Action _tryTriggerDone = null)
        {
            if (_m_bIsReqingTriggerGiftPack)//若正在请求中，则不再触发
            {
                regAfterReqTriggerGiftPack(_tryTriggerDone);
                return;
            }
            
            // 若不是购买完成后的自动触发 且 还在触发cd内, 不触发新的推送礼包
            if (!_isAfterBuyAutoTrigger && triggerCDRemainMs > 0)
            {
                _tryTriggerDone?.Invoke();
                return;
            }
            
            // 若当前还有有效的推送礼包，则不触发新的
            if (_m_curPushGiftPackInfo != null && _m_curPushGiftPackInfo.isValid)
            {
                _tryTriggerDone?.Invoke();
                return;
            }

            // 若触发条件不满足，则不触发新的推送礼包
            if (pushGiftGroupRefObj == null || !pushGiftGroupRefObj.triggerConditionIsEnable(null))
            {
                _tryTriggerDone?.Invoke();
                return;
            }
            
            _setIsReqingTriggerGiftPack(true);
            regAfterReqTriggerGiftPack(_tryTriggerDone);
            NPPlayer.instance.pushGiftComp.reqTriggerPushGiftGroup(_m_lPushGiftGroupId, (_isSucc) =>
            {
                _setIsReqingTriggerGiftPack(false);
            });
            
            return;
        }
    }
}