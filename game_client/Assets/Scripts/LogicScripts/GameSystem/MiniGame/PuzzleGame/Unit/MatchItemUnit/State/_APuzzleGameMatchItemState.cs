using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _APuzzleGameMatchItemState : _ASimpleState<EPuzzleGameMatchItemState>
    {
        [NotNull] protected PuzzleGameMatchItemUnit _m_matchItemUnit;
        public _APuzzleGameMatchItemState([NotNull] PuzzleGameMatchItemUnit _matchItemUnit)
        {
            _m_matchItemUnit = _matchItemUnit;
        }
        
        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"[PuzzleGameMatchItemState] _m_matchItemBgUnit:{_m_matchItemUnit.matchItemId} 进入:{state}状态");
            }

            _onEnterSub();
        }

        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"[PuzzleGameMatchItemState] _m_matchItemBgUnit:{_m_matchItemUnit.matchItemId} 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
    
    public abstract class _APuzzleGameMatchItemState<T_Param> : _ASimpleState<EPuzzleGameMatchItemState, T_Param>
    {
        [NotNull] protected PuzzleGameMatchItemUnit _m_matchItemUnit;
        public _APuzzleGameMatchItemState([NotNull] PuzzleGameMatchItemUnit _matchItemUnit)
        {
            _m_matchItemUnit = _matchItemUnit;
        }
        
        protected override void _onEnter(T_Param _param)
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameMatchItemState] _m_matchItemUnit:{_m_matchItemUnit.matchItemId} 进入:{state}状态");
            }

            _onEnterSub(_param);
        }

        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[PuzzleGameMatchItemState] _m_matchItemUnit:{_m_matchItemUnit.matchItemId} 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub(T_Param _param);
        
        protected abstract void _onExitSub();
    }
}