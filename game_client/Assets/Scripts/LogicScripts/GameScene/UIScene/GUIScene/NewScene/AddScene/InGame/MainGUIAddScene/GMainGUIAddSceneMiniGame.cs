using System;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneMiniGame : _ANPBasicAddContainerUIScene
    {
        [NotNull] public static GMainGUIAddSceneMiniGame instance { get { return _g_instance ??= new GMainGUIAddSceneMiniGame(); } }
        private static GMainGUIAddSceneMiniGame _g_instance;
        
        protected override void _onEnterScene()
        {
            setSceneInited();
        }

        protected override void _dealQuitScene()
        {
        }
        
        protected override void _onSceneInited()
        {
        }

        public override void _dealShowScene(Action _delegate)
        {
            _delegate?.Invoke();
        }

        public override void _dealHideScene(Action _delegate)
        {
            _delegate?.Invoke();
        }
    }
}