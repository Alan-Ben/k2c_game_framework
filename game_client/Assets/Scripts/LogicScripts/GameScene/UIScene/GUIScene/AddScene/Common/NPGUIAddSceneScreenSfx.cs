
namespace GOE
{
    /// <summary>
    /// 屏幕点击特效
    /// </summary>
    public class NPGUIAddSceneScreenSfx : _ABasicAdditionUIScene_NoChild
    {
        private static NPGUIAddSceneScreenSfx _g_instance = new NPGUIAddSceneScreenSfx();
        public static NPGUIAddSceneScreenSfx instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUIAddSceneScreenSfx();

                return _g_instance;
            }
        }

        public NPGUIAddSceneScreenSfx()
            : base()
        {
        }

        protected override void _onEnterScene()
        {
            //开启窗口加载
            NPGGUIWndScreenSfx.instance.load(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            NPGGUIWndScreenSfx.instance.discard();
        }

        protected override void _onSceneInited()
        {
            NPGGUIWndScreenSfx.instance.showWnd();
        }
    }
}
