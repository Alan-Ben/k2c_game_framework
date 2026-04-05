
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 空间站scene
    /// </summary>
    public class GMainGUIAddSceneSpaceStation : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneSpaceStation _g_instance = new GMainGUIAddSceneSpaceStation();
        public static GMainGUIAddSceneSpaceStation instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneSpaceStation();

                return _g_instance;
            }
        }

        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        //显示序列
        private long _m_showSerialize;

        protected GMainGUIAddSceneSpaceStation() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            NPGGUIWndMainSpaceStation.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _dealQuitSceneEx()
        {
            NPGGUIWndMainSpaceStation.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            NPGGUIWndMainSpaceStation.instance.showWnd();

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMainSpaceStation.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMainSpaceStation.instance.getBarResId());

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
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_delegate);
            
            NPGGUIWndMainSpaceStation.instance.hideWnd(stepCounter.addDoneStepCount);


            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
        }
        
    }
}
