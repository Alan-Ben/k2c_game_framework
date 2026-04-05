using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴未解锁主窗口
    /// </summary>
    public class GMainGUIAddSceneHeroLockInfo : _ANPGMainGUIAddSceneResBar<GGUIWndHeroLockInfo>
    {
        private static GMainGUIAddSceneHeroLockInfo _g_instance = new GMainGUIAddSceneHeroLockInfo();
        [NotNull] 
        public static GMainGUIAddSceneHeroLockInfo instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIAddSceneHeroLockInfo();
                return _g_instance;
            }
        }
        
        protected override GGUIWndHeroLockInfo _m_wnd { get { return GGUIWndHeroLockInfo.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }

        public void setInfo(HeroCardShowInfo _heroShowInfo, List<HeroCardShowInfo> _heroShowInfoList)
        {
            GGUIWndHeroLockInfo.instance.setInfo(_heroShowInfo, _heroShowInfoList);
        }
    }
}
