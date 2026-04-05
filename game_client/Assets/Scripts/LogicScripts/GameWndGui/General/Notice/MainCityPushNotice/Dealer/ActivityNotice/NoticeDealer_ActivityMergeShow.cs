namespace GOE
{
    /// <summary>
    /// 活动合并展示主城推送弹窗
    /// </summary>
    public class NoticeDealer_ActivityMergeShow : _AMainCityCanJumpPushNotice
    {
        private bool _m_bIsLoadWndInNotice = false;
        
        public NoticeDealer_ActivityMergeShow(EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
        }

        protected override bool _isEnable
        {
            get
            {
                bool needShow = false;
                foreach (var pushNoticeActivityMergeRefObj in GRefdataCoreMgr.instance.pushNoticeActivityMergeRefCore.refList)
                {
                    if(pushNoticeActivityMergeRefObj == null)
                        continue;

                    _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(pushNoticeActivityMergeRefObj.activity_id);
                    if(activityInfo != null && activityInfo.isEnable)
                    {
                        needShow = true;
                        break;
                    }
                }
                
                return needShow;
            }
        }

        protected override bool _canCurShow { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.ACTIVITY_MERGE_SHOW_PUSH; } }
        public override string nodeTag { get { return UINodeTagConst.C_ACTIVITY_MERGE_SHOW_PUSH_NOTICE; } }

        public override bool needTransBk { get { return true; } }

        public override void dealShowNotice()
        {
            if (!_m_bIsLoadWndInNotice)
            {
                _m_bIsLoadWndInNotice = true;
                GGUIWndActivityMergeShowPushNotice.instance.load();
                GGUIWndActivityMergeShowPushNotice.instance.dealCloseAction += setDealerDone;
            }
            
            GGUIWndActivityMergeShowPushNotice.instance.regLoadDoneDelegate(() =>
            {
                GGUIWndActivityMergeShowPushNotice.instance.showWnd();
                
                if(GGUIWndActivityMergeShowPushNotice.instance.wnd != null)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndActivityMergeShowPushNotice.instance.wnd);
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndActivityMergeShowPushNotice.instance.hideWnd();
        }

        protected override void __onDealerDone()
        {
            if (_m_bIsLoadWndInNotice)
            {
                GGUIWndActivityMergeShowPushNotice.instance.dealCloseAction -= setDealerDone;
                GGUIWndActivityMergeShowPushNotice.instance.discard();
                _m_bIsLoadWndInNotice = false;
            }
        }
        
        // 临时加上, 防止不能IF修改
        public override void showNotice()
        {
            base.showNotice();
        }

        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
            base._onGotoOtherMainViewNode();
        }
    }
}