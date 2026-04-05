using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 进度奖励item
    /// </summary>
    public class GGUIMonoCommonSimpleItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("背景")]
        public Image imgBg;
        [ALHeader("数量")]
        public Text txtNum;
        [ALHeader("数量描述时使用的key")]
        public string txtNumKey;
        [ALHeader("不显示数量文本需要隐藏的GO列表")]
        public List<GameObject> goTextHideList;
    }
}