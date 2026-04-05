using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场资源收集附加窗口
    /// </summary>
    public class GGUIMonoArenaStationCollection : _AALBasicUIWndMono
    {
        [ALHeader("获取银币按钮")]
        public GameObject btnGetSilver;
        [ALHeader("收集银币进度")]
        public Slider sldSilver;
        [ALHeader("银币数量")]
        public Text txtSilverNum;
        [ALHeader("银币图标")]
        public RawImage imgSilver;
        [ALHeader("银币收集满时需要显示的GO列表")]
        public List<GameObject> goFullShowList;
        [ALHeader("银币收集满时需要隐藏的GO列表")]
        public List<GameObject> goFullHideList;
        [ALHeader("银币粒子开始位置")]
        public RectTransform particleStartRectTransform;
        [ALHeader("粒子展示数量")]
        public int particleShowCount;
        [ALHeader("增加资源上浮center_tip id")]
        public long addTipID;
        [ALHeader("增加资源上浮提示展示父节点")]
        public Transform addTipParent;
    }
}