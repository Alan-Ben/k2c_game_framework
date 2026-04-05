using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴主窗口
    /// </summary>
    public class GMainGUIAddSceneHeroInfo : _ANPGMainGUIAddSceneResBar<GGUIWndHeroInfo>
    {
        private static GMainGUIAddSceneHeroInfo _g_instance = new GMainGUIAddSceneHeroInfo();
        [NotNull] 
        public static GMainGUIAddSceneHeroInfo instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIAddSceneHeroInfo();
                return _g_instance;
            }
        }
        
        protected override GGUIWndHeroInfo _m_wnd { get { return GGUIWndHeroInfo.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }

        public void setInfo(HeroCardShowInfo _heroShowInfo, List<HeroCardShowInfo> _heroShowInfoList, bool _showEnterAni)
        {
            GGUIWndHeroInfo.instance.setInfo(_heroShowInfo, _heroShowInfoList, _showEnterAni);
        }
    }
}
