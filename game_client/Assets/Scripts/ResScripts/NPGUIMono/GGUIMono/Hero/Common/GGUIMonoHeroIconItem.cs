using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 大臣头像item
    /// </summary>
    public class GGUIMonoHeroIconItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("大臣名字")] 
        public TextEx txtName;
    
        [ALHeader("大臣头像")] 
        public RawImage texHeroIcon;
        [ALHeader("大臣头像背景")]
        public Image imgHeroIconBg;

        [ALHeader("大臣战力")]
        public TextEx txtHeroPower;
        [ALHeader("大臣战力Key")]
        public string txtHeroPowerKey;
    }
}