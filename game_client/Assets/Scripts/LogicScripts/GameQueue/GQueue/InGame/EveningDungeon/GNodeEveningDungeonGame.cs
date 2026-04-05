using System;
using ALPackage;
using GOE.EveningDungeon;

namespace GOE
{
    public class GNodeEveningDungeonGame : BaseQueueNode
    {
        public GNodeEveningDungeonGame() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_EVENING_DUNGEON_GAME_MAIN)
        {
        }

        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
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
        
        public override void onEnterQueue()
        {
            // 这里先加载是为了将scene的实际quitScene权限增加onClose时的控制, 即只有当onClose时, scene才会被真正销毁
            GMainGUIAddSceneEveningDungeonGame.instance.enterScene();
            
            WinMsg.RegisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        public override void onClose()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);

            EveningDungeonGameLogic.instance.stop();
            GMainGUIAddSceneEveningDungeonGame.instance.quitScene();
        }

        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (!EveningDungeonGameLogic.instance.isStart)
                {
                    EveningDungeonGameLogic.instance.start(_triggerEnterDone, _triggerEnterDone);
                }
                else if(EveningDungeonGameLogic.instance.isPause)
                {
                    EveningDungeonGameLogic.instance.resumeGame();
                    _triggerEnterDone?.Invoke();
                }
                else
                {
                    _triggerEnterDone?.Invoke();
                }
            });
            
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(() =>
            {
                stepCounter.chgTotalStepCount(1);
                GUISceneMain.instance.showMainScene(GMainGUIAddSceneEveningDungeonGame.instance, stepCounter.addDoneStepCount);
            });
            
            stepCounter.addDoneStepCount();
        }

        public override void EnterNode()
        {
            
        }

        public override void QuitNode()
        {
            EveningDungeonGameLogic.instance.pauseGame();
        }

        private void _quitAllGameNode()
        {
            QueueMgr.instance.QuitUntilCanStop((_node) =>
            {
                if(_node != null && _node.nodeTag == UINodeTagConst.C_EVENING_DUNGEON_GAME_MAIN)
                    return true;

                return false;
            });
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_GAME_MAIN);
        }
        
        private void _onActivityStateChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 2 || !(_objs[1] is EEveningDungeonActivityState _curState))
                return;

            if (_curState != EEveningDungeonActivityState.ONGOING)
            {
                // NPMesMgr.instance.showOneBtnMes(
                //     TextTranslate.instance.getLanguage(TransKeyConst.rankRush_activityAlreadyClose_none), TransKeyConst.confirm,
                //     () =>
                //     {
                //         _quitAllGameNode();
                //     });
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.rankRush_activityAlreadyClose_none);
                _quitAllGameNode();
            }
        }
    }
}