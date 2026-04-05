using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 首充特殊奖励形象展示
    /// </summary>
    public class GGUIMonoSubFirstRechargeSpecialRewardActor : _AALBasicUIWndMono
    {
        [ALHeader("顾问情人形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("顾问情人形象")]
        public RawImage imgActor;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("获得提示文本")]
        public Text txtGainTip;
        [ALHeader("预览按钮")]
        public GameObject btnPreview;
        [ALHeader("是顾问时需要显示的GO列表")]
        public List<GameObject> goHeroShowList;
        [ALHeader("是家人时需要显示的GO列表")]
        public List<GameObject> goConsortShowList;
    }
}
