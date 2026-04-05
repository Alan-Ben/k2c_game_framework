
using System;
using ALPackage;
using ClientEnum;

namespace GOE
{
    public class GNodeDinnerMain : BaseQueueNode
    {
        //进入node回调
        private Action _m_onEnterDone;
        private Action _m_onQuitAction;
        private GDinnerInfo _m_dinnerInfo;

        public GNodeDinnerMain(GDinnerInfo _dinnerInfo, Action _onEnterDone = null, Action _onQuitAction = null)
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_DINNER_MAIN)
        {
            _m_dinnerInfo = _dinnerInfo;
            _m_onEnterDone = _onEnterDone;
            _m_onQuitAction = _onQuitAction;
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
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            GGUIAddSceneDinnerMain.instance.quitScene();
        }

        public override void EnterNode()
        {
        }
        
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_initDone);
            stepCounter.regAllDoneDelegate(_triggerEnterDone);
            GGUIAddSceneDinnerMain.instance.setInfo(_m_dinnerInfo);
            GUISceneMain.instance.showMainScene(GGUIAddSceneDinnerMain.instance, stepCounter.addDoneStepCount);
        }

        private void _initDone()
        {
            _m_onEnterDone?.Invoke();
            _m_onEnterDone = null;
        }

        public override void QuitNode()
        {
            GGUIAddSceneDinnerMain.instance.hideScene();
            _m_onQuitAction?.Invoke();
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
            
            QueueMgr.instance.forceCloseNode(this);
        }
    }
}
