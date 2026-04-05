using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.EveningDungeon
{
    public abstract class _AEveningDungeonGameState : _ASimpleState<EEveningDungeonGameState>
    {
        [NotNull] protected EveningDungeonGameLogic _m_gameLogic;
        
        public _AEveningDungeonGameState(EveningDungeonGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_AEveningDungeonGameState] 进入:{state}状态");
            }

            _onEnterSub();
        }
        
        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_AEveningDungeonGameState] 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}