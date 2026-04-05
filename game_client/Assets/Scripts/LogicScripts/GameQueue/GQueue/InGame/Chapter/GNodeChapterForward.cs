
using System;
using JetBrains.Annotations;

namespace GOE
{
    //关卡前进node
    public class GNodeChapterForward : UIQueueBaseNode
    {
        private int _m_enterSerialize;
        [NotNull] private ChapterMainViewMgr _m_chapterMainView;
        
        public GNodeChapterForward() 
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Chapter.C_CHAPTER)
        {
            _m_chapterMainView = new ChapterMainViewMgr();
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
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        public override void EnterNode()
        {
            
        }

        public override void QuitNode()
        {
            _m_enterSerialize++;
            
            _m_chapterMainView.discard();
        }

        public override void onEnterQueue()
        {
        }
        
        public override void onClose()
        {
        }
        
        
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneChapterMain.instance, () =>
            {
                GMainGUIAddSceneChapterMain.instance.showChapterForward();
                _m_chapterMainView.init(_triggerEnterDone);
            });
        }
    }
}