using System;

namespace GOE
{
    /// <summary>
    /// 推送礼包触发弹窗
    /// </summary>
    public class NoticeDealer_PushGiftPackTriggerPop : _AMainCityCanJumpPushNotice
    {
        // private Func<bool> _m_fIsEnableFunc;
        private PushGiftPackInfo _m_iPushGiftPackInfo;
        private bool _m_bNeedShowBk;
        
        private bool _m_bIsDealing = false;

        public NoticeDealer_PushGiftPackTriggerPop(PushGiftPackInfo _pushGiftPackInfo, bool _needShowBk, EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
            // _m_fIsEnableFunc = _isEnableFunc;
            _m_iPushGiftPackInfo = _pushGiftPackInfo;
            _m_bNeedShowBk = _needShowBk;

            _m_bIsDealing = false;
        }

        public event Action onDealDone;
        
        public bool isDealing { get { return _m_bIsDealing; } }
        
        protected override bool _isEnable { get { return true; } }
        protected override bool _canCurShow
        {
            get
            {
                if (_m_iPushGiftPackInfo != null && _m_iPushGiftPackInfo.pushGiftPackRefObj != null)
                {
                    // 若当前节点不在可弹出显示节点列表中，则不可显示
                    if (!_m_iPushGiftPackInfo.pushGiftPackRefObj.isCanPopShowNode(QueueMgr.instance._lastNode.nodeTag))
                        return false;
                }
                return true;
            }
        }

        public override ENoticeType[] noticeType { get { return NPNoticeType.g_AllTypeArr; } }
        protected override string _noticeTag { get { return NoticeTagConst.PUSH_GIFT_PACK_TRIGGER_POP; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        public override bool noticeCanDoESC { get { return false; } }
        
        public PushGiftPackInfo pushGiftPackInfo { get { return _m_iPushGiftPackInfo; } }

        public override void showNotice()
        {
            base.showNotice();
            
            _m_bIsDealing = true;
        }

        public override void dealShowNotice()
        {
            // 推送礼包信息无效或已读则直接完成处理
            if (_m_iPushGiftPackInfo == null || !_m_iPushGiftPackInfo.isValid || _m_iPushGiftPackInfo.hasRead
                // || (_m_fIsEnableFunc != null && !_m_fIsEnableFunc())
                )
            {
                setDealerDone();
                return;
            }
            
            QueueMgr.instance.AddNode(new GNodePushGiftPackPop(_m_iPushGiftPackInfo, _m_bNeedShowBk, setDealerDone));
        }

        public override void dealHideNotice()
        {
        }
                
        protected override void __onDealerDone()
        {
            onDealDone?.Invoke();
            onDealDone = null;
            
            _m_bIsDealing = false;
        }
        
        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
        }
    }
}