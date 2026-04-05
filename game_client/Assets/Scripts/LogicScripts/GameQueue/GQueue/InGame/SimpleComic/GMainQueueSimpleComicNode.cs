using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 对话节点
    /// </summary>
    public class GMainQueueSimpleComicNode : UIQueueBaseNode
    {
        private bool _m_bIsMain;//该节点是否为MainWnd
        private SimpleComicRefObj _m_refObj;//漫画配置
        private Action _m_aDoneAction;//对话结束回调
        
        public GMainQueueSimpleComicNode(SimpleComicRefObj _refObj, Action _doneAction, bool _isMain = true) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_SIMPLE_COMIC)
        {
            _m_refObj = _refObj;
            _m_aDoneAction = _doneAction;
            _m_bIsMain = _isMain;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return _m_bIsMain; } }
        /// <summary>
        /// 禁止esc回退
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }
        
        public override void EnterNode()
        {
            if (_m_bIsMain)
            {
                GUISceneMain.instance.showMainWnd(GGUIWndSimpleComic.instance, showDelegate);
            }
            else
            {
                GUISceneMain.instance.showAddWnd(GGUIWndSimpleComic.instance, showDelegate);
            }

            void showDelegate()
            {
                GGUIWndSimpleComic.instance.showWnd();
                GGUIWndSimpleComic.instance.initShowInfo(_m_refObj);
            }
        }

        public override void QuitNode()
        {
            GGUIWndSimpleComic.instance.hideWnd();
        }

        public override void onClose()
        {
            if (null != _m_aDoneAction)
                _m_aDoneAction();
            _m_aDoneAction = null;
            
            //发送漫画结束的的引导消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.COMIC_END);
        }
    }
}
