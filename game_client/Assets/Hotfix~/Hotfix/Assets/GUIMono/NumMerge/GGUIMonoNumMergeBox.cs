
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBox : _AHotfixBaseMono
    {
        [HotfixMono("宝箱资源父节点")]
        public Transform transBoxGoParent;
        [HotfixMono("当前进度条")]
        public Slider sldProgress;
        [HotfixMono("宝箱的进度值")]
        public Text txtValue;
        [HotfixMono("可领取的奖励数量")]
        public Text txtRewardCount;
        [HotfixMono("宝箱详情按钮")]
        public GameObject btnDetail;
        [HotfixMono("领取按钮")]
        public GameObject btnGet;
        [HotfixMono("有奖励领时显示的内容")]
        public List<GameObject> listHasRewardShow;
        [HotfixMono("无奖励领时显示的内容")]
        public List<GameObject> listHasRewardHide;


        public void setHasReward(bool _hasReward)
        {
            ALUGUICommon.setGameObjEnable(listHasRewardShow, false);
            ALUGUICommon.setGameObjEnable(listHasRewardHide, false);
            ALUGUICommon.setGameObjEnable(_hasReward ? listHasRewardShow : listHasRewardHide, true);
        }
    }
}