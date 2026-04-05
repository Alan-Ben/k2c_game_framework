using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子随机邀约过程表现
    /// </summary>
    public class NoticeDealer_ConsortRandCallProcess : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Func<bool> _m_fIsEnableFunc;
        private Common.ConsortObj.Consort_CallRes _m_CallRes;//邀约结果数据
        private long _m_lDialogId;//显示对话id
        private Func<Vector3> _m_fGetConsortHeadStartPosFunc;
        private NPGGoIndex _m_BgGoIndex;
        private Action _m_aOnDealDone;

        public NoticeDealer_ConsortRandCallProcess(Func<bool> _isEnableFunc, Common.ConsortObj.Consort_CallRes _callRes, long _dialogId, Func<Vector3> _getConsortHeadStartPosFunc, NPGGoIndex _bgGoIndex, Action _onDealDone = null)
        {
            _m_fIsEnableFunc = _isEnableFunc;
            _m_CallRes = _callRes;
            _m_lDialogId = _dialogId;
            _m_fGetConsortHeadStartPosFunc = _getConsortHeadStartPosFunc;
            _m_BgGoIndex = _bgGoIndex;
            _m_aOnDealDone = _onDealDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }
        public override bool noticeCanDoESC { get { return false; } }

        public override string noticeTag { get; }
        public override string nodeTag { get; }
        
        public override void dealShowNotice()
        {
            if (_m_CallRes == null || (_m_fIsEnableFunc != null && !_m_fIsEnableFunc()))
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortRandomInviteProcess.instance, () =>
            {
                GGUIWndConsortRandomInviteProcess.instance.showWnd();
                GGUIWndConsortRandomInviteProcess.instance.setInfo(_m_CallRes, _m_lDialogId, _m_fGetConsortHeadStartPosFunc, _m_BgGoIndex,
                    () =>
                    {
                        setDealerDone();
                    });
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortRandomInviteProcess.instance.hideWnd();
        }
        
                
        protected override void _onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
            _m_aOnDealDone = null;
        }

        public override void onCannotEscBack()
        {
            GGUIWndConsortRandomInviteProcess.instance.doEsc();
        }
    }
}