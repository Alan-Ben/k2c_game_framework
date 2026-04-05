using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星属性显示item
    /// </summary>
    public class GGUIMonoMarsPropertyShowItem : _AALBasicUIWndMono
    {
        [ALHeader("属性名称文本")]
        public TextEx txtPropertyName;
        
        [ALHeader("非负数时是否需要显示加号(只作用于当前数值和变化后数值, 变化的数值一定会有符号)")]
        public bool nonnegativeNeedShowPlusSign;
        
        [ALHeader("当前属性数值文本")]
        public List<TextEx> txtNowPropertyValueList;
        
        [ALHeader("变化(增加/减少)的属性数值文本")]
        public List<TextEx> txtChgPropertyValueList;
        [ALHeader("有变化(增加/减少)的属性数值时展示的物体列表")]
        public List<GameObject> hasChgValueShowList;
        [ALHeader("没有变化(增加/减少)的属性数值时显示的物体列表")]
        public List<GameObject> noChgValueShowList;
        
        [ALHeader("变化后属性数值文本")]
        public List<TextEx> txtNextPropertyValueList;
    }
}