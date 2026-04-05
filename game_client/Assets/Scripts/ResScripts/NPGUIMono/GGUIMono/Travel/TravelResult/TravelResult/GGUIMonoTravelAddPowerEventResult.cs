using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 增加实力事件结果窗口
    /// </summary>
    public class GGUIMonoTravelAddPowerEventResult : _ATravelResultMono
    {
        [ALHeader("大臣iconItem")]
        public GGUIMonoHeroIconItem monoHeroIcon;

        [ALHeader("增加的战力")]
        public TextEx txtAddPower;
        [ALHeader("增加的战力Key")]
        public string txtAddPowerKey;
        
        public static long uiResPathId { get { return 3609; } }
    }
}