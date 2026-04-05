namespace GOE
{
    public abstract class _AActivityMainCityPushNotice : _AMainCityCanJumpPushNotice
    {
        public _AActivityMainCityPushNotice(EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
        }
        
        protected override bool _isEnable
        {
            get
            {
                return AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.ACTIVITY_MAIN_CITY_PUSH_NOTICE_TODAY_IGNORE);
            }
        }

        protected override void __onDealerDone()
        {
        }
    }
}