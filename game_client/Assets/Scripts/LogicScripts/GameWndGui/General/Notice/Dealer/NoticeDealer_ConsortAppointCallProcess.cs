using System;

namespace GOE
{
    public class NoticeDealer_ConsortAppointCallProcess : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Func<bool> _m_fIsEnableFunc;
        private GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _m_appointInfoRetInfo;
        private GGottenConsortInfo _m_rGottenConsortInfo;//出游妃子信息
        private ConsortTravelRefObj _m_rTravelRefObj;//出游配表数据
        private Action _m_aOnDealDone;//展示完成回调

        public NoticeDealer_ConsortAppointCallProcess(Func<bool> _isEnableFunc, GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _retInfo, GGottenConsortInfo _consortInfo, ConsortTravelRefObj _travelRefObj, Action _onDealDone = null)
        {
            _m_fIsEnableFunc = _isEnableFunc;
            _m_appointInfoRetInfo = _retInfo;
            _m_rGottenConsortInfo = _consortInfo;
            _m_rTravelRefObj = _travelRefObj;
            _m_aOnDealDone = _onDealDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }

        public override string noticeTag { get; }
        public override string nodeTag { get; }
        
        public override void dealShowNotice()
        {
            if (_m_appointInfoRetInfo == null || (_m_fIsEnableFunc != null && !_m_fIsEnableFunc()))
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortAppointCallProcess.instance, () =>
            {
                GGUIWndConsortAppointCallProcess.instance.showWnd();
                GGUIWndConsortAppointCallProcess.instance.setData(_m_appointInfoRetInfo, _m_rGottenConsortInfo, _m_rTravelRefObj,
                    () =>
                    {
                        setDealerDone();
                    });
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortAppointCallProcess.instance.hideWnd();
        }
        
                
        protected override void _onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
            _m_aOnDealDone = null;
        }
    }
}