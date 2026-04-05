using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 跑马灯窗口
    /// </summary>
    public class GGUIWndMarquee : _ATALBasicUIWnd<GGUIMonoMarquee>
    {
        private static GGUIWndMarquee _g_instance;
        public static GGUIWndMarquee instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarquee();
                return _g_instance;
            }
        }

        public GGUIWndMarquee() : base(EALUIWndLayer.TOP)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarquee.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarquee.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }
    }

}