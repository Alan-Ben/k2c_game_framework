using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBuildingBuildSuccess : _ANPGGUIBasicWnd<GGUIMonoBuildingBuildSuccess>
    {
        [NotNull] public static GGUIWndBuildingBuildSuccess instance { get { return _g_instance ??= new GGUIWndBuildingBuildSuccess(); } }
        private static GGUIWndBuildingBuildSuccess _g_instance;
        
        
        private NPGGuiWndTexture _m_previewTex;
        private BuildingRefObj _m_buildingRef;
        private BusinessBuildingRefObj _m_businessRef;
        private Action _m_aOnCloseWnd;
        

        public GGUIWndBuildingBuildSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBuildingBuildSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingBuildSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_previewTex?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_previewTex?.hideWnd();
            _m_aOnCloseWnd?.Invoke();
            _m_aOnCloseWnd = null;
        }
        protected override void _onReset()
        {
            _m_previewTex?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_previewTex?.discard();
            _m_previewTex = null;
            
            //建造表现完成引导trigger
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.BUILD_BUILD_EFFECT_WND_END);
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgPreviewTex != null)
                _m_previewTex = new NPGGuiWndTexture(wnd.imgPreviewTex);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(BuildingRefObj _buildingRef, Action _onCloseWnd)
        {
            _m_buildingRef = _buildingRef;
            _m_businessRef = GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_buildingRef?.id ?? 0);
            _m_aOnCloseWnd = _onCloseWnd;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingRef == null)
                return;
            
            _m_previewTex?.setTexture(_m_buildingRef.preview_tex_index);
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.building_buildSuccessTitle_name, TextTranslate.instance.getLanguage(_m_buildingRef.name)));
            ALUGUICommon.setLabelTxt(wnd.txtBuildDesc, TextTranslate.instance.getLanguage(_m_buildingRef.build_desc));
            ALUGUICommon.setLabelTxt(wnd.txtZeroEarnings, TextTranslate.instance.getLanguage(TransKeyConst.building_earningsPerPerson_num, 0L.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtEarningsPerPerson, TextTranslate.instance.getLanguage(TransKeyConst.building_earningsPerPerson_num, (_m_businessRef?.employee_earnings ?? 0).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtZeroCapacity, 0);
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeCapacity, _m_businessRef?.employee_base_max_count ?? 0);
            ALUGUICommon.setGameObjEnable(wnd.businessBuildingShow, _m_businessRef != null);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUILDING_BUILD_SUC);
        }
    }
}