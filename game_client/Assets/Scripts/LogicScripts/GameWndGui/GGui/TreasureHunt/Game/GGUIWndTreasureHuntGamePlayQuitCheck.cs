using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏暂停退出确认对话框
    /// </summary>
    public class GGUIWndTreasureHuntGamePlayQuitCheck : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlayQuitCheck>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlayQuitCheck instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlayQuitCheck(); } }
        private static GGUIWndTreasureHuntGamePlayQuitCheck _g_instance;
        
        
        private NPGGUIWndCommonToggleEx _m_dontShowTodayToggle;
        

        public GGUIWndTreasureHuntGamePlayQuitCheck()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlayQuitCheck.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlayQuitCheck.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_dontShowTodayToggle?.setSelected(false);
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            _m_dontShowTodayToggle?.discard();
            _m_dontShowTodayToggle = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoDontShowToday != null)
                _m_dontShowTodayToggle = new NPGGUIWndCommonToggleEx(wnd.monoDontShowToday);
            
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
        }
        
        
        private void _onBtnCancelClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_QUIT_CHECK);
        }
        private void _onBtnConfirmClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_MAIN);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_QUIT_CHECK);
            GNodeTreasureHuntGameMain.addNode();
            if (_m_dontShowTodayToggle is { isOn: true })
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.TREASURE_HUNT_GAMEPLAY_QUIT);
        }
    }
}