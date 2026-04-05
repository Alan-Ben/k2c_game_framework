using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBuildingBuild : _ANPGGUIBasicWnd<GGUIMonoBuildingBuild>
    {
        [NotNull] public static GGUIWndBuildingBuild instance { get { return _g_instance ??= new GGUIWndBuildingBuild(); } }
        private static GGUIWndBuildingBuild _g_instance;
        
        
        private NPGGuiWndTexture _m_previewTex;
        private NPGGUIWndCommonItem _m_buildCost;

        private BuildingInfo _m_buildingInfo;
        private Vector3 _m_focusPos;
        
        private new bool _m_bIsShow;
        
        
        public GGUIWndBuildingBuild() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBuildingBuild.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingBuild.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        
        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_previewTex?.showWnd();
            _m_buildCost?.showWnd();

            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD_WND_BTN, _onClickBuildBtn);
            
            _focusToPos();
        }
        protected override void _onHideWnd()
        {
            _cancelFocus();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD_WND_BTN, _onClickBuildBtn);
            
            _m_previewTex?.hideWnd();
            _m_buildCost?.hideWnd();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_previewTex?.discardTexture();
            _m_buildCost?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_previewTex?.discard();
            _m_buildCost?.discard();
            
            _m_previewTex = null;
            _m_buildCost = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onClickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onClickBuildBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnLockJump, _onClickLockJumpBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgPreviewTex != null)
                _m_previewTex = new NPGGuiWndTexture(wnd.imgPreviewTex);
            if (wnd.monoBuildCost != null)
                _m_buildCost = new NPGGUIWndCommonItem(wnd.monoBuildCost);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onClickBuildBtn);
            ALUGUICommon.combineBtnClick(wnd.btnLockJump, _onClickLockJumpBtn);
        }
        
        
        public void refreshWnd(BuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            _m_previewTex?.setTexture(_m_buildingInfo.baseRef.preview_tex_index);
            ALUGUICommon.setGameObjEnable(wnd.businessBuildingShow, _m_buildingInfo.isHasBusinessFunction());
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeEarnings, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingEmployeeEarningsPerPerson_num, _m_buildingInfo.getBusinessFunctionEmployeeEarnings().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            bool isUnlock = _m_buildingInfo.baseRef.build_condition.IsEnable(null);
            ALUGUICommon.setGameObjEnable(wnd.listUnlockedShow, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.listLockedShow, !isUnlock);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.building_desc));
            _m_buildCost?.setItem(_m_buildingInfo.baseRef.build_cost);
            ALUGUICommon.setLabelTxt(wnd.txtLockTip, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.build_condition_desc, _m_buildingInfo.baseRef.build_condition_desc_params));
        }
        public void setFocusPos(Vector3 _position)
        {
            _cancelFocus();
            _m_focusPos = _position;
            _focusToPos();
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseLastNode();
        }
        private void _onClickBuildBtn()
        {
            if (wnd == null)
                return;
            
            _onClickBuildBtn(wnd.btnBuild);
        }
        private void _onClickBuildBtn(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.buildingComp.reqBuildingBuild(_m_buildingInfo.id, _isSuc =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
                
                if (_isSuc)
                    _onClickCloseBtn(null);
            });
        }
        private void _onClickLockJumpBtn(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;
            
            NPPlayerEffectSerializeInfo.dealEffect(_m_buildingInfo.baseRef.lock_jump_btn_effect, null);
        }
        private void _focusToPos()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            Vector3 target = CameraController.instance.controlCamera.ViewportToWorldPoint(wnd.focusViewportPos);
            Vector3 center = CameraController.instance.controlCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            target = _m_focusPos + (center - target);
            float moveTime = wnd.focusMoveTime;
            float cameraSize = wnd.focusCameraSize;

            // 这里放到 mono task 中异步处理
            // 如果以拖动的手法点开这个界面，随后触发这个方法的话，场景的拖动也会随之响应，如果不放在 mono task 中，下面的聚焦操作反而会有可能被拖动操作打断，
            // 又因为解除了限制器，相机可能会飞出可视范围之外。
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                CameraController.instance.pausePosLimiter();
                MainAdditionBuildingTDScene.instance.focusToTarget(target, moveTime);
                MainAdditionBuildingTDScene.instance.setCameraSize(cameraSize, moveTime);
            });
        }
        private void _cancelFocus()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            float moveTime = wnd.focusMoveTime;
            // 这里放到 mono task 中异步处理，和上面对应，以保证执行顺序
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                CameraController.instance.resumePosLimiter();
                MainAdditionBuildingTDScene.instance.resetCameraSize(moveTime);
            });
        }
    }
}