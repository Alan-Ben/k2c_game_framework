using System;

namespace GOE
{
    /// <summary>
    /// 妃子Scene
    /// </summary>
    public class GMainGUIMainSceneConsort : _ANPGMainGUIAddSceneResBar
    {
        private static GMainGUIMainSceneConsort _g_instance = new GMainGUIMainSceneConsort();
        public static GMainGUIMainSceneConsort instance { get { return _g_instance ??= new GMainGUIMainSceneConsort(); } }

        protected override void _onEnterScene()
        {
            setSceneInited();
        }

        protected override void _onSceneInited()
        {
        }

        public override void _dealShowScene(Action _delegate)
        {
            _delegate?.Invoke();
        }

        protected override void _dealQuitSceneSub()
        {
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            _delegate?.Invoke();
        }
    }
}