using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildGraduatedDetail : _ANPGGUIBasicWnd<GGUIMonoChildGraduatedDetail>
    {
        [NotNull] public static GGUIWndChildGraduatedDetail instance { get { return _g_instance ??= new GGUIWndChildGraduatedDetail(); } }
        private static GGUIWndChildGraduatedDetail _g_instance;

        
        private GGUISubWndCommonPropertyDetail _m_otherBonusWnd;
        private _IChildInfo _m_childInfo;
        

        public GGUIWndChildGraduatedDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildGraduatedDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildGraduatedDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_otherBonusWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_otherBonusWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_otherBonusWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_otherBonusWnd?.discard();
            _m_otherBonusWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoOtherBonus != null)
                _m_otherBonusWnd = new GGUISubWndCommonPropertyDetail(wnd.monoOtherBonus);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(_IChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
            refreshWnd();
        }
        private void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;

            if (_m_childInfo.guardianRef == null)
            {
                ALLog.Error("GGUIWndChildGraduatedDetail.refreshWnd: _m_childInfo.guardianRef = null");
                return;
            }
            
            PlayerLvlRefObj curPlayerLvlRefObj = NPPlayer.instance.playerInfo.curLevelRef;
            GGottenConsortInfo guardianInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_childInfo.guardianRef.id);
            if (curPlayerLvlRefObj == null || guardianInfo == null)
            {
                ALLog.Error("GGUIWndChildGraduatedDetail.refreshWnd: curPlayerLvlRefObj == null || guardianInfo == null");
                return;
            }
            
            wnd.txtBaseEarnings.setValue(curPlayerLvlRefObj.child_educate_get_base_earnings);
            long intimacy = guardianInfo.intimacy;
            long caretakerCoe = guardianInfo.fetterInfo.consortFettersLvlRef.caretaker_bonus_calculate_coefficient;
            wnd.txtGuardianBonus.setValue(intimacy * caretakerCoe / 10000f);
            wnd.txtTalentBonus.setValue(guardianInfo.fetterInfo.consortFettersLvlRef.income_bonus / 100f);
            wnd.txtCareerBonus.setValue(_m_childInfo.careerRef.career_add / 100f);
            wnd.txtQualityBonus.setValue(curPlayerLvlRefObj.child_graduate_get_earnings_add / 100f);
            _m_otherBonusWnd?.refreshWnd(_m_childInfo.bonusJudgeParts);
            wnd.setIsSuper(_m_childInfo.isSuper);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}