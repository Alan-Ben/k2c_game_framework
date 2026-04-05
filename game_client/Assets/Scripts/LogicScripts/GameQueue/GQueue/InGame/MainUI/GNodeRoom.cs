using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 卧室节点
    /// </summary>
    public class GNodeRoom : BaseQueueNode
    {
        /// <summary>
        /// 完成之后的回调
        /// </summary>
        private Action _m_dEnteredAction;
        
        public GNodeRoom(Action _action = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_NODE_ROOM)
        {
            _m_dEnteredAction = _action;
        }
        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }

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

        public override void QuitNode()
        {
            if (null != _m_dEnteredAction)
                _m_dEnteredAction();
            _m_dEnteredAction = null;
        }

        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            //如果场景都没初始化过，并且不需要重新加载，就加个云先吧
            // if (MainAdditionRoomTDScene.instance.isInited && !MainAdditionRoomTDScene.instance.checkNeedReload())
            // {
                _enterCityScene(() =>
                {
                    _triggerEnterDone?.Invoke();
                });
            // }
            // else
            // {
            //     Loading.showLoading(_complete =>
            //     {
            //         _enterCityScene(() =>
            //         {
            //             _complete?.Invoke();
            //                     
            //             _triggerEnterDone?.Invoke();
            //         });
            //     });   
            // }
        }

        //进入乐园场景的操作
        private void _enterCityScene(Action _doneAction = null)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_onAllSceneInited);
            stepCounter.regAllDoneDelegate(_doneAction);

            //如果需要reload先进一个空场景
            if (MainAdditionRoomTDScene.instance.checkNeedReload())
            {
                GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);
                GMainGUIAddSceneRoom.instance.setTDSceneLoading(true);
            }

            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(() =>
            {
                GTDSceneMain.instance.showMainScene(MainAdditionRoomTDScene.instance, ()=>
                {
                    GMainGUIAddSceneRoom.instance.setTDSceneLoading(false);
                    stepCounter.addDoneStepCount();
                });
                GUISceneMain.instance.showMainScene(GMainGUIAddSceneRoom.instance, stepCounter.addDoneStepCount);
            });
        }
        

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
            if (null != _m_dEnteredAction)
                _m_dEnteredAction();
            _m_dEnteredAction = null;
            
            //弹出需要在主城或者卧室弹出的弹出，这个时候保证ui跟td都已经完全显示了,并且云也完全散开了
            // 不在这里检测原因是 同时添加多个MainUI Notice后, 每有一个Notice展示完成, 都会重新EnterNode, 导致重新检测一遍, 会重复添加Notice
            // Loading.regHideDoneDelegate(() =>
            // {
            //     GCommon.popCityAndRoomWnd();
            // });
        }

        public override void onCannotEscBack()
        {
        }
    }
}