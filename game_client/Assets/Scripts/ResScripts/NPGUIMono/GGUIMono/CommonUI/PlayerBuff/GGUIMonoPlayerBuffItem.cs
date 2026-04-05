using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家buff item显示
    /// </summary>
    public class GGUIMonoPlayerBuffItem : _AALBasicUIWndMono
    {
        [ALHeader("buff名")]
        public TextEx txtBuffName;
        [ALHeader("buff名文本key, 一个参数:buff名")]
        public string txtBuffNameKey;
        
        [ALHeader("buff描述")]
        public TextEx txtBuffDesc;
        [ALHeader("buff描述文本key, 一个参数:buff描述")]
        public string txtBuffDescKey;
        
        [ALHeader("buff图标")]
        public RawImage buffIcon;
        
        [ALHeader("buff层数")]
        public TextEx txtBuffLayer;
        [ALHeader("buff层数文本key, 一个参数:buff层数")]
        public string txtBuffLayerKey;
    }
}