using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneInn : _ANPGMainGUIAddSceneResBar
    {
        [NotNull] public static GMainGUIAddSceneInn instance { get { return _g_instance ??= new GMainGUIAddSceneInn(); } }
        private static GMainGUIAddSceneInn _g_instance;
        
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(4);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            GGUIWndInnMain.instance.load(stepCounter.addDoneStepCount);
            GGUIWndInnHud.instance.load(stepCounter.addDoneStepCount);
            GGUIWndInnNewGuest.instance.load(stepCounter.addDoneStepCount);
            GGUIWndInnSpecialGuestComingTip.instance.load(stepCounter.addDoneStepCount);
        }
        protected override void _onSceneInited()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg += refreshNewGuestPart;
        }
        public override void _dealShowScene(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_delegate);
            
            GGUIWndInnMain.instance.showWnd(stepCounter.addDoneStepCount);
            GGUIWndInnHud.instance.showWnd(stepCounter.addDoneStepCount);
            showResBar(GGUIWndInnMain.instance.getBarResId(), GGUIWndInnMain.instance.getPlayerIconResId());
            
            refreshNewGuestPart();
        }
        protected override void _dealQuitSceneSub()
        {
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg -= refreshNewGuestPart;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            
            GGUIWndInnMain.instance.discard();
            GGUIWndInnHud.instance.discard();
            GGUIWndInnNewGuest.instance.discard();
            GGUIWndInnSpecialGuestComingTip.instance.discard();
        }
        protected override void _dealHideSceneSub(Action _delegate)
        {
            GGUIWndInnMain.instance.hideWnd();
            GGUIWndInnHud.instance.hideWnd();
            GGUIWndInnNewGuest.instance.hideWnd();
            GGUIWndInnSpecialGuestComingTip.instance.hideWnd();
            _delegate?.Invoke();
        }
        
        public void refreshNewGuestPart()
        {
            InnSpecialGuestInfo specialGuestInfo = NPPlayer.instance.innComp.tryGetFirstSpecialGuest();
            if (specialGuestInfo != null)
            {
                GGUIWndInnSpecialGuestComingTip.instance.refreshWnd(specialGuestInfo);
                GGUIWndInnSpecialGuestComingTip.instance.showWnd();
                GGUIWndInnNewGuest.instance.hideWnd();
                return;
            }
            
            GGUIWndInnSpecialGuestComingTip.instance.hideWnd();
            InnNormalGuestHandbookInfo newUnlockGuest = NPPlayer.instance.innComp.tryGetFirstUnlockableNormalGuest();
            if (newUnlockGuest != null)
            {
                GGUIWndInnNewGuest.instance.refreshWnd(newUnlockGuest);
                GGUIWndInnNewGuest.instance.showWnd();
            }
            else
                GGUIWndInnNewGuest.instance.hideWnd();
        }
        
        private void _onNodeChg()
        {
            if (QueueMgr.instance._lastNode is GNodeInnMain)
                refreshNewGuestPart();
        }
    }
}