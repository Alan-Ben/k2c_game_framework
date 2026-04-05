using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店博物馆礼品提示窗口
    /// </summary>
    public class GGUIWndInnMuseumGiftTip : _ATALBasicUIWnd<GGUIMonoInnMuseumGiftTip>
    {
        [NotNull] public static GGUIWndInnMuseumGiftTip instance { get { return _g_instance ??= new GGUIWndInnMuseumGiftTip(); } }
        private static GGUIWndInnMuseumGiftTip _g_instance;


        private InnSpecialGuestInfo _m_guestInfo;


        public GGUIWndInnMuseumGiftTip() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnMuseumGiftTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnMuseumGiftTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            // Set auto close
            if (wnd != null && wnd.autoCloseDelay > 0)
            {
                ALCommonActionMonoTask.addMonoTask(() => _onBtnCloseClick(null), wnd.autoCloseDelay);
            }
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

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd([NotNull] InnSpecialGuestInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_MUSEUM_GIFT_TIP);
        }
    }
}