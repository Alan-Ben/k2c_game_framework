using System;
using ALPackage;

namespace GOE
{
    public class GGUIWndBuildingUnbuiltNonsenseFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoBuildingUnbuiltNonsense, GGUIWndBuildingUnbuiltNonsense>
    {
        private readonly GResPathIndex _m_resIndex;
        private string _m_nonsenseTranslated;
        
        
        public GGUIWndBuildingUnbuiltNonsenseFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1111);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndBuildingUnbuiltNonsense _createItemWnd(GGUIMonoBuildingUnbuiltNonsense _wndMono)
        {
            GGUIWndBuildingUnbuiltNonsense wnd = new GGUIWndBuildingUnbuiltNonsense(_wndMono, discard);
            wnd.refreshWnd(_m_nonsenseTranslated);
            wnd.showWnd();
            return wnd;
        }
        
        
        public void setNonsenseTranslated(string _nonsenseTranslated)
        {
            _m_nonsenseTranslated = _nonsenseTranslated;
            wnd?.refreshWnd(_m_nonsenseTranslated);
        }
    }
    public class GGUIWndBuildingUnbuiltNonsense : _ATALGGUIWndCommonFollowItem<GGUIMonoBuildingUnbuiltNonsense>
    {
        private string _m_nonsenseTranslated;
        private Action _m_deleteFunc;
        
        
        public GGUIWndBuildingUnbuiltNonsense(GGUIMonoBuildingUnbuiltNonsense _wnd, Action _deleteFunc) 
            : base(_wnd)
        {
            _m_deleteFunc = _deleteFunc;
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(string _nonsenseTranslated)
        {
            _m_nonsenseTranslated = _nonsenseTranslated;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNonsense, _m_nonsenseTranslated);
            if (wnd.showAnimation != null)
                wnd.showAnimation.Play(wnd.showAnimationName);
            ALCommonActionMonoTask.addMonoTask(_delete, wnd.deleteDelay);
        }
        
        
        private void _delete()
        {
            _m_deleteFunc?.Invoke();
        }
    }
}