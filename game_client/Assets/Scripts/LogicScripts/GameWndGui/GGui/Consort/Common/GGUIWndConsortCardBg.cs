using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子卡牌背景子窗口
    /// </summary>
    public class GGUIWndConsortCardBg : _ATALBasicUISubWnd<GGUIMonoConsortCardBg>
    {
        private NPGGuiWndTexture _m_wBgImg;
        
        public GGUIWndConsortCardBg(GGUIMonoConsortCardBg _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgBg != null)
                _m_wBgImg = new NPGGuiWndTexture(wnd.imgBg);
        }
        
        protected override void _onDiscard()
        {
            _m_wBgImg?.discard();
            _m_wBgImg = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBgImg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBgImg?.discardTexture();
        }
        
        public void setQuality(EQuality _quality)
        {
            if(wnd == null || _m_wBgImg == null)
                return;

            ConsortCardBgConfig cardBgConfig = null;
            foreach (var item in wnd.cardBgConfigList)
            {
                if (item != null && item.quality == _quality)
                {
                    cardBgConfig = item;
                    break;
                }
            }
            
            if (cardBgConfig != null)
            {
                _m_wBgImg.showWnd();
                _m_wBgImg.setTexture(cardBgConfig.bgImgIndex);
            }
            else
            {
                NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((int)_quality);
                if (qualityExtRefObj != null)
                {
                    _m_wBgImg.showWnd();
                    _m_wBgImg.setTexture(qualityExtRefObj.consort_card_bg);
                }
            }
        }
    }
}