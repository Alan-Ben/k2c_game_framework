namespace GOE
{
    /// <summary>
    /// 主城推送弹窗触发类型
    /// </summary>
    public enum EMainCityPushNoticeTriggerType
    {
        OTHER,//其他
        LOGIN,//登陆
    }
    
    /// <summary>
    /// 主城推送弹窗基类, 所有可能在主城弹出的推送弹窗都需要继承该类
    /// </summary>
    public abstract class _AMainCityPushNotice : NPUINoticeMgr._ANPUINoticeDealer
    {
        protected EMainCityPushNoticeTriggerType _m_ePushNoticeTriggerType;//推送弹窗触发类型
        private MainCityPushNoticeRefObj _m_rPushNoticeRefObj;//主城推送弹窗引用对象
        
        public _AMainCityPushNotice(EMainCityPushNoticeTriggerType _pushNoticeTriggerType)
        {
            _m_ePushNoticeTriggerType = _pushNoticeTriggerType;
        }

        public MainCityPushNoticeRefObj mainCityPushNoticeRefObj
        {
            get
            {
                if (_m_rPushNoticeRefObj == null || _m_rPushNoticeRefObj.notice_tag != noticeTag)
                {
                    _m_rPushNoticeRefObj = GRefdataCoreMgr.instance.getMainCityPushNoticeRefObj(noticeTag);
                }
                if(_m_rPushNoticeRefObj == null)
                {
                    Debug.LogError_EditorOnly($"主城推送弹窗引用对象获取失败, main_city_push_notice表中找不到 notice_tag = {noticeTag} 的配表数据, 该主城推送弹窗将会最后展示");
                }

                return _m_rPushNoticeRefObj;
            }
        }
        
        public override bool isPriorityDealer { get { return false; } }//主城推送类弹窗默认不作为优先处理对象
        public override bool canPlayPriority { get { return true; } }//主城推送类弹窗默认允许弹出高优先级Notice弹窗
        public override ENoticeType[] noticeType { get { return null; } }//主城推送弹窗, 默认只在主城展示(若还想在其他地方展示, 需子类重写该属性)
        public sealed override ENoticeType noticeTypeSingle { get { return ENoticeType.BUILDING; } } //主城推送弹窗, 在主城需要展示

        public sealed override string noticeTag { get { return _noticeTag; } }

        /// <summary>
        /// 是否有效
        /// </summary>
        public sealed override bool isEnable
        {
            get
            {
                return _isEnable;
            }
        }

        public sealed override bool canCurShow
        {
            get
            {
                MainCityPushNoticeRefObj refObj = mainCityPushNoticeRefObj;
                if (refObj != null)
                {
                    switch (_m_ePushNoticeTriggerType)
                    {
                        case EMainCityPushNoticeTriggerType.LOGIN:
                            // 若登录触发条件为空，或不满足条件，暂时不可展示
                            if (refObj.login_show_condition == null || !refObj.login_show_condition.hasCondition || !refObj.login_show_condition.IsEnable(null))
                                return false;
                            break;
                        case EMainCityPushNoticeTriggerType.OTHER:
                            // 若其他触发条件为空，或不满足条件，暂时不可展示
                            if (refObj.other_show_condition == null || !refObj.other_show_condition.hasCondition || !refObj.other_show_condition.IsEnable(null))
                                return false;
                            break;
                    }
                }

                return _canCurShow;
            }
        }

        protected abstract bool _isEnable { get; }
        protected abstract bool _canCurShow { get; }
        protected abstract string _noticeTag { get; }
    }
}