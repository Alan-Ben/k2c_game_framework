using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSmoothDampLongText : _AALBasicUIWndMono
    {
        [ALHeader("文本")]
        public Text txtValue;
        [ALHeader("平滑时间"), ALInfo("不是变化时间，但总之是值越小越快")]
        public float smoothTime = 0.2f;
        [ALHeader("额外的语言 key ")]
        public string textKey;
        [ALHeader("是否显示为大数字")]
        public bool showLargeNumber = true;
    }
}