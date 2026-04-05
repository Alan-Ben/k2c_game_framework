using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAchieveGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成就名")]
        public Text txtName;
        [ALHeader("成就步骤进度文本(可领取时展示)")]
        public Text txtProcess;
        [ALHeader("成就步骤进度文本(不可领取时展示)")]
        public Text txtCanNotGetProcess;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonMaskItemContainer rewardItemContainer;
        [ALHeader("状态列表")]
        public List<GGUIAchievePointProgressState> stateList;
        [ALHeader("粒子开始飞行位置")]
        public RectTransform particleStartTrans;

        [ALHeader("领取步骤奖励特效id")]
        public long getRewardSfxId;
        [ALHeader("领取步骤奖励特效父节点")]
        public Transform transGetRewardSfxParent;
    }
}
