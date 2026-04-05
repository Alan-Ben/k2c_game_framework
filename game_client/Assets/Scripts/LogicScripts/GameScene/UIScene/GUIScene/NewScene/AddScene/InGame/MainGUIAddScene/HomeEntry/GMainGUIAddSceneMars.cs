
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 火星scene
    /// </summary>
    public class GMainGUIAddSceneMars : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneMars _g_instance = new GMainGUIAddSceneMars();
        public static GMainGUIAddSceneMars instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneMars();

                return _g_instance;
            }
        }

        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        //显示序列
        private long _m_showSerialize;

        protected GMainGUIAddSceneMars() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            NPGGUIWndMainMars.instance.load(stepCounter.addDoneStepCount);
            GGUIWndMarsHud.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _dealQuitSceneEx()
        {
            NPGGUIWndMainMars.instance.discard();
            GGUIWndMarsHud.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            NPGGUIWndMainMars.instance.showWnd();
            GGUIWndMarsHud.instance.showWnd();

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMainMars.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMainMars.instance.getBarResId());

            if (null != _delegate)
                _delegate();
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            _m_showSerialize = ALSerializeOpMgr.next();
            if (!isEntered)
            {
                if(_delegate != null)
                    _delegate();

                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_delegate);
            
            NPGGUIWndMainMars.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndMarsHud.instance.hideWnd(stepCounter.addDoneStepCount);

            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
        }
    }
}
