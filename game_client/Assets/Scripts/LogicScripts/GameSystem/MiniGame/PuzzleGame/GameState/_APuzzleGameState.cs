using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _APuzzleGameState : _ABaseGameState<EPuzzleGameState, PuzzleGameLogic>
    {
        protected _APuzzleGameState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameState] 进入:{state}状态");
            }

            _onEnterSub();
        }

        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameState] 退出:{state}状态");
            }

            _onExitSub();
        }

        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}