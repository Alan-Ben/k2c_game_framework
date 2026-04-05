
using System;
using ALPackage;
using ClientEnum;

namespace GOE
{
    public class GNodeTowerBattle : BaseQueueNode
    {
        //回退按钮
        private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;
        private TowerChallengeResult _m_result;

        public GNodeTowerBattle(TowerChallengeResult _result)
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TOWER_BATTLE)
        {
            _m_result = _result;
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
        }
        
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_initDone);
            stepCounter.regAllDoneDelegate(_triggerEnterDone);
            GTDSceneMain.instance.showMainScene(MainAdditionTowerBattleTDScene.instance, stepCounter.addDoneStepCount);//进入主界面3D场景
            GUISceneMain.instance.showMainScene(GGUIAddSceneTowerBattle.instance, stepCounter.addDoneStepCount);
        }
        
        private void _initDone()
        {
            MainAdditionTowerBattleTDScene.instance.setInfo(_m_result);
            GGUIAddSceneTowerBattle.instance.setInfo(_m_result);
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
