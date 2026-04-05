using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民获取提示
    /// </summary>
    public class GGUIWndMarsPopularGetTip : _ANPGGUIBasicWnd<GGUIMonoMarsPopularGetTip>
    {
        [NotNull] public static GGUIWndMarsPopularGetTip instance { get { return _g_instance ??= new GGUIWndMarsPopularGetTip(); } }
        private static GGUIWndMarsPopularGetTip _g_instance;


        private GGUIWndMarsPopularGetTip()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsPopularGetTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPopularGetTip.objName; } }
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

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_GET_TIP);
        }
    }
}
