using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 可设置选中颜色的页签
    /// </summary>
    public class GGUIMonoCommonSetColorTab : NPGGUIMonoCommonTab
    {
        [ALHeader("选中时需要设置的颜色")]
        public Color selectColor = Color.white;
        [ALHeader("未选中时需要设置的颜色")]
        public Color notSelectColor = Color.black;
        [ALHeader("需要设置颜色的对象列表")]
        public List<Graphic> setColorList;
    }
}
