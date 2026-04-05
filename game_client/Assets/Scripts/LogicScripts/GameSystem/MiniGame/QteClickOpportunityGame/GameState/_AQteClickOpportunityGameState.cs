using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _AQteClickOpportunityGameState : _ABaseGameState<EQteClickOpportunityGameState, QteClickOpportunityGameLogic>
    {
        public _AQteClickOpportunityGameState([NotNull] QteClickOpportunityGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_AQteClickOpportunityGameState] 进入:{state}状态");
            }

            _m_gameLogic.qteClickOpportunityGameUnit.setGameState(state);
            _onEnterSub();
        }
        
        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_AQteClickOpportunityGameState] 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}