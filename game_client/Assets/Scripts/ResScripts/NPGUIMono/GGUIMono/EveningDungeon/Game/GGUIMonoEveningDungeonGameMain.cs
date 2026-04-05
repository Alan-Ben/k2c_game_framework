using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 晚间活动游戏配置
    /// </summary>
    [Serializable]
    public class EveningDungeonGameUIConfig
    {
        [ALHeader("攻击后, 切换大臣的延迟时间(秒)")]
        public float afterAttackChgHeroDelayS;
        
        [ALHeader("更新boss信息的时间间隔(秒)")]
        public float updateBossInfoTimeInterval = 0.1f;
        [ALHeader("刷新其他玩家攻击log的时间间隔(秒)")]
        public float refreshOtherPlayerAttackLogTimeInterval = 0.1f;
        
        // [ALHeader("自己攻击时显示结果窗口的延迟时间(秒)")]
        // public float selfAttackShowResultWndDelayTimeS;
        
        [ALHeader("在自动攻击时, 结果窗口显示的时间(秒)")]
        public float onAutoAttackResultWndShowTimeS;
    }
    
    /// <summary>
    /// 晚间活动游戏窗口
    /// </summary>
    public class GGUIMonoEveningDungeonGameMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("游戏状态显隐配置")]
        public List<NPCommonEnumAniStatInfo<EEveningDungeonGameState>> stateShowList;

        [ALHeader("游戏UI配置")]
        public EveningDungeonGameUIConfig gameUIConfig;
        
        [ALHeader("boss子窗口")]
        public GGUISubMonoEveningDungeonBoss monoBoss;

        [ALHeader("排行小窗口")]
        public GGUIMonoEveningDungeonRankMini monoRankMini;

        [ALHeader("其他玩家攻击区域列表")]
        public List<GGUISubMonoEveningDungeonOtherPlayerAttackArea> monoOtherPlayerAttackAreaList;
        
        [ALHeader("选中大臣头像信息")] 
        public GGUIMonoHeroIconItem monoSelectHeroHead;
        [ALHeader("选中大臣攻击力")]
        public TextEx txtSelectHeroATK;
        [ALHeader("选中大臣攻击力Key(一个参数, 大臣攻击力)")]
        public string txtSelectHeroATKKey;
        
        [ALHeader("有选中大臣的显示GO列表")]
        public List<GameObject> hasSelectHeroShowList;
        [ALHeader("没有选中大臣的显示GO列表")]
        public List<GameObject> noSelectHeroShowList;
        
        [ALHeader("打开选择大臣窗口按钮")]
        public GameObject btnOpenSelectHeroWnd;

        [ALHeader("飞船展示")]
        public GGUIMonoEveningDungeonGameAirshipShow monoAirshipShow;
        
        [ALHeader("尾刀记录按钮")]
        public GameObject btnFinalAttackRecord;
        
        [ALHeader("战斗按钮")]
        public GameObject btnFight;
        [ALHeader("自动战斗按钮")]
        public NPGGUIMonoCommonToggleEx autoAttackToggle;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5500); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5500);} }
    }
}