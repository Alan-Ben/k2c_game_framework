using ALPackage;
using NPEnum;

namespace GOE
{
    public class GGUISubWndCommonNPCIconContainerItem : _ATALBasicUISubWnd<GGUIMonoCommonNPCIconContainerItem>
    {
        private NPNPCRefObj _m_npcRef;

        private NPGGuiWndTexture _m_iconWnd;
        
        public GGUISubWndCommonNPCIconContainerItem(GGUIMonoCommonNPCIconContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            refreshWnd();
        }

        public void refreshWnd(NPNPCRefObj _npcRef)
        {
            _m_npcRef = _npcRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_npcRef == null)
                return;

            switch (_m_npcRef.npcType)
            {
                case ENPNPCType.SELF:
                    _m_iconWnd?.setTexture(GCommon.getItemTexIcon(ENPItemType.ICON, NPPlayer.instance.playerInfo.getCurrentIconId()));
                    ALUGUICommon.setLabelTxt(wnd.txtName, NPPlayer.instance.playerInfo.PlayerName);
                    break;
                default:
                    _m_iconWnd?.setTexture(_m_npcRef.npcIcon);
                    ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_npcRef.npcName));
                    break;
            }
        }
    }
}