using ALPackage;
using CommonEnum;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoCommonPropertyItem : _AALBasicUIWndMono
    {
        [ALHeader("需要获取来自哪个功能模块的加成")]
        public EUnionBonusMgrTag tag;
        [ALHeader("需要的属性值类型")]
        public EBonusPropertyType propertyType;
        [ALHeader("数值显示的文本")]
        public Text txtValue;
        [ALHeader("文本采用的语言 key ")]
        [ALInfo("文本会填入 {0} 中")]
        public string valueKey;
        [ALHeader("是否展示为百分数")]
        [ALInfo("勾上后原值会被除以 100 ")]
        public bool isPercentage = false;
    }
}