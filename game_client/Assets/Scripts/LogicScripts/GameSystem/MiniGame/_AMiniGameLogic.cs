using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _AMiniGameLogic : _AGameLogic
    {
        private Action _m_aOnGamePlayerDealGameDone;
        private Action<bool> _m_aOnGameStop;//游戏结束回调

        protected MiniGameMainRefObj _m_rMiniGameMainRefObj;//小游戏主表数据
        protected bool _m_bIsSuccess;//游戏是否成功
        protected long _m_lGameSerializeId;//游戏唯一序列号

        protected _AMiniGameLogic()
        {
        }
        
        public long subGameId { get { return _m_rMiniGameMainRefObj?.gameSubId ?? 0; } }
        internal bool gameSuccess { get { return _m_bIsSuccess; } }
        internal long gameSerializeId { get { return _m_lGameSerializeId; } }

        #region 子类重写属性

        /// <summary>
        /// 小游戏类型
        /// </summary>
        public abstract EMiniGameType eMiniGameType { get; }
        
        /// <summary>
        /// 控制器
        /// </summary>
        public abstract _AMiniGameController controller { get; }

        public abstract _ANPBasicAddContainerUIScene uiScene { get; }
        public abstract _AMainAdditionMiniGameTDScene tdScene { get; }

        #endregion
        
        public void start(MiniGameMainRefObj _miniGameMainRefObj, Action _onGamePlayerDealGameDone, Action<bool> _onGameStop, Action _onComplete, Action _onFailed)
        {
            _m_rMiniGameMainRefObj = _miniGameMainRefObj;
            _m_aOnGamePlayerDealGameDone = _onGamePlayerDealGameDone;
            _m_aOnGameStop = _onGameStop;
            _m_bIsSuccess = false;
            
            base.start(_onComplete, _onFailed);
        }

        protected override void _startGameOp(Action _complete, Action _failed)
        {
            if (_m_rMiniGameMainRefObj == null)
            {
                Debug.LogError("[_AMiniGameLogic _startGameOp] GameLogic 启动失败 _m_rMiniGameMainRefObj is null");
                _failed?.Invoke();
                return;
            }

            if (_m_rMiniGameMainRefObj.eGameType != eMiniGameType)
            {
                Debug.LogError($"[_AMiniGameLogic _startGameOp] GameLogic 启动失败 _m_rMiniGameMainRefObj.eGameType:{_m_rMiniGameMainRefObj.eGameType} != eMiniGameType:{eMiniGameType}");
                _failed?.Invoke();
                return;
            }
            
            _startGameOpSub(_complete, _failed);
        }

        protected override void _onStart()
        {
            _m_lGameSerializeId = ALSerializeOpMgr.next();
            
            _onStartSub();
        }

        protected override void _onStop()
        {
            _onStopSub();

            setGameSuccess(_m_bIsSuccess);
            Action<bool> onGameStop = _m_aOnGameStop;
            _m_aOnGameStop = null;
            onGameStop?.Invoke(_m_bIsSuccess);

            _m_rMiniGameMainRefObj = null;
            
            _m_lGameSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onTick(float _deltaTime)
        {
            _onTickSub(_deltaTime);
        }

        #region 子类继承方法

        protected abstract void _startGameOpSub(Action _complete, Action _failed);
        
        protected abstract void _onStartSub();
        
        protected abstract void _onStopSub();

        protected abstract void _onTickSub(float _deltaTime);

        #endregion
        
        internal void setGameSuccess(bool _isSuccess)
        {
            if(_m_bIsSuccess == _isSuccess)
                return;
            
            _m_bIsSuccess = _isSuccess;

            if (_m_bIsSuccess)
            {
                Action onGamePlayerDealGameDone = _m_aOnGamePlayerDealGameDone;
                _m_aOnGamePlayerDealGameDone = null;
                onGamePlayerDealGameDone?.Invoke();
            }
        }
    }
}