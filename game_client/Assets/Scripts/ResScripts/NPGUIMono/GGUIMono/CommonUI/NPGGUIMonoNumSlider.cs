
using UnityEngine;
using UnityEngine.UI;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进度条数字选择子窗口
    /// </summary>
    public class NPGGUIMonoNumSlider : MonoBehaviour
    {
        [ALHeader("选择物品数量的滚动条")]
        public Slider sldNumSelect;
        [ALHeader("选中数量增加按钮")]
        public GameObject btnNumSelectPlus;
        [ALHeader("选中数量减少按钮")]
        public GameObject btnNumSelectDec;
        [ALHeader("选中的数量/总数量")]
        public Text txtSelectNum;
    }
}