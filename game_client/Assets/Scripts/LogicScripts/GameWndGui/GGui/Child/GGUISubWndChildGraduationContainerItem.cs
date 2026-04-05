using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndChildGraduationContainerItem : _ATALBasicUISubWnd<GGUIMonoChildGraduationContainerItem>
    {
        private GGUISubWndChildInfo _m_childInfoWnd;
        private NPGGUIWndCommonItem _m_presentItemWnd;
        
        private _IChildInfo _m_childInfo;
        private _IItem _m_presentItem;
        
        
        public GGUISubWndChildGraduationContainerItem(GGUIMonoChildGraduationContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();
            _m_presentItemWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
            _m_presentItemWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
            _m_presentItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_presentItemWnd?.discard();
            _m_childInfoWnd = null;
            _m_presentItemWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.monoPresentItem != null)
                _m_presentItemWnd = new NPGGUIWndCommonItem(wnd.monoPresentItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }


        public void refreshWnd(_IChildInfo _childInfo, _IItem _presentItem)
        {
            _m_childInfo = _childInfo;
            _m_presentItem = _presentItem;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfoWnd == null)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_childInfo);
            _m_presentItemWnd?.showWnd(_m_presentItem);
        }


        private void _onBtnDetailClick(GameObject _obj)
        {
            GGUIWndChildGraduatedDetail.instance.refreshWnd(_m_childInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildGraduatedDetail.instance, GGUIWndChildGraduatedDetail.instance.showWnd, EUIQueueStageType.MAIN, string.Empty, true, false);
        }
    }
}