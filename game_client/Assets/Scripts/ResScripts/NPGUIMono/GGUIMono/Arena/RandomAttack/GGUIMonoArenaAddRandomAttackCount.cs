using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场增加谈判次数弹窗
    /// </summary>
    public class GGUIMonoArenaAddRandomAttackCount : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("已购买次数")]
        public Text txtAlreadyBuyCount;
        [ALHeader("增加按钮")]
        public GameObject btnIncrease;
        [ALHeader("减少按钮")]
        public GameObject btnDecrease;
        [ALHeader("最大数量按钮")]
        public GameObject btnMax;
        [ALHeader("最小数量按钮")]
        public GameObject btnMini;
        [ALHeader("达到最大数量时需要置灰的列表")]
        public List<MaskableGraphic> maxGrayImgList;
        [ALHeader("达到最小数量时需要置灰的列表")]
        public List<MaskableGraphic> minGrayImgList;
        [ALHeader("选择的数量信息")]
        public Text txtSelectCount;
        [ALHeader("选择数量进度条")]
        public NPGGUIMonoCommonSlider selectCountSlider;
        [ALHeader("购买按钮")]
        public GameObject btnBuy;
        [ALHeader("购买消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5204); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5204); } }
    }
}