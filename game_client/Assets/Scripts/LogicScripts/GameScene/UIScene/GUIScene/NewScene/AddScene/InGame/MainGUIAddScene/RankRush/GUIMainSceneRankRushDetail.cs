using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 冲榜详情主界面
    /// </summary>
    public class GUIMainSceneRankRushDetail : _ANPGMainGUIAddSceneResBar<GGUIWndRankRushDetail>
    {
        private static GUIMainSceneRankRushDetail _g_instance = new GUIMainSceneRankRushDetail();
        [NotNull] 
        public static GUIMainSceneRankRushDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GUIMainSceneRankRushDetail();
                return _g_instance;
            }
        }

        protected override GGUIWndRankRushDetail _m_wnd { get { return GGUIWndRankRushDetail.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }

        public void setInfo(ActivityRankRushInfo _info, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn)
        {
            GGUIWndRankRushDetail.instance.setInfo(_info, _selectTabType, _needShowGiftBtn);
        }
    }
}
