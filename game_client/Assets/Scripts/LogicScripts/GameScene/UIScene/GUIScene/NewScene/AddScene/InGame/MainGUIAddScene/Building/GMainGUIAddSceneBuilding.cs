using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneBuilding : _ANPBasicAddContainerUIScene
    {
        [NotNull] public static GMainGUIAddSceneBuilding instance { get { return _g_instance ??= new GMainGUIAddSceneBuilding(); } }
        private static GMainGUIAddSceneBuilding _g_instance;


        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(setSceneInited);
            stepCounter.chgTotalStepCount(3);
            
            GGUIWndHomeEntryFollow.instance.load(stepCounter.addDoneStepCount);
            GGUIWndBuildingFollow.instance.load(stepCounter.addDoneStepCount);
            GGUIWndBuildingMain.instance.load(stepCounter.addDoneStepCount);
        }
        
        protected override void _onSceneInited()
        {
        }
        
        protected override void _dealQuitScene()
        {
            GGUIWndHomeEntryFollow.instance.discard();
            GGUIWndBuildingMain.instance.discard();
            GGUIWndBuildingFollow.instance.discard();
        }
        
        
        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndHomeEntryFollow.instance.showWnd();
            GGUIWndBuildingMain.instance.showWnd();
            GGUIWndBuildingFollow.instance.showWnd();

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(GGUIWndBuildingMain.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(GGUIWndBuildingMain.instance.getBarResId());

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
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(_delegate);
            
            GGUIWndHomeEntryFollow.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndBuildingMain.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndBuildingFollow.instance.hideWnd(stepCounter.addDoneStepCount);

            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
        }

        public void showWndsGO()
        {
            ALUGUICommon.setGameObjEnable(GGUIWndHomeEntryFollow.instance.wnd, true);
            ALUGUICommon.setGameObjEnable(GGUIWndBuildingMain.instance.wnd, true);
            ALUGUICommon.setGameObjEnable(GGUIWndBuildingFollow.instance.wnd, true);

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(GGUIWndBuildingMain.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(GGUIWndBuildingMain.instance.getBarResId());
        }
        public void hideWndsGO()
        {
            ALUGUICommon.setGameObjEnable(GGUIWndHomeEntryFollow.instance.wnd, false);
            ALUGUICommon.setGameObjEnable(GGUIWndBuildingMain.instance.wnd, false);
            ALUGUICommon.setGameObjEnable(GGUIWndBuildingFollow.instance.wnd, false);
            
            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
        }
    }
}