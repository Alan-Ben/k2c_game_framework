using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamSelectContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("队伍序号")]
        public Text txtNum;
        [ALHeader("第一个大臣头像")]
        public RawImage imgFirstHero;
        [ALHeader("带兵量进度条")]
        public Slider sldSoldierHp;
        [ALHeader("状态剩余时间")]
        public Text stateTimeRemain;
        [ALHeader("状态进度")]
        public Slider sldStateProgress;
        [ALHeader("队伍实力")]
        public Text txtPower;
        [ALHeader("点击按钮")]
        public GameObject btnConfirm;
        [ALHeader("编辑按钮")]
        public GameObject btnEdit;
        [ALHeader("修理按钮")]
        public GameObject btnRepair;
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
        [ALHeader("战力差显示相关")] 
        public List<GameObject> listSafeShow;
        public float safeThreshold = 1.2f;
        public List<GameObject> listDangerShow;
        public float highRiskyThreshold = 0.9f;
        public List<GameObject> listHighRiskyShow;
        [ALHeader("士兵数量比例相关内容")
         ,ALInfo("依次是：需要变颜色的对象，比例健康时显示的对象和颜色以及健康的比例阈值，比例不健康时显示的对象和颜色以及不健康的比例阈值，比例危急时显示的对象和颜色")]
        public List<Graphic> listSoldierColorChgList;
        public List<GameObject> listHealthyShow;
        public Color healthyColor;
        public float healthyPercentThreshold = 0.5f;
        public List<GameObject> listUnhealthyShow;
        public Color unhealthyColor;
        public float unhealthyPercentThreshold = 0.3f;
        public List<GameObject> listCriticalShow;
        public Color criticalColor;


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
        public void setPowerShow(float _powerRatio)
        {
            ALUGUICommon.setGameObjEnable(listHighRiskyShow, false);
            ALUGUICommon.setGameObjEnable(listDangerShow, false);
            ALUGUICommon.setGameObjEnable(listSafeShow, false);
            if (_powerRatio > safeThreshold)
                ALUGUICommon.setGameObjEnable(listSafeShow, true);
            else if (_powerRatio > highRiskyThreshold)
                ALUGUICommon.setGameObjEnable(listDangerShow, true);
            else
                ALUGUICommon.setGameObjEnable(listHighRiskyShow, true);
        }
        public void setSoldierHpPercent(float _percent)
        {
            Color targetColor;
            ALUGUICommon.setGameObjEnable(listCriticalShow, false);
            ALUGUICommon.setGameObjEnable(listUnhealthyShow, false);
            ALUGUICommon.setGameObjEnable(listHealthyShow, false);

            if (_percent >= healthyPercentThreshold)
            {
                targetColor = healthyColor;
                ALUGUICommon.setGameObjEnable(listHealthyShow, true);
            }
            else if (_percent >= unhealthyPercentThreshold)
            {
                targetColor = unhealthyColor;
                ALUGUICommon.setGameObjEnable(listUnhealthyShow, true);
            }
            else
            {
                targetColor = criticalColor;
                ALUGUICommon.setGameObjEnable(listCriticalShow, true);
            }

            ALUGUICommon.setUIObjColor(listSoldierColorChgList, targetColor);
        }
    }
}