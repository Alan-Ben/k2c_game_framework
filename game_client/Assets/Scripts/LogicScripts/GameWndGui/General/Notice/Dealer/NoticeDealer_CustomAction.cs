
using System;

namespace GOE
{
    /// <summary>
    /// 自定义显示Notice
    /// </summary>
    public class NoticeDealer_CustomAction : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Action<Action> _m_showAction;
        private ENoticeType[] _m_noticeTypeArray;
        private Action _m_dealDone;
        private string _m_sNoticeTag;
        private string _m_sNodeTag;
        private bool _m_bCanCurShow;
        private bool _m_bCanPlayPriority;
        private bool _m_bIsPriorityDealer;
        private bool _m_bNeedTransBk;
        private bool _m_bIsNoticeFullScreen;
        private bool _m_bIsOnlyUINode;

        public NoticeDealer_CustomAction(Action<Action> _showAction, ENoticeType[] _noticeTyp, Action _dealDone = null, string _noticeTag = default, string _nodeTag = default
            , bool _canCurShow = true, bool _canPlayPriority = false, bool _isPriorityDealer = false,
            bool _needTransBk = false, bool _isNoticeFullScreen = false, bool _isOnlyUINode = true)
        {
            _m_showAction = _showAction;
            _m_noticeTypeArray = _noticeTyp;
            _m_dealDone = _dealDone;
            _m_sNoticeTag = _noticeTag;
            _m_sNodeTag = _nodeTag;
            _m_bCanCurShow = _canCurShow;
            _m_bCanPlayPriority = _canPlayPriority;
            _m_bIsPriorityDealer = _isPriorityDealer;
            _m_bNeedTransBk = _needTransBk;
            _m_bIsNoticeFullScreen = _isNoticeFullScreen;
            _m_bIsOnlyUINode = _isOnlyUINode;
        }
        
        public override ENoticeType[] noticeType { get { return _m_noticeTypeArray; } }
        public override string noticeTag { get { return _m_sNoticeTag; } }
        public override string nodeTag { get { return _m_sNodeTag; } }
        public override bool canCurShow { get { return _m_bCanCurShow; } }
        public override bool canPlayPriority { get { return _m_bCanPlayPriority; } }
        public override bool isPriorityDealer { get { return _m_bIsPriorityDealer; } }
        public override bool needTransBk { get { return _m_bNeedTransBk; } }
        public override bool isNoticeFullScreen { get { return _m_bIsNoticeFullScreen; } }
        public override bool isOnlyUINode { get { return _m_bIsOnlyUINode; } }

        public override void dealShowNotice()
        {
            if (_m_showAction == null)
            {
                setDealerDone();
            }
            else
            {
                _m_showAction(setDealerDone);
            }
        }

        public override void dealHideNotice()
        {
        }

        protected override void _onDealerDone()
        {
            _m_dealDone?.Invoke();
        }
    }
}
