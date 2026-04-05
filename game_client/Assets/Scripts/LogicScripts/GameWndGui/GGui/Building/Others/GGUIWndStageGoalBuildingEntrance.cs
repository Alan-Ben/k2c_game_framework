
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndStageGoalBuildingEntranceFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoStageGoalBuildingEntrance, GGUIWndStageGoalBuildingEntrance>
    {
        [NotNull] private readonly GResPathIndex _m_resIndex;
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        
        
        public GGUIWndStageGoalBuildingEntranceFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1116);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        protected override GGUIWndStageGoalBuildingEntrance _createItemWnd(GGUIMonoStageGoalBuildingEntrance _wndMono)
        {
            GGUIWndStageGoalBuildingEntrance wnd = new GGUIWndStageGoalBuildingEntrance(_wndMono);
            wnd.refreshWnd(_m_bigStepRefObj);
            wnd.showWnd();
            return wnd;
        }


        public void setStepRef(StageGoalBigStepRefObj _bigStepRef)
        {
            _m_bigStepRefObj = _bigStepRef;
            wnd?.refreshWnd(_m_bigStepRefObj);
        }
    }
    public class GGUIWndStageGoalBuildingEntrance : _ATALGGUIWndCommonFollowItem<GGUIMonoStageGoalBuildingEntrance>
    {
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        
        
        public GGUIWndStageGoalBuildingEntrance(GGUIMonoStageGoalBuildingEntrance _wnd) 
            : base(_wnd)
        {
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


        public void refreshWnd(StageGoalBigStepRefObj _bigStepRef)
        {
            _m_bigStepRefObj = _bigStepRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_bigStepRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_bigStepRefObj.getTitle);
        }
    }
}