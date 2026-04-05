using System;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 找东西小游戏
    /// </summary>
    public class GNodeMiniGameFindThings : _AMiniGameBaseNode
    {
        public static void enterGame([NotNull] MiniGameMainRefObj _miniGameMainRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop)
        {
            if (_miniGameMainRefObj.eGameType != EMiniGameType.FIND_THINGS)
            {
                Debug.LogError($"[GNodeMiniGameFindThings enterGame] 传入的配表数据_miniGameMainRefObj.eGameType:{_miniGameMainRefObj.eGameType}不是找东西小游戏:{EMiniGameType.FIND_THINGS}");
                _onGameStop?.Invoke(false);
                return;
            }
            
            FindThingsGameRefObj findThingsGameRefObj = GRefdataCoreMgr.instance.findThingsGameRefCore.getRef(_miniGameMainRefObj.gameSubId);
            if (findThingsGameRefObj == null)
            {
                Debug.LogError($"[GNodeMiniGameFindThings enterGame] 找不到_miniGameMainRefObj.gameSubId:{_miniGameMainRefObj.gameSubId}的findThingsGameRefObj配表数据");
                _onGameStop?.Invoke(false);
                return;
            }
            
            MiniGameMgr.exitGame();
            QueueMgr.instance.AddNode(new GNodeMiniGameFindThings(_miniGameMainRefObj, findThingsGameRefObj, _onGamePlayerDealGameDone, _onGameStop));
        }
        
        [NotNull] private FindThingsGameRefObj _m_rFindThingsGameRefObj;
        private bool _m_bIsSkipGame;//是否跳过游戏
        
        private Action _m_aOnGamePlayerDealGameDone;
        private Action<bool> _m_aOnGameStop;//游戏结束时
        
        public GNodeMiniGameFindThings([NotNull] MiniGameMainRefObj _miniGameMainRefObj, [NotNull] FindThingsGameRefObj _findThingsGameRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop) : base(_miniGameMainRefObj, UINodeTagConst.C_MINI_GAME_FIND_THINGS)
        {
            _m_rFindThingsGameRefObj = _findThingsGameRefObj;
            _m_aOnGamePlayerDealGameDone = _onGamePlayerDealGameDone;
            _m_aOnGameStop = _onGameStop;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }

        protected override _ANPBasicAddContainerUIScene _m_uiScene { get { return GMainGUIAddSceneMiniGame.instance; } }
        protected override _AMainAdditionMiniGameTDScene _m_tdScene { get { return null; } }

        protected override void onEnterQueueSub()
        {
            _m_bIsSkipGame = false;
            
            // 预加载小游戏资源
            GGUIWndFindThingsGame.instance.load(() =>
            {
                GGUIWndFindThingsGame.instance.preLoadFindThingsGamePrefab(_m_rFindThingsGameRefObj.ui_prefab_path, MiniGameMgr.exitGame, null);
            });
        }

        protected override void onCloseSub()
        {
            if(_m_bIsSkipGame)
                _onGamePlayerDealGameDone();
            
            Action<bool> onGameStop = _m_aOnGameStop;
            _m_aOnGameStop = null;
            onGameStop?.Invoke(GGUIWndFindThingsGame.instance.gameSuccess || _m_bIsSkipGame);
            
            GGUIWndFindThingsGame.instance.discradFindThingsGamePrefab();
            GGUIWndFindThingsGame.instance.discard();
            
            MiniGameMgr.exitGame();
        }

        protected override void onEnterNodeSub()
        {
            if (_m_uiScene != null)
            {
                _m_uiScene.regEnterDelegate(() =>
                {
                    _m_uiScene.showMainWnd(GGUIWndFindThingsGame.instance);
                });
            }
        }

        protected override void onQuitNodeSub()
        {
        }

        protected override void onMiniGameCommunalWndSkipBtnClick()
        {
            _m_bIsSkipGame = true;

            _onGamePlayerDealGameDone();
        }

        private void _onGamePlayerDealGameDone()
        {
            Action onGamePlayerDealGameDone = _m_aOnGamePlayerDealGameDone;
            _m_aOnGamePlayerDealGameDone = null;
            onGamePlayerDealGameDone?.Invoke();
        }
    }
}