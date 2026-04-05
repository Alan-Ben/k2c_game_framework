using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildDetail : _ANPGGUIBasicWnd<GGUIMonoChildDetail>
    {
        [NotNull] public static GGUIWndChildDetail instance { get { return _g_instance ??= new GGUIWndChildDetail(); } }
        private static GGUIWndChildDetail _g_instance;
        
        
        private GGUISubWndChildInfo _m_childInfoWnd;
        private GGUISubWndCommonPropertyDetail _m_otherBonus;

        private ChildInfo _m_childInfo;
        
        
        public GGUIWndChildDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();
            _m_otherBonus?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
            _m_otherBonus?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
            _m_otherBonus?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_otherBonus?.discard();
            _m_childInfoWnd = null;
            _m_otherBonus = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.monoOtherBonus != null)
                _m_otherBonus = new GGUISubWndCommonPropertyDetail(wnd.monoOtherBonus);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(ChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_childInfo);
            _m_otherBonus?.refreshWnd(_m_childInfo.bonusJudgeParts);
            ALUGUICommon.setLabelTxt(wnd.txtEducatingAwards, _m_childInfo.getEducationExpValue().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtEducatingBaseAwards, NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_get_hero_exp ?? 0);
            string bonus = (_m_childInfo.initStudyBonus / 100f).ToString();
            ALUGUICommon.setLabelTxt(wnd.txtChildQualityBonus, string.IsNullOrEmpty(wnd.childQualityBonusKey) ? bonus : TextTranslate.instance.getLanguage(wnd.childQualityBonusKey, bonus));
            wnd.setIsSuper(_m_childInfo.isSuper);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}