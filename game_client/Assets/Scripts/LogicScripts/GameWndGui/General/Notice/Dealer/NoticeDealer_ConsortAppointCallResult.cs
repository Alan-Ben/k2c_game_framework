using System;

namespace GOE
{
    /// <summary>
    /// 妃子指定邀约结果
    /// </summary>
    public class NoticeDealer_ConsortAppointCallResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private long _m_lConsortId;//指定邀约的妃子ID
        private long _m_lTravelId;//指定的出游方式
        private GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _m_RetMsg;
        private Action _m_aOnDealDone;//展示完成回调
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public NoticeDealer_ConsortAppointCallResult(long _consortId, long _travelId, GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _retMsg, Action _onDealDone = null)
        {
            _m_lConsortId = _consortId;
            _m_lTravelId = _travelId;
            _m_RetMsg = _retMsg;
            _m_aOnDealDone = _onDealDone;
        }
        
        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_RANDOM_INVITE_REWARD; } }

        public override void dealShowNotice()
        {
            if (_m_RetMsg == null)
            {
                setDealerDone();
                return;
            }
            
            // 先用随机邀约弹窗
            GUISceneMain.instance.showAddWnd(GGUIWndConsortRandomInviteReward.instance, () =>
            {
                GGUIWndConsortRandomInviteReward.instance.showWnd();
                GGUIWndConsortRandomInviteReward.instance.setData(new Common.ConsortObj.Consort_CallRes(_m_lConsortId, 0, _m_RetMsg.getAddCharmPoint(), false, _m_RetMsg.getChildId()));
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortRandomInviteReward.instance.hideWnd();
        }
        
                
        protected override void _onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
            _m_aOnDealDone = null;
        }
    }
}