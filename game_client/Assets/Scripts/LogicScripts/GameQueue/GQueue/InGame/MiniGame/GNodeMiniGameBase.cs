using System;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    internal class GNodeMiniGameBase : _AMiniGameBaseNode
    {
        public static void enterGame(long _miniGameMainId, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop)
        {
            MiniGameMainRefObj miniGameMainRefObj = GRefdataCoreMgr.instance.miniGameMainRefCore.getRef(_miniGameMainId);
            if (miniGameMainRefObj == null)
            {
                Debug.LogError($"[GNodeMiniGame enterGame] 找不到miniGameMainId:{_miniGameMainId}对应的配表数据");
                _onGameStop?.Invoke(false);
                return;
            }

            enterGame(miniGameMainRefObj, _onGamePlayerDealGameDone, _onGameStop);
        }
        
        public static void enterGame([NotNull] MiniGameMainRefObj _miniGameMainRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop)
        {
            MiniGameMgr.exitGame();
            QueueMgr.instance.AddNode(new GNodeMiniGameBase(_miniGameMainRefObj, _onGamePlayerDealGameDone, _onGameStop));
        }
        
        private _AMiniGameLogic _getGameLogic(EMiniGameType _miniGameType)
        {
            switch (_miniGameType)
            {
                case EMiniGameType.PUZZLE:
                    return PuzzleGameLogic.instance;

                case EMiniGameType.TAKE_THINGS_SEQUENTIALLY:
                    return TakeThingsSequentiallyGameLogic.instance;
                
                case EMiniGameType.QTE_CLICK_OPPORTUNITY_GAME:
                    return QteClickOpportunityGameLogic.instance;

                case EMiniGameType.DRAG_BOX:
                    return DragBoxGameLogic.instance;
                
                default:
                    Debug.LogError($"[getGameLogic] 找不到小游戏:{_miniGameType}对应的GameLogic");
                    return null;
            }
        }

        private _AMiniGameLogic _m_aGameLogic;
        
        private Action _m_aOnGamePlayerDealGameDone;
        private Action<bool> _m_aOnGameStop;
        
        private GNodeMiniGameBase([NotNull] MiniGameMainRefObj _miniGameMainRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop) : base(_miniGameMainRefObj, UINodeTagConst.C_MINI_GAME_BASE)
        {
            _m_aOnGamePlayerDealGameDone = _onGamePlayerDealGameDone;
            _m_aOnGameStop = _onGameStop;
            
            _m_aGameLogic = _getGameLogic(_m_rMiniGameMainRefObj?.eGameType ?? EMiniGameType.NONE);
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }

        protected override _ANPBasicAddContainerUIScene _m_uiScene { get { return _m_aGameLogic?.uiScene; } }
        protected override _AMainAdditionMiniGameTDScene _m_tdScene { get { return _m_aGameLogic?.tdScene; } }

        protected override void onCloseSub()
        {
            _m_aGameLogic?.stop();
            
            MiniGameMgr.exitGame();
        }

        /// <summary>
        /// 在自身doEnterNode加载逻辑 和 子类doEnterNodeSub加载逻辑完成后 调用基类_triggerEnterDone前的最后一步, 执行到这个方法时uiScene和tdScene已经处于Entered状态
        /// </summary>
        /// <param name="_triggerEnterDone"></param>
        protected override void doEnterNodeLastStep(Action _triggerEnterDone)
        {
            Action _onGameLogicStart = () =>
            {
                base.doEnterNodeLastStep(_triggerEnterDone);
            };
            
            if (_m_aGameLogic == null)
            {
                Debug.LogError($"[GNodeMiniGameBase doEnterNodeLastStep] 游戏:{_m_rMiniGameMainRefObj?.eGameType} 的_m_aGameLogic为空");
                _onGameLogicStart();
            }
            else if(!_m_aGameLogic.isStart)
            {
                _m_aGameLogic.start(_m_rMiniGameMainRefObj, _m_aOnGamePlayerDealGameDone, _m_aOnGameStop, _onGameLogicStart, _onGameLogicStart);
            }
            else if(_m_aGameLogic.isPause)
            {
                _m_aGameLogic.resumeGame();
                _onGameLogicStart();
            }
            else
            {
                _onGameLogicStart();
            }
        }

        protected override void onEnterNodeSub()
        {
        }

        protected override void onQuitNodeSub()
        {
            _m_aGameLogic?.pauseGame();//暂停游戏逻辑
        }

        protected override void onMiniGameCommunalWndSkipBtnClick()
        {
            _m_aGameLogic?.setGameSuccess(true);
        }
    }
}