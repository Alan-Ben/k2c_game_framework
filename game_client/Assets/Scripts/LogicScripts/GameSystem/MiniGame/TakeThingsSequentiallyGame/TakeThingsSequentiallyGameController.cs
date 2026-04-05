using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameController : _AMiniGameController
    {
        [NotNull] private TakeThingsSequentiallyGameLogic _m_gameLogic;
        public TakeThingsSequentiallyGameController([NotNull] TakeThingsSequentiallyGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }

        /// <summary>
        /// 取走物品
        /// </summary>
        /// <param name="_thingId"></param>
        internal void takeThing(long _thingId)
        {
            _m_gameLogic.takeThing(_thingId);
        }
    }
}