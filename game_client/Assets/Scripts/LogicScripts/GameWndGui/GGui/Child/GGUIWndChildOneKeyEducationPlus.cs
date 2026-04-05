using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildOneKeyEducationPlus : _ANPGGUIBasicWnd<GGUIMonoChildOneKeyEducationPlus>
    {
        [NotNull] public static GGUIWndChildOneKeyEducationPlus instance { get { return _g_instance ??= new GGUIWndChildOneKeyEducationPlus(); } }
        private static GGUIWndChildOneKeyEducationPlus _g_instance;


        public GGUIWndChildOneKeyEducationPlus() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildOneKeyEducationPlus.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildOneKeyEducationPlus.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_PLUS_TOGGLE, _onSimulateClickToggle);
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_PLUS_TOGGLE, _onSimulateClickToggle);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnOpen, _onBtnOpenClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.combineBtnClick(wnd.btnOpen, _onBtnOpenClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_plus_unlock_id);
            wnd.setLockShow(unlockRef != null && !unlockRef.isConditionEnable(null));
            wnd.setOpenShow(AccountSettingMgr.instance.accountSetting.isChildOneKeyPlusEducating);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, unlockRef?.getUnlockTip());
        }
        
        
        private void _onBtnBackClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Child.C_CHILD_ONE_KEY_EDUCATION_PLUS);
        }
        private void _onBtnOpenClick(GameObject _)
        {
            if (wnd == null)
                return;
            
            wnd.setOpenShow(true);
            AccountSettingMgr.instance.accountSetting.setChildOneKeyPlusEducating(true);
        }
        private void _onBtnCloseClick(GameObject _)
        {
            if (wnd == null)
                return;
            
            wnd.setOpenShow(false);
            AccountSettingMgr.instance.accountSetting.setChildOneKeyPlusEducating(false);
        }
        private void _onSimulateClickToggle()
        {
            if (AccountSettingMgr.instance.accountSetting.isChildOneKeyPlusEducating)
                _onBtnCloseClick(null);
            else
                _onBtnOpenClick(null);
        }
    }
}