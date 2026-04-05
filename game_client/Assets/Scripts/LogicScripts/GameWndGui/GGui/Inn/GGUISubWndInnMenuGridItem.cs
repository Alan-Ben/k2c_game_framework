using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnMenuGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoInnMenuGridItem>
    {
        private int _m_index;
        private InnDishInfo _m_dishInfo;
        private NPGGuiWndTexture _m_dishIconWnd;
        private NPGGuiWndTexture _m_smallTipIconWnd;
        private GGUISubWndInnMenuGrid _m_parentGrid;
        
        
        public GGUISubWndInnMenuGridItem(GGUIMonoInnMenuGridItem _wnd, GGUISubWndInnMenuGrid _parentGrid)
            : base(_wnd)
        {
            _m_parentGrid = _parentGrid;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_dishIconWnd?.showWnd();
            _m_smallTipIconWnd?.showWnd();
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX, _onSimulateClickDishItem);
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_dishIconWnd?.hideWnd();
            _m_smallTipIconWnd?.hideWnd();
            
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX, _onSimulateClickDishItem);
        }
        protected override void _onReset()
        {
            _m_dishIconWnd?.discardTexture();
            _m_smallTipIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;
            _m_smallTipIconWnd?.discard();
            _m_smallTipIconWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);
            if (wnd.imgUnlockSmallTipIcon != null)
                _m_smallTipIconWnd = new NPGGuiWndTexture(wnd.imgUnlockSmallTipIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _resetGridItem()
        {
        }


        public void refreshWnd(InnDishInfo _dishInfo, int _index)
        {
            _m_dishInfo = _dishInfo;
            _m_index = _index;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_dishInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDishName, _m_dishInfo.nameTranslated);
            _m_dishIconWnd?.setTexture(_m_dishInfo.refObj.icon);
            _m_smallTipIconWnd?.setTexture(_m_dishInfo.refObj.unlock_small_icon);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockSmallTip, _m_dishInfo.unlockSmallTipTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDishNum, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishNum_num, _m_dishInfo.num));
            wnd.setShowState(_m_dishInfo.uiState);
        }
        
        
        private void _onBtnClick(GameObject _)
        {
            if (_m_dishInfo == null)
                return;

            if (!_m_dishInfo.isUnlock)
            {
                GGUIWndInnDishUnlock.instance.refreshWnd(_m_dishInfo, _m_parentGrid?.getDishList());
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnDishUnlock.instance, GGUIWndInnDishUnlock.instance.showWnd, UINodeTagConst.C_INN_DISH_UNLOCK);
                return;
            }
            
            GGUIWndInnDishLevelUp.instance.refreshWnd(_m_dishInfo, _m_parentGrid?.getDishList());
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnDishLevelUp.instance, GGUIWndInnDishLevelUp.instance.showWnd, UINodeTagConst.C_INN_DISH_LEVEL_UP);
        }
        
        private void _onSimulateClickDishItem(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0) 
                return;

            long targetIndex = (long)_objects[0];
            if (targetIndex == _m_index)
                _onBtnClick(null);
        }
    }
}