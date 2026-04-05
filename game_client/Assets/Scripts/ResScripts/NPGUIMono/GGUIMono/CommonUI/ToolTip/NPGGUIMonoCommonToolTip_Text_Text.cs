using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带两个文本跟随的子窗口基类
    /// </summary>
    public class NPGGUIMonoCommonToolTip_Text_Text : NPGGUIMonoCommonToolTip
    {

        [ALHeader("文本-1")]
        public Text txtOne;
        [ALHeader("文本-2")]
        public Text txtTwo;

        [ALHeader("按钮-1")]
        public GameObject goOne;
        [ALHeader("按钮-2")]
        public GameObject goTwo;
    }
}