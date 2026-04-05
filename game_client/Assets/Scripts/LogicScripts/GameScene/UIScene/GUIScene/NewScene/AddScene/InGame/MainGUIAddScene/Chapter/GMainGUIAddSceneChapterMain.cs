using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneChapterMain : _ANPBasicAddContainerUIScene
    {
        [NotNull] public static GMainGUIAddSceneChapterMain instance { get { return _g_instance ??= new GMainGUIAddSceneChapterMain(); } }
        private static GMainGUIAddSceneChapterMain _g_instance;
        
        private GGUIWndChapterDialogTip _m_dialogTipWnd;
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(setSceneInited);
            stepCounter.chgTotalStepCount(3);

            GGUIWndChapterMainVideoSwitch.instance.load(stepCounter.addDoneStepCount);
            GGUIWndChapterMain.instance.load(stepCounter.addDoneStepCount);
            GGUIWndChapterBossMain.instance.load(stepCounter.addDoneStepCount);
        }
        
        protected override void _onSceneInited()
        {
        }
        
        protected override void _dealQuitScene()
        {
            GGUIWndChapterMainVideoSwitch.instance.discard();
            GGUIWndChapterMain.instance.discard();
            GGUIWndChapterBossMain.instance.discard();
        }
        
        
        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndChapterMainVideoSwitch.instance.showWnd();
            
            if (null != _delegate)
                _delegate();
        }
        
        public override void _dealHideScene(Action _delegate)
        {
            if (!isEntered)
            {
                if(_delegate != null)
                    _delegate();
                return;
            } ;
            
            GGUIWndChapterMain.instance.hideWnd();
            GGUIWndChapterMainVideoSwitch.instance.hideWnd();
            GGUIWndChapterBossMain.instance.hideWnd();

            if (_m_dialogTipWnd != null) 
                _m_dialogTipWnd.discard();
            _m_dialogTipWnd = null;
            
            if (null != _delegate)
                _delegate();
        }
        
        /// <summary>
        /// 显示关卡投资前进
        /// </summary>
        public void showChapterForward()
        {
            GGUIWndChapterBossMain.instance.hideWnd();
            GGUIWndChapterMain.instance.showWnd();
            GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChapterMain.instance.getGameObj());
        }
        
        /// <summary>
        /// 显示关卡boss战
        /// </summary>
        /// <param name="_chapterRef"></param>
        public void showChapterBoss(ChapterRefObj _chapterRef)
        {
            GGUIWndChapterMain.instance.hideWnd();
            GGUIWndChapterBossMain.instance.showWnd(() =>
            {
                GGUIWndChapterBossMain.instance.setInfo(_chapterRef);
            });
            GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChapterBossMain.instance.getGameObj()); 
        }
        
        //展示提示
        public void showDialogTipWnd(long _uiResPathId, NPGTextureIndex _index, string _name, Action _doneAction)
        {
            if (null == _m_dialogTipWnd)
            {
                _m_dialogTipWnd = new GGUIWndChapterDialogTip(_uiResPathId);
                _m_dialogTipWnd.load();
            }
            
            _m_dialogTipWnd.regLoadDoneDelegate(() =>
            {
                _m_dialogTipWnd.showWnd();
                _m_dialogTipWnd.setInfo(_index, _name, _doneAction);
            });
        }
        
        public void hideDialogTipWnd()
        {
            if (_m_dialogTipWnd != null) 
                _m_dialogTipWnd.discard();
            _m_dialogTipWnd = null;
        }
        
        public void showEventPreTipWnd(Action _onEventPreTipClose)
        {
            showAddWnd(GGUIWndChapterEventPreTip.instance, () =>
            {
                GGUIWndChapterEventPreTip.instance.setInfo(_onEventPreTipClose);
            });
        }
        
        public void hideEventPreTipWnd()
        {
            GGUIWndChapterEventPreTip.instance.hideWnd();
        }
    }
}