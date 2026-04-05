using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用对应的一个资源收集目标对象
    /// 处理了文字会跟着变
    /// </summary>
    public class NPGGUICommonHarvestMono: _AALBasicUIWndMono
    {
        [ALHeader("文字text")]
        public Text txtNum;
        [ALHeader("收集点位置")]
        public RectTransform tarTransform;
        [ALHeader("翻译key")]
        public string transKey;
        [ALHeader("征收类型")]
        public EHarvestType harvestResType;
    }
}