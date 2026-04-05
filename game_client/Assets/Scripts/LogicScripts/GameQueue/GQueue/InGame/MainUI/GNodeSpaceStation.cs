using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 空间站node
    /// </summary>
    public class GNodeSpaceStation : _AGNodeMainSub
    {
        public GNodeSpaceStation() : base(EMainFunctionTabType.SPACE_STATION, UINodeTagConst.C_NODE_SPACE_STATION)
        {
        }

        /// <summary>
        /// 允许弹出的提示窗口类型，默认都不弹
        /// </summary>
        public override ENoticeType enableNoticeType { get { return ENoticeType.SPACE_STATION; } }

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
        
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(()=>
            {
                MainAdditionSpaceStationTDScene.instance.showGos();
                _triggerEnterDone?.Invoke();
            });

            //如果需要reload先进一个空场景
            if (MainAdditionSpaceStationTDScene.instance.checkNeedReload())
            {
                GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);
            }

            GTDSceneMain.instance.showMainScene(MainAdditionSpaceStationTDScene.instance, stepCounter.addDoneStepCount);
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneSpaceStation.instance, stepCounter.addDoneStepCount);
        }

        protected override void _doQuitNode()
        {
            MainAdditionSpaceStationTDScene.instance.hideGos();
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}