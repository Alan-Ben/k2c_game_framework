using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamEditItemContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("队伍序号")]
        public Text txtNum;
        [ALHeader("队伍名称")]
        public Text txtName;
        [ALHeader("带兵量")]
        public Text txtSoldierNum;
        [ALHeader("士兵损耗量")]
        public Text txtSoldierLossNum;
        [ALHeader("实力加成百分比")]
        public Text txtPowerAddPercent;
        [ALHeader("解锁条件")]
        public Text txtUnlockCondition;
        [ALHeader("大臣列表")]
        public GGUIMonoMarsExploreTeamHeroContainer monoHeroContainer;
        [ALHeader("状态剩余时间")]
        public Text stateTimeRemain;
        [ALHeader("状态进度")]
        public Slider sldStateProgress;
        [ALHeader("队伍实力")] 
        public Text txtTeamPower;
        [ALHeader("编辑按钮")]
        public GameObject btnEdit;
        [ALHeader("返回按钮")]
        public GameObject btnBack;
        [ALHeader("维修按钮")]
        public GameObject btnRepair;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("解锁跳转按钮")]
        public GameObject btnUnlockJump;
        [ALHeader("求助按钮")]
        public GameObject btnHelp;
        [ALHeader("各种状态下显示的内容")] 
        public List<GameObject> listExploringShow;
        public List<GameObject> listBackShow;
        public List<GameObject> listCollectingShow;
        public List<GameObject> listIdleShow;
        public List<GameObject> listRepairingShow;
        public List<GameObject> listEmptyShow;
        public List<GameObject> listLockShow;
        public List<GameObject> listCanAskHelpShow;
        public List<GameObject> listIdleSoliderLossShow;


        public void setState(EMarsExploreTeamUIState _state)
        {
            ALUGUICommon.setGameObjEnable(listExploringShow, false);
            ALUGUICommon.setGameObjEnable(listBackShow, false);
            ALUGUICommon.setGameObjEnable(listCollectingShow, false);
            ALUGUICommon.setGameObjEnable(listIdleShow, false);
            ALUGUICommon.setGameObjEnable(listRepairingShow, false);
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(listCanAskHelpShow, false);
            ALUGUICommon.setGameObjEnable(listIdleSoliderLossShow, false);

            switch (_state)
            {
                case EMarsExploreTeamUIState.Exploring:
                    ALUGUICommon.setGameObjEnable(listExploringShow, true);
                    break;
                case EMarsExploreTeamUIState.Back:
                    ALUGUICommon.setGameObjEnable(listBackShow, true);
                    break;
                case EMarsExploreTeamUIState.Collecting:
                    ALUGUICommon.setGameObjEnable(listCollectingShow, true);
                    break;
                case EMarsExploreTeamUIState.Idle:
                    ALUGUICommon.setGameObjEnable(listIdleShow, true);
                    break;
                case EMarsExploreTeamUIState.Repairing:
                    ALUGUICommon.setGameObjEnable(listRepairingShow, true);
                    break;
                case EMarsExploreTeamUIState.Empty:
                    ALUGUICommon.setGameObjEnable(listEmptyShow, true);
                    break;
                case EMarsExploreTeamUIState.Lock:
                    ALUGUICommon.setGameObjEnable(listLockShow, true);
                    break;
                case EMarsExploreTeamUIState.CanAskHelp:
                    ALUGUICommon.setGameObjEnable(listCanAskHelpShow, true);
                    break;
                case EMarsExploreTeamUIState.IdleSoldierLoss:
                    ALUGUICommon.setGameObjEnable(listIdleSoliderLossShow, true);
                    break;
            }
        }
    }
}