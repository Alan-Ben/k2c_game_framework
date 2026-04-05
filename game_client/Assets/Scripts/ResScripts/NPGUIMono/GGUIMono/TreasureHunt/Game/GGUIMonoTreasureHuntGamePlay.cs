using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTreasureHuntGamePlay : _AALBasicUIWndMono
    {
        [ALHeader("区域名称")]
        public Text txtAreaName;
        [ALHeader("前近距离")]
        public Text txtDistance;
        [ALHeader("奖励提示文本，和距离多远时显示")]
        public Text txtRewardTip;
        public float showRewardTipDistance = 50;
        public List<GameObject> listHasRewardTipShow;
        public List<GameObject> listHasRewardTipHide;
        [ALHeader("助跑距离情况")]
        public Text txtRunUp;
        [ALHeader("奖励获取情况")]
        public Text txtRewardGain;
        [ALHeader("距离进度情况")]
        public Text txtDistanceProgressCur;
        public Text txtDistanceProgressTarget;
        [ALHeader("当前的血量情况")]
        public Text txtHealth;
        [ALHeader("暂停按钮")]
        public GameObject btnPause;
        [ALHeader("触摸移动按钮")]
        public GameObject btnTouchMove;
        [ALHeader("助跑和游戏中显示的对象")]
        public List<GameObject> listRunUpShow;
        public List<GameObject> listGamingShow;
        [ALHeader("移动音效ID")]
        public long moveAudioId;
        [ALHeader("拖拽过程移动音效连续播放间隔")]
        public float dragMoveAudioDuration = 0.3f;
        
        
        public void setHasRewardTipShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listHasRewardTipShow, false);
            ALUGUICommon.setGameObjEnable(listHasRewardTipHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listHasRewardTipShow : listHasRewardTipHide, true);
        }
        public void setRunUpShow()
        {
            ALUGUICommon.setGameObjEnable(listGamingShow, false);
            ALUGUICommon.setGameObjEnable(listRunUpShow, true);
        }
        public void setGamingShow()
        {
            ALUGUICommon.setGameObjEnable(listRunUpShow, false);
            ALUGUICommon.setGameObjEnable(listGamingShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6825); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6825); } }
    }
}