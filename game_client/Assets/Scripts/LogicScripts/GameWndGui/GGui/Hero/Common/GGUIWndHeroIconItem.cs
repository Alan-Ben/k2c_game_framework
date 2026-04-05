using ALPackage;
using NPEnum;

namespace GOE
{
    public class GGUIWndHeroIconItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoHeroIconItem, GGUIWndHeroIconItem>
    {
        private _IHeroCardShow _m_iHeroCardShow;//大臣显示数据
        private long _m_power;
        
        private NPGGuiWndTexture _m_texHeroIcon;//情人图标
        private GGuiWndSprite _m_imgHeroIconBg;//情人图标背景
        
        public GGUIWndHeroIconItem(GGUIMonoHeroIconItem _mono) : base(_mono)
        {
            initWnd();
        }
        
        public _IHeroCardShow heroCardShow { get => _m_iHeroCardShow; }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            if (wnd.texHeroIcon != null)
                _m_texHeroIcon = new NPGGuiWndTexture(wnd.texHeroIcon);

            if (wnd.imgHeroIconBg != null)
                _m_imgHeroIconBg = new GGuiWndSprite(wnd.imgHeroIconBg);
        }
        
        protected override void _onDiscardEx()
        {
            _m_texHeroIcon?.discard();
            _m_texHeroIcon = null;
            
            _m_imgHeroIconBg?.discard();
            _m_imgHeroIconBg = null;
        }
        
        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_texHeroIcon?.hideWnd();
            _m_imgHeroIconBg?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_texHeroIcon?.discardTexture();
            _m_imgHeroIconBg?.discardTexture();
        }

        public void setData(_IHeroCardShow _cardShow)
        {
            if(null == _cardShow)
                return;
            
            _m_iHeroCardShow = _cardShow;
            _m_power = _cardShow.power;

            _refreshWnd();
        }

        public void setPower(long _power)
        {
            _m_power = _power;

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_iHeroCardShow || _m_iHeroCardShow.heroRefObj == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_iHeroCardShow.heroRefObj.transName);

            if (_m_texHeroIcon != null)
            {
                _m_texHeroIcon.showWnd();
                _m_texHeroIcon.setTexture(_m_iHeroCardShow.getIcon());
            }

            if (_m_imgHeroIconBg != null)
            {
                _m_imgHeroIconBg.showWnd();
                _m_imgHeroIconBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_iHeroCardShow.id)?.hero_head_bg);
            }

            if(string.IsNullOrEmpty(wnd.txtHeroPowerKey))
                ALUGUICommon.setLabelTxt(wnd.txtHeroPower, _m_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            else
                ALUGUICommon.setLabelTxt(wnd.txtHeroPower, TextTranslate.instance.getLanguage(wnd.txtHeroPowerKey, _m_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }
    }
}