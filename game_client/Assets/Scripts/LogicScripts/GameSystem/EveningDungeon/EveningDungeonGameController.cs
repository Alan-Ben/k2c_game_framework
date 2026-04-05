using JetBrains.Annotations;

namespace GOE.EveningDungeon
{
    /// <summary>
    /// 晚间活动游戏控制器
    /// </summary>
    public class EveningDungeonGameController
    {
        [NotNull] private EveningDungeonGameLogic _m_gameLogic;

        public EveningDungeonGameController ([NotNull] EveningDungeonGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }

        public void attack()
        {
            _m_gameLogic.attack();
        }
    }
}