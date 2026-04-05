
namespace GOE
{
    /************************
    * 引导界面遮罩AdditionScene
    **/
    public class NPGTutorialGUIAddSceneMovementMask : _ABasicAdditionUIScene_NoChild
    {
        private static NPGTutorialGUIAddSceneMovementMask _g_instance = new NPGTutorialGUIAddSceneMovementMask();
        public static NPGTutorialGUIAddSceneMovementMask instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGTutorialGUIAddSceneMovementMask();

                return _g_instance;
            }
        }

        private NPGGUIMonoTutorialWndStepObj _m_liMoveLevelInfo;

        public NPGTutorialGUIAddSceneMovementMask()
            : base()
        {
            _m_liMoveLevelInfo = null;
        }
        protected override void _onEnterScene()
        {
            //开启窗口加载
            NPGGUIWndMovementMask.instance.load();
            NPGGUIWndMovementMask.instance.regLoadDoneDelegate(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            NPGGUIWndMovementMask.instance.discard();
        }

        protected override void _onSceneInited()
        {
            //展示窗口
            NPGGUIWndMovementMask.instance.showWnd();
        }

        //use the special levelinfo enter scene
        public void enterScene(NPGGUIMonoTutorialWndStepObj _enterLevelInfo)
        {
            if(null == _enterLevelInfo)
                return;

            if(isEntered)
                quitScene();

            _m_liMoveLevelInfo = _enterLevelInfo;

            enterScene();
        }

        /** check the level info to judge if this op can quit scene */
        public void quitScene(NPGGUIMonoTutorialWndStepObj _enterLevelInfo)
        {
            if(null == _m_liMoveLevelInfo || _m_liMoveLevelInfo != _enterLevelInfo)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("NPGGUIAddSceneMovementMask, null == _m_liMoveLevelInfo || _m_liMoveLevelInfo != _enterLevelInfo ");
#endif
                return;
            }
            quitScene();
        }
    }
}
