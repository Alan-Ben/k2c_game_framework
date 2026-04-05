using System;
using System.Collections.Generic;
using GOE.EveningDungeon;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 晚间活动游戏Scene
    /// </summary>
    public class GMainGUIAddSceneEveningDungeonGame : _ANPGMainGUIAddSceneResBar<GGUIWndEveningDungeonGameMain>
    {
        [NotNull] public static GMainGUIAddSceneEveningDungeonGame instance { get { return _g_instance ??= new GMainGUIAddSceneEveningDungeonGame(); } }
        private static GMainGUIAddSceneEveningDungeonGame _g_instance;
        
        protected override GGUIWndEveningDungeonGameMain _m_wnd { get { return GGUIWndEveningDungeonGameMain.instance; } }
        
        public GGUISubWndEveningDungeonBoss getBossWnd { get { return GGUIWndEveningDungeonGameMain.instance.bossWnd; } }

        public EveningDungeonGameUIConfig gameUIConfig { get { return GGUIWndEveningDungeonGameMain.instance.gameUIConfig; } }
        
        public EveningDungeonHeroFightInfo curSelectHeroInfo { get { return GGUIWndEveningDungeonGameMain.instance.curSelectHeroInfo; } }

        public bool isAutoAttack { get { return GGUIWndEveningDungeonGameMain.instance.isAutoAttack; } }

        public void setGameController(EveningDungeonGameController _gameController)
        {
            GGUIWndEveningDungeonGameMain.instance.setGameController(_gameController);
        }

        public void getOtherPlayerAttackArea(List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _areaList)
        {
            GGUIWndEveningDungeonGameMain.instance.getOtherPlayerAttackArea(_areaList);
        }
        
        /// <summary>
        /// 刷新游戏状态显示
        /// </summary>
        /// <param name="_gameState"></param>
        public void refreshStateShow(EEveningDungeonGameState _gameState)
        {
            GGUIWndEveningDungeonGameMain.instance.refreshStateShow(_gameState);   
        }
        
        /// <summary>
        /// 刷新当前选中大臣
        /// </summary>
        public void refreshCurSelectHero(Action _onChgDone)
        {
            GGUIWndEveningDungeonGameMain.instance.refreshCurSelectHero(_onChgDone);
        }
        
        /// <summary>
        /// 停止自动攻击
        /// </summary>
        public void stopAutoAttack()
        {
            GGUIWndEveningDungeonGameMain.instance.stopAutoAttack();
        }

        /// <summary>
        /// 飞船攻击表现
        /// </summary>
        /// <param name="_showDone"></param>
        public void showAirshipAttack(Action _showDone)
        {
            GGUIWndEveningDungeonGameMain.instance.showAirshipAttack(_showDone);
        }

        /// <summary>
        /// 飞船离开表现
        /// </summary>
        /// <param name="_showDone"></param>
        public void showAirshipDeparture(Action _showDone)
        {
            GGUIWndEveningDungeonGameMain.instance.showAirshipDeparture(_showDone);
        }
    }
}