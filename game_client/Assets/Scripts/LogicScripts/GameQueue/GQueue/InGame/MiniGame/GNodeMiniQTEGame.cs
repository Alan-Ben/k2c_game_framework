using System;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 引导小游戏
    /// </summary>
    public class GNodeMiniQTEGame : _AMiniGameBaseNode
    {
        public static void enterGame([NotNull] MiniGameMainRefObj _miniGameMainRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop)
        {
            if (_miniGameMainRefObj.eGameType != EMiniGameType.QTE_GAME)
            {
                Debug.LogError($"[GNodeMiniQTEGame enterGame] 传入的配表数据_miniGameMainRefObj.eGameType:{_miniGameMainRefObj.eGameType}不是QTE小游戏:{EMiniGameType.QTE_GAME}");
                _onGameStop?.Invoke(false);
                return;
            }
            
            QTEGameRefObj qteGameRefObj = GRefdataCoreMgr.instance.qteGameRefCore.getRef(_miniGameMainRefObj.gameSubId);
            if (qteGameRefObj == null)
            {
                Debug.LogError($"[GNodeMiniQTEGame enterGame] 找不到_miniGameMainRefObj.gameSubId:{_miniGameMainRefObj.gameSubId}的QTEGameRefObj配表数据");
                _onGameStop?.Invoke(false);
                return;
            }
            
            MiniGameMgr.exitGame();
            QueueMgr.instance.AddNode(new GNodeMiniQTEGame(_miniGameMainRefObj, qteGameRefObj, _onGamePlayerDealGameDone, _onGameStop));
        }
        
        [NotNull] private QTEGameRefObj _m_rQteGameRefObj;
        private bool _m_bIsSkipGame;//是否跳过游戏
        
        private Action _m_aOnGamePlayerDealGameDone;
        private Action<bool> _m_aOnGameStop;//游戏结束时
        
        private GGUIWndMiniQTEGame _m_wQteGameWnd;
        
        public GNodeMiniQTEGame([NotNull] MiniGameMainRefObj _miniGameMainRefObj, [NotNull] QTEGameRefObj _qteGameRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop) : base(_miniGameMainRefObj, UINodeTagConst.C_MINI_GAME_QTE_GAME)
        {
            _m_rQteGameRefObj = _qteGameRefObj;
            _m_aOnGamePlayerDealGameDone = _onGamePlayerDealGameDone;
            _m_aOnGameStop = _onGameStop;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        
        protected override _ANPBasicAddContainerUIScene _m_uiScene { get { return null; } }
        protected override _AMainAdditionMiniGameTDScene _m_tdScene { get { return null; } }

        protected override void onEnterQueueSub()
        {
            _m_bIsSkipGame = false;
        }

        protected override void onCloseSub()
        {
            if (_m_bIsSkipGame)
                _onGamePlayerDealGameDone();
            
            Action<bool> onGameStop = _m_aOnGameStop;
            _m_aOnGameStop = null;
            onGameStop?.Invoke((_m_wQteGameWnd?.gameEnd ?? true) || _m_bIsSkipGame);
            
            _m_wQteGameWnd?.discard();
            _m_wQteGameWnd = null;
            
            MiniGameMgr.exitGame();
        }

        protected override void doEnterNodeSub(Action _triggerSubEnterDone)
        {
            if (_m_wQteGameWnd == null)
            {
                _m_wQteGameWnd = new GGUIWndMiniQTEGame(_m_rQteGameRefObj, onMiniGameCommunalWndSkipBtnClick);
                _m_wQteGameWnd.load();
            }
            
            _m_wQteGameWnd.regLoadDoneDelegate(()=>
            {
                _m_wQteGameWnd?.showWnd();
                
                _triggerSubEnterDone?.Invoke();
            });
        }

        protected override void onEnterNodeSub()
        {
        }

        /// <summary>
        /// QuitNode时不调用hide, 这样可以实现在QTE游戏页面中中前往其他页面进行操作, 完成游戏
        /// </summary>
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