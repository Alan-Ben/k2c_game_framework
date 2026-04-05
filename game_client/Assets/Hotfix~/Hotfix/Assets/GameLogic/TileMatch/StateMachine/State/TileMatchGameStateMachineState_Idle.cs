using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Idle : _ATileMatchGameStateMachineBaseState
        {
            private float _m_fEnterTipStateTime;//进入提示状态的时间
            
            public TileMatchGameStateMachineState_Idle([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.Idle; } }

            protected override void _onEnter()
            {
                _m_fEnterTipStateTime = Time.realtimeSinceStartup + _m_gameLogic._m_gameConfig.idleToTipTime;
            }

            protected override void _onExit()
            {
            }

            protected override void _onTick(float _deltaTime)
            {
                if (_m_fEnterTipStateTime < Time.realtimeSinceStartup ||
                    (_m_gameLogic._m_gameConfig.errorSwitchShowTipCount > 0 && _m_gameLogic._m_errorSwitchCount >= _m_gameLogic._m_gameConfig.errorSwitchShowTipCount))
                {
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Tips>();
                    _m_fEnterTipStateTime = Time.realtimeSinceStartup + _m_gameLogic._m_gameConfig.idleToTipTime;
                }
            }

            public override bool canEnterState(_ATHotfixStateBase<ETileMatchGameState> _newState)
            {
                return false;
            }

            public override void resetData()
            {
            }
            
            public override void doPress(TileMatchBlockInfo _cubeInfo)
            {
            }
        }
    }
}