
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用的container物品进度条数字选择子窗口
    /// </summary>
    public class NPGGUIMonoCommonContainerItemNumSlider : _AALBasicUIWndMono
    {
        [ALHeader("物品的基本信息")]
        public NPGGUIMonoCommonItem item;

        [ALHeader("数量选择条")]
        public NPGGUIMonoNumSlider monoNumSlider;
    }
}
