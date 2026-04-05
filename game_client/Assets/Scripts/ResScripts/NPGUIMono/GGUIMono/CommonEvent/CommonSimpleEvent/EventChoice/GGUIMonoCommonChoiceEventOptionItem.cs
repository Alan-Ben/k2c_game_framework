using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 选项事件格 - 选项
    /// </summary>
    public class GGUIMonoCommonChoiceEventOptionItem : _AGGUIMonoOptionItem
    {
        [ALHeader("文本描述")]
        public TextEx txtDesc;
        
        [ALHeader("选项图片")]
        public RawImage texIcon;
    }
}