
namespace GOE
{
    /// <summary>
    /// 跑马灯
    /// </summary>
    public class NPGUIAddSceneMarquee : _ABasicAdditionUIScene_NoChild
    {
        private static NPGUIAddSceneMarquee _g_instance = new NPGUIAddSceneMarquee();
        public static NPGUIAddSceneMarquee instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUIAddSceneMarquee();

                return _g_instance;
            }
        }

        public NPGUIAddSceneMarquee()
            : base()
        {
        }

        protected override void _onEnterScene()
        {
            //开启窗口加载
            GGUIWndMarquee.instance.load(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            GGUIWndMarquee.instance.discard();
        }

        protected override void _onSceneInited()
        {
            GGUIWndMarquee.instance.showWnd();
        }
    }
}
