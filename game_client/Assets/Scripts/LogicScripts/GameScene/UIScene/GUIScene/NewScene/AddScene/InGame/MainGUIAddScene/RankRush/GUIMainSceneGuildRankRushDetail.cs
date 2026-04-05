using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情主界面
    /// </summary>
    public class GUIMainSceneGuildRankRushDetail : _ANPGMainGUIAddSceneResBar<GGUIWndGuildRankRushDetail>
    {
        private static GUIMainSceneGuildRankRushDetail _g_instance = new GUIMainSceneGuildRankRushDetail();
        [NotNull] 
        public static GUIMainSceneGuildRankRushDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GUIMainSceneGuildRankRushDetail();
                return _g_instance;
            }
        }

        protected override GGUIWndGuildRankRushDetail _m_wnd { get { return GGUIWndGuildRankRushDetail.instance; } }

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
            GGUIWndGuildRankRushDetail.instance.setInfo(_info, _selectTabType, _needShowGiftBtn);
        }
    }
}
