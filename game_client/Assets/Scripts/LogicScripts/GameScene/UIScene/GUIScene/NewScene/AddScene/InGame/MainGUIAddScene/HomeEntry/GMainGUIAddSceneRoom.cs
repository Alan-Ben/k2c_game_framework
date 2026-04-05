
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 卧室scene
    /// </summary>
    public class GMainGUIAddSceneRoom : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneRoom _g_instance = new GMainGUIAddSceneRoom();
        public static GMainGUIAddSceneRoom instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneRoom();

                return _g_instance;
            }
        }

        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        //显示序列
        private long _m_showSerialize;

        protected GMainGUIAddSceneRoom() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            NPGGUIWndMainRoom.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _dealQuitSceneEx()
        {
            NPGGUIWndMainRoom.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            NPGGUIWndMainRoom.instance.showWnd();

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMainRoom.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMainRoom.instance.getBarResId());

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
            
            NPGGUIWndMainRoom.instance.hideWnd(stepCounter.addDoneStepCount);


            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
        }

        public void setTDSceneLoading(bool _isLoading)
        {
            if(!isEntered)
                return;
            
            NPGGUIWndMainRoom.instance.setTDSceneLoading(_isLoading);
        }
        
        #region 隐藏UI处理

        /// <summary>
        /// 处理效果隐藏UI
        /// </summary>
        /// <param name="_durationSec"></param>
        public void dealEffectHideUI(float _durationSec)
        {
            regShowDone(() =>
            {
                //隐藏UI
                NPGGUIWndMainRoom.instance.hideWnd();
                NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
                NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);

                //如果小于0一直隐藏
                if (_durationSec < 0)
                    return;

                //延时显示UI
                _m_showSerialize = ALSerializeOpMgr.next();
                long serialize = _m_showSerialize;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (_m_showSerialize != serialize)
                        return;

                    NPGGUIWndMainRoom.instance.showWnd();

                    //最后显示Bar
                    _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMainRoom.instance.getPlayerIconResId());
                    _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMainRoom.instance.getBarResId());
                }, _durationSec);
            });
        }

        #endregion
    }
}
