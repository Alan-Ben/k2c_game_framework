using System;
using ALPackage;

namespace GOE
{
    public class MiniGameMgr
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_miniGameMainId"></param>
        /// <param name="_onGamePlayerDealGameDone">通过玩家手动操作游戏成功时回调</param>
        /// <param name="_onGameStop">游戏逻辑停止时调用</param>
        public static void enterGame(long _miniGameMainId, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop)
        {
            //发送埋点-进入小游戏
            GCommon.sendStepReport(TraceConst.ENTER_MINI_GAME.setMarkParam(_miniGameMainId));

            MiniGameMainRefObj miniGameMainRefObj = GRefdataCoreMgr.instance.miniGameMainRefCore.getRef(_miniGameMainId);
            if (miniGameMainRefObj == null)
            {
                Debug.LogError($"[GNodeMiniGame enterGame] 找不到miniGameMainId:{_miniGameMainId}对应的MiniGameMainRefObj配表数据");
                return;
            }

            _onGameStop += (_isSuccess) =>
            {
                //发送埋点-完成小游戏
                GCommon.sendStepReport(TraceConst.FINISH_MINI_GAME.setMarkParam(_isSuccess,_miniGameMainId));

                //直接发送消息
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.MINI_GAME_END);
                
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"[MiniGameMgr enterGame] {miniGameMainRefObj.eGameType}小游戏结束 是否成功:{_isSuccess}");
            };
            
            exitGame();
            switch (miniGameMainRefObj.eGameType)
            {
                case EMiniGameType.PUZZLE:
                case EMiniGameType.TAKE_THINGS_SEQUENTIALLY:
                case EMiniGameType.QTE_CLICK_OPPORTUNITY_GAME:
                case EMiniGameType.DRAG_BOX:
                    GNodeMiniGameBase.enterGame(miniGameMainRefObj, _onGamePlayerDealGameDone, _onGameStop);
                    break;
                
                case EMiniGameType.FIND_THINGS:
                    GNodeMiniGameFindThings.enterGame(miniGameMainRefObj, _onGamePlayerDealGameDone, _onGameStop);
                    break;  
                
                case EMiniGameType.QTE_GAME:
                    GNodeMiniQTEGame.enterGame(miniGameMainRefObj, _onGamePlayerDealGameDone, _onGameStop);
                    break;
                
                default:
                    Debug.LogError($"[MiniGameMgr enterGame] 找不到小游戏类型:{miniGameMainRefObj.eGameType} 对应的进入游戏方法");
                    _onGameStop?.Invoke(false);
                    break;
            }
        }

        public static void exitGame()
        {
            QueueMgr.instance.forceCloseNode((_node) =>
            {
                return _node is _AMiniGameBaseNode;
            });
            // QueueMgr.instance.quitStage((int)EUIQueueStageType.MINI_GAME);
        }
    }
}