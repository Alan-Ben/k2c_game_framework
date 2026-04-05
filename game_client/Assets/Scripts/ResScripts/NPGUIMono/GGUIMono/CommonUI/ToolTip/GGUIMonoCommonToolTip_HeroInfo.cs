using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士信息跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_HeroInfo : NPGGUIMonoCommonToolTip
    {
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("擅长属性")]
        public GGUIMonoCommonAttrContainer attrContainer;
    }
}