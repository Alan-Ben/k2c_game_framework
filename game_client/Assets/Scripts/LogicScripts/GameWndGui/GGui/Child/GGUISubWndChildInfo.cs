using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    public class GGUISubWndChildInfo : _ANPGGUIBasicSubWnd<GGUIMonoChildInfo>
    {
        private _IChildInfo _m_childInfo;
        
        private NPGGuiWndTexture _m_icon;
        private NPGGuiWndTexture _m_cardIcon;
        private NPGGuiWndTexture _m_attrIcon;
        private NPGGuiWndTexture _m_qualityIcon;
        private NPGGuiWndTexture _m_qualityBg;
        private NPGGUIWndCommonShowCase _m_tdShowCase;
        private NPGGUIWndCommonShowCase _m_miniTdShowCase;
        private NPGGuiWndTexture _m_guardianIcon;
        private GGuiWndSprite _m_guardianIconBg;


        public GGUISubWndChildInfo(GGUIMonoChildInfo _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_icon?.showWnd();
            _m_cardIcon?.showWnd();
            _m_attrIcon?.showWnd();
            _m_qualityIcon?.showWnd();
            _m_qualityBg?.showWnd();
            _m_tdShowCase?.showWnd();
            _m_miniTdShowCase?.showWnd();
            _m_guardianIcon?.showWnd();
            _m_guardianIconBg?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_icon?.hideWnd();
            _m_cardIcon?.hideWnd();
            _m_attrIcon?.hideWnd();
            _m_qualityIcon?.hideWnd();
            _m_qualityBg?.hideWnd();
            _m_tdShowCase?.hideWnd();
            _m_miniTdShowCase?.hideWnd();
            _m_guardianIcon?.hideWnd();
            _m_guardianIconBg?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_icon?.discardTexture();
            _m_cardIcon?.discardTexture();
            _m_attrIcon?.discardTexture();
            _m_qualityIcon?.discardTexture();
            _m_qualityBg?.discardTexture();
            _m_tdShowCase?.resetWnd();
            _m_miniTdShowCase?.resetWnd();
            _m_guardianIcon?.discardTexture();
            _m_guardianIconBg?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_icon?.discard();
            _m_cardIcon?.discard();
            _m_attrIcon?.discard();
            _m_qualityIcon?.discard();
            _m_qualityBg?.discard();
            _m_tdShowCase?.discard();
            _m_miniTdShowCase?.discard();
            _m_guardianIcon?.discard();
            _m_guardianIconBg?.discard();
            _m_icon = null;
            _m_cardIcon = null;
            _m_attrIcon = null;
            _m_qualityIcon = null;
            _m_qualityBg = null;
            _m_tdShowCase = null;
            _m_miniTdShowCase = null;
            _m_guardianIcon = null;
            _m_guardianIconBg = null;

            if (wnd == null)
                return;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_icon = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.imgCardIcon != null)
                _m_cardIcon = new NPGGuiWndTexture(wnd.imgCardIcon);
            if (wnd.imgAttrIcon != null)
                _m_attrIcon = new NPGGuiWndTexture(wnd.imgAttrIcon);
            if (wnd.imgQuality != null)
                _m_qualityIcon = new NPGGuiWndTexture(wnd.imgQuality);
            if (wnd.imgQualityBg != null)
                _m_qualityBg = new NPGGuiWndTexture(wnd.imgQualityBg);
            if (wnd.scTdShow != null)
                _m_tdShowCase = new NPGGUIWndCommonShowCase(wnd.scTdShow);
            if (wnd.scTdMiniShow != null)
                _m_miniTdShowCase = new NPGGUIWndCommonShowCase(wnd.scTdMiniShow);
            if(wnd.imgGuardianIcon != null)
                _m_guardianIcon = new NPGGuiWndTexture(wnd.imgGuardianIcon);
            if(wnd.imgGuardianIconBg != null)
                _m_guardianIconBg = new GGuiWndSprite(wnd.imgGuardianIconBg);
        }


        public void refreshWnd(_IChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;

            //监护人头像、头像框
            if (_m_childInfo.guardianRef != null)
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_childInfo.guardianRef.id);
                if (consortInfo != null)
                {
                    _m_guardianIcon?.setTexture(GCommon.getItemTexIcon(ENPItemType.CONSORT_SKIN, consortInfo.consortSkinShowInfo?.skinId ?? 0));
                    _m_guardianIconBg?.setTexture(GCommon.getQualityExtRefObj(ENPItemType.CONSORT, consortInfo.consortId)?.consort_head_bg);
                }
            }

            _m_icon?.setTexture(_m_childInfo.resRef?.icon);
            _m_cardIcon?.setTexture(_m_childInfo.resRef?.card_icon);
            _m_tdShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_childInfo.resRef?.td_show));
            _m_miniTdShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_childInfo.resRef?.td_mini_show));
            _m_attrIcon?.setTexture(_m_childInfo.basicAttrRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtName, string.IsNullOrEmpty(wnd.nameKey) ? _m_childInfo.name : TextTranslate.instance.getLanguage(wnd.nameKey, _m_childInfo.name));
            ALUGUICommon.setLabelTxt(wnd.txtGuardian, string.IsNullOrEmpty(wnd.guardianKey) ? _m_childInfo.guardianRef?.transName : TextTranslate.instance.getLanguage(wnd.guardianKey, _m_childInfo.guardianRef?.transName));
            ALUGUICommon.setLabelTxt(wnd.txtCareer, string.IsNullOrEmpty(wnd.careerKey) ? _m_childInfo.careerRef.transName : TextTranslate.instance.getLanguage(wnd.careerKey, _m_childInfo.careerRef.transName));
            ALUGUICommon.setLabelTxt(wnd.txtQuality, string.IsNullOrEmpty(wnd.qualityKey) ? _m_childInfo.qualityRef.nameTranslated : TextTranslate.instance.getLanguage(wnd.qualityKey, _m_childInfo.qualityRef.nameTranslated));
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, string.IsNullOrEmpty(wnd.intimacyKey) ? _m_childInfo.initIntimacy.ToString() : TextTranslate.instance.getLanguage(wnd.intimacyKey, _m_childInfo.initIntimacy));
            if (wnd.txtPlayerName != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtPlayerName, string.Empty);
                _m_childInfo.getPlayerName(_name =>
                {
                    if (wnd == null)
                        return;
                    
                    ALUGUICommon.setLabelTxt(wnd.txtPlayerName, string.IsNullOrEmpty(wnd.playerNameKey) ? _name : TextTranslate.instance.getLanguage(wnd.playerNameKey, _name));
                });
            }
            _m_qualityIcon?.setTexture(_m_childInfo.qualityRef.icon);
            _m_qualityBg?.setTexture(_m_childInfo.qualityRef.bg);
            ALUGUICommon.setLabelTxt(wnd.txtEarnings, string.IsNullOrEmpty(wnd.earningsKey) ? _m_childInfo.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD) : TextTranslate.instance.getLanguage(wnd.earningsKey, _m_childInfo.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setGameObjEnable(wnd.listSuperShow, _m_childInfo.isSuper);
            wnd.setSexShow(_m_childInfo.sex == EChildSexType.BOY);
        }
    }
}