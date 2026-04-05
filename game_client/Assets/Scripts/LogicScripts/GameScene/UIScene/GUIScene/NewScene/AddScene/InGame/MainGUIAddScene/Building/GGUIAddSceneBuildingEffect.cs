using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIAddSceneBuildingEffect : _ANPBasicAddContainerUIScene
    {
        [NotNull] public static GGUIAddSceneBuildingEffect instance { get { return _g_instance ??= new GGUIAddSceneBuildingEffect(); } }
        private static GGUIAddSceneBuildingEffect _g_instance;


        protected override void _onEnterScene()
        {
            GGUIWndBuildingEffect.instance.load(setSceneInited);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndBuildingEffect.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndBuildingEffect.instance.showWnd();
            _delegate?.Invoke();
        }

        public override void _dealHideScene(Action _delegate)
        {
            if (!isEntered)
            {
                _delegate?.Invoke();
                return;
            }

            GGUIWndBuildingEffect.instance.hideWnd(_delegate);
        }


        public void playAnimation(Action _complete = null)
        {
            GGUIWndBuildingEffect.instance.playAnimation(_complete);
        }
    }
}
