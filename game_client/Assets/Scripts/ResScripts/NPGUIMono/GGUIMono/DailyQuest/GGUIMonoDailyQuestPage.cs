using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 每日任务信息页面
    /// </summary>
    public class GGUIMonoDailyQuestPage : _AALBasicUIWndMono
    {
        [ALHeader("重置时间")]
        public Text txtTime;
        [ALHeader("积分图标")]
        public RawImage imgScoreIcon;
        [ALHeader("当前积分")]
        public Text txtScore;
        [ALHeader("活跃积分进度")]
        public GGUIMonoCommonRewardSlider monoRewardSlider;
        [ALHeader("任务列表")]
        public GGUIMonoDailyQuestContainer monoDailyQuestContainer;
        [ALHeader("活跃积分奖励全部领取时需要展示的GO列表")]
        public List<GameObject> goActiveRewardAllGetShowList;
        [ALHeader("活跃积分奖励全部领取时需要隐藏的GO列表")]
        public List<GameObject> goActiveRewardAllGetHideList;

        [ALHeader("粒子动画终点")]
        public RectTransform particleEndRect;
        [ALHeader("粒子需要播放的额外动画")]
        public CommonAnimationShowTypeInfo<EAchieveParticleAniType> particleAni;


        [ALHeader("粒子到达时播放特效父节点")]
        public Transform particleSfxParent;
        [ALHeader("粒子到达时播放特效id")]
        public long particleSfxId;
        [ALHeader("特效可同时存在的最大数量")]
        public long particleSfxMaxNum = 1;

        [ALInfo("====宝箱预览配置====")]
        [ALHeader("每日任务宝箱奖励预览pathid")]
        public long boxRewardPreviewResPathId;
        [ALHeader("每日任务宝箱奖励预览标题翻译key")]
        public string boxRewardPreviewTitleStrKey;
        [ALHeader("每日任务宝箱奖励预览弹窗位置偏移")]
        public Vector2 boxRewardPreviewToolTipOffset;
    }
}
