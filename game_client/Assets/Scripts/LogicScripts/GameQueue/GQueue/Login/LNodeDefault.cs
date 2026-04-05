using System;
using ALPackage;

namespace GOE
{
    public class LNodeDefault : UIQueueBaseNode
    {
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return true; } }

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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }

        public LNodeDefault()
            : base(EUIQueueStageType.MAIN)
        {
        }

        public override void EnterNode()
        {
            //加载UI视图，暂时都用默认视图
            NPGUISceneLogin.instance.enterScene();
            NPGUISceneLogin.instance.regInitDelegate(
                () =>
                {
                    ALLog.Error($"enter NPLNodeDefault");
                });
        }

        public override void QuitNode()
        {
        }
    }
}
