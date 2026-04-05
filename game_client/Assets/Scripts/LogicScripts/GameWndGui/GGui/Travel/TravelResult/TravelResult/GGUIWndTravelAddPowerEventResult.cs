using ALPackage;

namespace GOE
{
    /// <summary>
    /// 增加实力事件结果窗口
    /// </summary>
    public class GGUIWndTravelAddPowerEventResult : _ATravelSpecificEventResultWnd<GGUIMonoTravelAddPowerEventResult, TravelAddPowerEventInfo, _ATravelSpecificEventResultInfo<TravelAddPowerEventInfo>>
    {
        private GGUIWndHeroIconItem _m_wHeroIconItem;
        
        public GGUIWndTravelAddPowerEventResult(_ATravelSpecificEventResultInfo<TravelAddPowerEventInfo> _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelAddPowerEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
        }
        
        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroIcon != null)
                _m_wHeroIconItem = new GGUIWndHeroIconItem(wnd.monoHeroIcon);
        }

        protected override void _onDiscardSub()
        {
            _m_wHeroIconItem?.discard();
            _m_wHeroIconItem = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wHeroIconItem?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wHeroIconItem?.resetWnd();
        }

        protected override void _onRefreshWnd()
        {
            if(_m_eventResultInfo == null || wnd == null)
                return;

            if (_m_eventResultInfo.specificEventInfo != null)
            {
                if (_m_wHeroIconItem != null)
                {
                    _m_wHeroIconItem.showWnd();
                    _m_wHeroIconItem.setData(_m_eventResultInfo.specificEventInfo.selectedHeroInfo);
                }
                
                string heroPowerKey = string.IsNullOrEmpty(wnd.txtAddPowerKey) ? TransKeyConst.common_add_num : wnd.txtAddPowerKey;
                ALUGUICommon.setLabelTxt(wnd.txtAddPower,
                    TextTranslate.instance.getLanguage(heroPowerKey,
                        _m_eventResultInfo.specificEventInfo.addPowerEventRefObj?.add_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            }
        }
    }
}