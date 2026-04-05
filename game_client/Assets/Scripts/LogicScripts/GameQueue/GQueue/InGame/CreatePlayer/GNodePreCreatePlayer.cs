using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 预创角的node，触发引导用
    /// </summary>
    public class GNodePreCreatePlayer : BaseQueueNode
    {
        /// <summary>
        /// 完成之后的回调
        /// </summary>
        private Action _m_dEnteredAction;
        
        public GNodePreCreatePlayer(Action _onEnterDone = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_PRE_CREATE_PLAYER)
        {
            _m_dEnteredAction = _onEnterDone;
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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }
        
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        public override void EnterNode()
        {
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(
                () =>
                {
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(2);
                    stepCounter.regAllDoneDelegate(_onAllSceneInited);
                    
                    GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance, stepCounter.addDoneStepCount);
                    GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance, stepCounter.addDoneStepCount);
                });
        }

        

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
            Action complete = _m_dEnteredAction;
            _m_dEnteredAction = null;
            complete?.Invoke();
        }

        public override void QuitNode()
        {
           
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}