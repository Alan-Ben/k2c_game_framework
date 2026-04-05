using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏助跑提示窗口
    /// </summary>
    public class GGUIWndTreasureHuntGamePlayRunUpTip : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlayRunUpTip>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlayRunUpTip instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlayRunUpTip(); } }
        private static GGUIWndTreasureHuntGamePlayRunUpTip _g_instance;
        
        
        public GGUIWndTreasureHuntGamePlayRunUpTip() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlayRunUpTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlayRunUpTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        
        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }
        
        
        public void refreshTipTime(float _seconds)
        {
            if (wnd == null)
                return;
                
            int roundedSeconds = Mathf.RoundToInt(_seconds);
            
            wnd.setSeconds(_seconds);
            ALUGUICommon.setLabelTxt(wnd.txtRunUpTime, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_runUpTime_value, roundedSeconds));
        }
    }
}