using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _ATakeThingsSequentiallyGameState : _ABaseGameState<ETakeThingsSequentiallyGameState, TakeThingsSequentiallyGameLogic>
    {
        public _ATakeThingsSequentiallyGameState([NotNull] TakeThingsSequentiallyGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_ATakeThingsSequentiallyGameState] 进入:{state}状态");
            }

            _m_gameLogic.sequentiallyGameUnit.setGameState(state);
            _onEnterSub();
        }
        
        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_ATakeThingsSequentiallyGameState] 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}