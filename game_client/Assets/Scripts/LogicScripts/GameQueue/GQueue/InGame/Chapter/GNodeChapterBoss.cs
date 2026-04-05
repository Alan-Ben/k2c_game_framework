using System;
using ALPackage;

namespace GOE
{
    //关卡boss的node
    public class GNodeChapterBoss : UIQueueBaseNode
    {
        private ChapterRefObj _m_chapterRef;
        private Action _m_closeAction;

        public GNodeChapterBoss(ChapterRefObj _chapterRef, Action _doneAction)
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Chapter.C_CHAPTER_BOSS)
        {
            _m_chapterRef = _chapterRef;
            _m_closeAction = _doneAction;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneChapterMain.instance, () =>
            {
                GMainGUIAddSceneChapterMain.instance.showChapterBoss(_m_chapterRef);
            });
        }

        public override void QuitNode()
        {
           
        }

        public override void onClose()
        {
            if (null != _m_closeAction)
                _m_closeAction();
            _m_closeAction = null;
        }
    }
}
