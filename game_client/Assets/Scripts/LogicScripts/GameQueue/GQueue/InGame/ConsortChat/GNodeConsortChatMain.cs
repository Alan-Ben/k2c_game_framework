using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 情人互动主界面Node
    /// </summary>
    public class GNodeConsortChatMain : BaseQueueNode
    {
        private EConsortChatMainPage _m_mainPage;

        private bool _m_dontChangeLastPage;
        //多个排行榜
        public GNodeConsortChatMain(EConsortChatMainPage _mainPage) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_CONSORT_CHAT_MAIN)
        {
            _m_mainPage = _mainPage;
            _m_dontChangeLastPage = false;
        }
        //多个排行榜
        public GNodeConsortChatMain() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_CONSORT_CHAT_MAIN)
        {
            _m_dontChangeLastPage = true;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        public override bool isOnlyUINode => true;

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            GGUIWndConsortChatMain.instance.load();

            if (!_m_dontChangeLastPage)
                GGUIWndConsortChatMain.instance.setPageType(_m_mainPage);
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            GGUIWndConsortChatMain.instance.discard();
        }

        public override void doEnterNode(Action _triggerEnterDone)
        {
            GGUIWndConsortChatMain.instance.regLoadDoneDelegate(() => {
                base.doEnterNode(_triggerEnterDone);
            });
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance);
            GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);
            
            GGUIWndConsortChatMain.instance.showWnd();
        }

        public override void QuitNode()
        {
            GGUIWndConsortChatMain.instance.hideWnd();
        }
    }
}
