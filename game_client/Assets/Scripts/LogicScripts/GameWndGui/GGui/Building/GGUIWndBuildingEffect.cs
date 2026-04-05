using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndBuildingEffect : _ANPGGUIBasicWnd<GGUIMonoBuildingEffect>
    {
        [NotNull] public static GGUIWndBuildingEffect instance { get { return _g_instance ??= new GGUIWndBuildingEffect(); } }
        private static GGUIWndBuildingEffect _g_instance;


        public GGUIWndBuildingEffect()
            : base(EALUIWndLayer.NORMAL)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoBuildingEffect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingEffect.objName; } }
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
        }
        protected override void _onWndInitDone()
        {
        }


        public void playAnimation(Action _complete = null)
        {
            if (wnd == null || !_m_bIsShow || wnd.anim == null)
            {
                _complete?.Invoke();
                return;
            }

            wnd.anim.Play(wnd.animName, _complete);
        }
    }
}
