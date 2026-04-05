using System;
using ALPackage;
using Random = UnityEngine.Random;

namespace GOE
{
    public class GGUIWndBuildingCoinEarningTipFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoBuildingCoinEarningTip, GGUIWndBuildingCoinEarningTip>
    {
        private readonly GResPathIndex _m_resIndex;
        private long _m_num;
        
        
        public GGUIWndBuildingCoinEarningTipFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1113);
        }
        public GGUIWndBuildingCoinEarningTipFollowItemController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndBuildingCoinEarningTip _createItemWnd(GGUIMonoBuildingCoinEarningTip _wndMono)
        {
            GGUIWndBuildingCoinEarningTip wnd = new GGUIWndBuildingCoinEarningTip(_wndMono, discard);
            wnd.refreshWnd(_m_num);
            wnd.showWnd();
            return wnd;
        }

        public void setCoinNum(long _num)
        {
            _m_num = _num;
            wnd?.refreshWnd(_m_num);
        }
    }
    public class GGUIWndBuildingCoinEarningTip : _ATALGGUIWndCommonFollowItem<GGUIMonoBuildingCoinEarningTip>
    {
        private long _m_num;
        private Action _m_deleteFunc;
        
        
        public GGUIWndBuildingCoinEarningTip(GGUIMonoBuildingCoinEarningTip _wnd, Action _deleteFunc) : base(_wnd)
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

            ALUGUICommon.setLabelTxt(wnd.txtCoinNum, TextTranslate.instance.getLanguage(TransKeyConst.building_earningTip_num, _m_num.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
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