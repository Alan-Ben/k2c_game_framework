using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 冲榜详情主界面
    /// </summary>
    public class GUIMainSceneRankRushMultipleDetail : _ANPGMainGUIAddSceneResBar<GGUIWndRankRushMultipleDetail>
    {
        private static GUIMainSceneRankRushMultipleDetail _g_instance = new GUIMainSceneRankRushMultipleDetail();
        [NotNull] 
        public static GUIMainSceneRankRushMultipleDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GUIMainSceneRankRushMultipleDetail();
                return _g_instance;
            }
        }

        protected override GGUIWndRankRushMultipleDetail _m_wnd { get { return GGUIWndRankRushMultipleDetail.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }

        public void setInfo(long _activityId, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn)
        {
            GGUIWndRankRushMultipleDetail.instance.setInfo(_activityId, _selectTabType, _needShowGiftBtn);
        }
    }
}
