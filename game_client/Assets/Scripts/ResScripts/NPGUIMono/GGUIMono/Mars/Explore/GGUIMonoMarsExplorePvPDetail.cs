using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExplorePvPDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("战斗日期")]
        public Text txtTime;
        [ALHeader("双方头像")]
        public RawImage imgMyAvatar;
        public RawImage imgEnemyAvatar;
        [ALHeader("双方玩家信息")]
        public NPGGUIMonoPlayerIcon monoMyPlayerInfo;
        public NPGGUIMonoPlayerIcon monoEnemyPlayerInfo;
        [ALHeader("双方名字")]
        public Text txtMyName;
        public Text txtEnemyName;
        [ALHeader("双方队伍实力损耗")]
        public Text txtMyPowerLoss;
        public Text txtEnemyPowerLoss;
        [ALHeader("双方总兵力")]
        public Text txtMyTotalTroops;
        public Text txtEnemyTotalTroops;
        [ALHeader("双方损失值")]
        public Text txtMyLossTroops;
        public Text txtEnemyLossTroops;
        [ALHeader("双方生还值")]
        public Text txtMySurvivorTroops;
        public Text txtEnemySurvivorTroops;
        [ALHeader("胜利或失败是显示的内容")]
        public List<GameObject> listWinShow;
        public List<GameObject> listLoseShow;
        [ALHeader("战报翻页按钮")]
        public GameObject btnNextLog;
        public GameObject btnPrevLog;
        public List<GameObject> listHasNextLogShow;
        public List<GameObject> listHasPrevLogShow;
        [ALHeader("数据加载中显示的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        [ALHeader("对方是玩家还是 npc 显示的内容")]
        public List<GameObject> listEnemyIsPlayerShow;
        public List<GameObject> listEnemyIsNPCShow;
        
        
        public void setIsWin(bool _isWin)
        {
            ALUGUICommon.setGameObjEnable(listWinShow, false);
            ALUGUICommon.setGameObjEnable(listLoseShow, false);
            ALUGUICommon.setGameObjEnable(_isWin ? listWinShow : listLoseShow, true);
        }
        public void setHasNextLog(bool _hasNext)
        {
            ALUGUICommon.setGameObjEnable(listHasNextLogShow, _hasNext);
        }
        public void setHasPrevLog(bool _hasPrev)
        {
            ALUGUICommon.setGameObjEnable(listHasPrevLogShow, _hasPrev);
        }
        public void setLoadingState(bool _isLoading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isLoading ? listLoadingShow : listLoadingHide, true);
        }
        public void setEnemyIsPlayer(bool _isPlayer)
        {
            ALUGUICommon.setGameObjEnable(listEnemyIsPlayerShow, false);
            ALUGUICommon.setGameObjEnable(listEnemyIsNPCShow, false);
            ALUGUICommon.setGameObjEnable(_isPlayer ? listEnemyIsPlayerShow : listEnemyIsNPCShow, true);
        }
        
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7417); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7417); } }
    }
}