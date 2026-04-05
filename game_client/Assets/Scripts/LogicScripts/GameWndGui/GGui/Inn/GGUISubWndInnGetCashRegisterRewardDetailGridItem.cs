using ALPackage;
using Common.InnObj;

namespace GOE
{
    public class GGUISubWndInnGetCashRegisterRewardDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoInnGetCashRegisterRewardDetailGridItem>
    {
        private Inn_DishSettleInfo _m_serverInfo;
        private NPGGuiWndTexture _m_dishIcon;
        
        
        public GGUISubWndInnGetCashRegisterRewardDetailGridItem(GGUIMonoInnGetCashRegisterRewardDetailGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {    
            _m_dishIcon?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_dishIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_dishIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_dishIcon?.discard();
            _m_dishIcon = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgDishIcon != null)
                _m_dishIcon = new NPGGuiWndTexture(wnd.imgDishIcon);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(Inn_DishSettleInfo _serverInfo)
        {
            _m_serverInfo = _serverInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_serverInfo == null)
                return;

            InnDishInfo dishInfo = NPPlayer.instance.innComp.getDishInfoById(_m_serverInfo.getDishId());
            if (dishInfo == null)
            {
                ALLog.Error($"GGUISubWndInnGetCashRegisterRewardDetailGridItem.refreshWnd: dishInfo is null, dishId: {_m_serverInfo.getDishId()}");
                return;
            }
            
            _m_dishIcon?.setTexture(dishInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtDishName, dishInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtGuestNum, _m_serverInfo.getReceiveNum());
            ALUGUICommon.setLabelTxt(wnd.txtFinesseAdd, _m_serverInfo.getAddFinesse());
        }
    }
}