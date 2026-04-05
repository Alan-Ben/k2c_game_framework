using System;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public partial class TileMatchGameStateMachine
        {
            public class TileMatchGameStateMachineStateFactory : _ATHotfixStateFactory<_ATileMatchGameStateMachineBaseState, ETileMatchGameState>
            {
                public TileMatchGameStateMachineStateFactory([NotNull] TileMatchGameLogic _gameLogic)
                {
                    regCacheController(typeof(TileMatchGameStateMachineState_None), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_None(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Init), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Init(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Idle), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Idle(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Reorder), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Reorder(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Switch), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Switch(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Tips), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Tips(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_Died), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_Died(_gameLogic); }));
                    regCacheController(typeof(TileMatchGameStateMachineState_DealServer), new TileMatchGameStateMachineStateCache(() => { return new TileMatchGameStateMachineState_DealServer(_gameLogic); }));
                }

                /// <summary>
                /// 视频相关状态的缓存对象，一般播放视频不会过多，所以最高缓存使用8个
                /// </summary>
                protected class TileMatchGameStateMachineStateCache : _THotfixBasicStateCacheController<_ATileMatchGameStateMachineBaseState, ETileMatchGameState>
                {
                    public TileMatchGameStateMachineStateCache(Func<_ATileMatchGameStateMachineBaseState> _createFunc) : base(_createFunc, 1, 8)
                    {

                    }
                }
            }
        }
    }
}