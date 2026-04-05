using System;
using ALPackage;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 建筑收益暴击倍数提示tip的跟随控制
    /// </summary>
    public class GGUIWndBuildingEarningMultipleTipFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoBuildingEarningMultipleTip, GGUIWndBuildingEarningMultipleTip>
    {
        private readonly GResPathIndex _m_resIndex;
        private long _m_num;
        
        public GGUIWndBuildingEarningMultipleTipFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1113);
        }
        public GGUIWndBuildingEarningMultipleTipFollowItemController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndBuildingEarningMultipleTip _createItemWnd(GGUIMonoBuildingEarningMultipleTip _wndMono)
        {
            GGUIWndBuildingEarningMultipleTip wnd = new GGUIWndBuildingEarningMultipleTip(_wndMono, discard);
            wnd.refreshWnd(_m_num);
            wnd.showWnd();
            return wnd;
        }

        public void setMultipleNum(long _num)
        {
            _m_num = _num;
            wnd?.refreshWnd(_m_num);
        }
    }

    /// <summary>
    /// 建筑收益暴击倍数提示tip
    /// </summary>
    public class GGUIWndBuildingEarningMultipleTip : _ATALGGUIWndCommonFollowItem<GGUIMonoBuildingEarningMultipleTip>
    {
        private long _m_num;
        private Action _m_deleteFunc;
        
        
        public GGUIWndBuildingEarningMultipleTip(GGUIMonoBuildingEarningMultipleTip _wnd, Action _deleteFunc) : base(_wnd)
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
        
        
        public void refreshWnd(long _num)
        {
            _m_num = _num;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtMultipleNum, TextTranslate.instance.getLanguage(TransKeyConst.building_clickEarningMutipleValueTip_num, _m_num));
            if (wnd.showAnimation != null)
                wnd.showAnimation.Play(wnd.showAnimationName);
            ALCommonActionMonoTask.addMonoTask(_delete, wnd.deleteDelay);
            if (wnd.offsetParent != null)
                ALUGUICommon.setUIPos(wnd.offsetParent, Random.insideUnitCircle * wnd.randomRadius);
        }

        private void _delete()
        {
            _m_deleteFunc?.Invoke();
        }
    }
}