using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _APuzzleGameMatchItemBgState : _ASimpleState<EPuzzleGameMatchItemBgState>
    {
        [NotNull] protected PuzzleGameMatchItemBgUnit _m_matchItemBgUnit;
        public _APuzzleGameMatchItemBgState([NotNull] PuzzleGameMatchItemBgUnit _matchItemBgUnit)
        {
            _m_matchItemBgUnit = _matchItemBgUnit;
        }

        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameMatchItemBgState] _m_matchItemBgUnit:{_m_matchItemBgUnit.matchItemId} 进入:{state}状态");
            }

            _onEnterSub();
        }

        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameMatchItemBgState] _m_matchItemBgUnit:{_m_matchItemBgUnit.matchItemId} 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}