
using System;

namespace GOE
{
    /// <summary>
    /// 空notice，用于过度执行不同的action
    /// </summary>
    public class NoticeDealer_EmptyNoticeOnlyAction : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Action _m_dealDone;
        private readonly float _m_delayTime;

        public NoticeDealer_EmptyNoticeOnlyAction(Action _dealDone, float _delayTime = 0)
        {
            _m_dealDone = _dealDone;
            _m_delayTime = _delayTime;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }


        public override void dealShowNotice()
        {
            if (_m_delayTime <= 0)
            {
                setDealerDone();
            }
            else
            {
                CommonTaskController.CommonActionAddMonoTask(setDealerDone, _m_delayTime);
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
