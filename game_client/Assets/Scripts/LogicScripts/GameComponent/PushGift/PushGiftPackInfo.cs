using System;

namespace GOE
{
    /// <summary>
    /// 推送礼包信息
    /// </summary>
    public class PushGiftPackInfo : _ICommonCountDownInfo
    {
        private long _m_lPushGiftPackId;//推送礼包id
        private PushGiftPackRefObj _m_rPushGiftPackRefObj;//推送礼包配表数据
        private long _m_lActivateTimeMs;//激活时间(毫秒)
        private bool _m_bHasRead;//是否已读(显示过礼包)
        private bool _m_IsAutoTriggered;//是否是自动触发的
        private PushGiftGroupInfo _m_PushGiftGroupInfo;//所属礼包组信息

        private NoticeDealer_PushGiftPackTriggerPop _m_showNotice;
        
        public PushGiftPackInfo(long _pushGiftPackId)
        {
            _m_lPushGiftPackId = _pushGiftPackId;
            _m_lActivateTimeMs = 0;
            _m_bHasRead = false;
            _m_IsAutoTriggered = false;
            _m_PushGiftGroupInfo = null;
        }

        #region 属性

        public long pushGiftPackId { get { return _m_lPushGiftPackId; } }
        
        public PushGiftPackRefObj pushGiftPackRefObj
        {
            get
            {
                if(_m_rPushGiftPackRefObj == null || _m_rPushGiftPackRefObj.gift_pack_id != _m_lPushGiftPackId)
                    _m_rPushGiftPackRefObj = GRefdataCoreMgr.instance.pushGiftPackRefCore.getRef(_m_lPushGiftPackId);

                return _m_rPushGiftPackRefObj;
            }
        }

        /// <summary>
        /// 对应现金礼包id
        /// </summary>
        public long giftPackId { get { return pushGiftPackRefObj?.gift_pack_id ?? 0; } }

        public PushGiftGroupInfo pushGiftGroupInfo { get { return _m_PushGiftGroupInfo; } }
        
        /// <summary>
        /// 礼包激活时间(毫秒)
        /// </summary>
        public long activateTimeMs { get { return _m_lActivateTimeMs; } }
        
        /// <summary>
        /// 剩余有效时间(毫秒)
        /// </summary>
        public long remainTimeMs
        {
            get
            {
                return _m_lActivateTimeMs + (pushGiftPackRefObj?.continue_time ?? 0) * 1000 - FpsAndPingMgr.instance.serverTimeTag;
            }
        }

        /// <summary>
        /// 剩余可买次数
        /// </summary>
        public long leftCanBuyCount { get { return NPPlayer.instance.giftPackComp.getGiftPackLeftCount(giftPackId); } }
        /// <summary>
        /// 已购买次数
        /// </summary>
        public long hadBuyCount { get { return NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(giftPackId); } }

        public bool isValid 
        {
            get
            {
                // 当推送礼包还在有效期内，且礼包还有剩余购买次数时，视为有效
                return remainTimeMs > 0 && leftCanBuyCount > 0;
            }
        }

        /// <summary>
        /// 是否已读(显示过礼包)
        /// </summary>
        public bool hasRead { get { return _m_bHasRead;} }

        public bool isAutoTriggered { get { return _m_IsAutoTriggered; } set { _m_IsAutoTriggered = value; } }

        public NoticeDealer_PushGiftPackTriggerPop showNotice { get { return _m_showNotice; } }

        #endregion

        #region 设置数据方法

        /// <summary>
        /// 设置所属礼包组信息
        /// </summary>
        /// <param name="_pushGiftGroupInfo"></param>
        public void setPushGiftGroupInfo(PushGiftGroupInfo _pushGiftGroupInfo)
        {
            _m_PushGiftGroupInfo = _pushGiftGroupInfo;
        }

        /// <summary>
        /// 设置激活时间(毫秒)
        /// </summary>
        /// <param name="_timeMs"></param>
        public void setActivateTimeMs(long _timeMs)
        {
            _m_lActivateTimeMs = _timeMs;
        }
        
        /// <summary>
        /// 设置礼包已读
        /// </summary>
        public void setRead(bool _isRead)
        {
            _m_bHasRead = _isRead;
        }

        #endregion

        #region 触发出礼包展示

        /// <summary>
        /// 显示触发弹窗Notice
        /// </summary>
        public void showTriggerPopWndNotice(EMainCityPushNoticeTriggerType _pushNoticeTriggerType)
        {
            if (_m_showNotice != null || _m_bHasRead || !isValid)//若已经有Notice在展示 或 已经展示过 或 已经无效，则不再展示
                return;

            _m_showNotice = new NoticeDealer_PushGiftPackTriggerPop(this, true, _pushNoticeTriggerType);
            _m_showNotice.onDealDone += _onTriggerPopWndNoticeDealDone;
            NPUINoticeMgr.instance.addDealer(_m_showNotice);
        }

        private void _onTriggerPopWndNoticeDealDone()
        {
            _m_showNotice = null;
        }
        
        /// <summary>
        /// 移除触发弹窗Notice
        /// </summary>
        public void removeTriggerPopWndNotice(bool _removeFromNoticeMgr)
        {
            if (_removeFromNoticeMgr)
            {
                NPUINoticeMgr.instance.removeDealer((_noticeDealer) =>
                {
                    return _noticeDealer == _m_showNotice;
                });
            }

            _m_showNotice = null;
        }

        #endregion
        
        /// <summary>
        /// 尝试购买完成后自动触发下一个推送礼包
        /// </summary>
        /// <param name="_tryTriggerDone">尝试触发操作完成, 不管是否真的触发出推送礼包都调用</param>
        /// <returns></returns>
        public void tryAfterBuyAutoTriggerNextPushGiftPack(Action _tryTriggerDone = null)
        {
            Debug.LogError($"自动触发不需要客户端发起请求, 由服务端控制");
            // if (_m_PushGiftGroupInfo == null || pushGiftPackRefObj == null)
            // {
            //     _tryTriggerDone?.Invoke();
            //     return;
            // }
            //
            // // 还有剩余购买次数，则不自动触发下一个礼包, 自动触发不需要判断礼包组是否在触发cd内
            // if (leftCanBuyCount > 0)
            // {
            //     _tryTriggerDone?.Invoke();
            //     return;
            // }
            //
            // if(pushGiftPackRefObj.after_buy_auto_trigger_next)
            // {
            //     _m_PushGiftGroupInfo.tryTriggerPushGiftPack(true, _tryTriggerDone);
            // }
            // else
            // {
            //     _tryTriggerDone?.Invoke();
            // }
        }
    }
}