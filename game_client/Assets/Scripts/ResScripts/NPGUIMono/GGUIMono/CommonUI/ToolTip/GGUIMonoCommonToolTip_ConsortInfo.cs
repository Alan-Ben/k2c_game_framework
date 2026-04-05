using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子信息跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_ConsortInfo : NPGGUIMonoCommonToolTip
    {
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("亲密度")]
        public Text txtIntimacy;
        [ALHeader("魅力值")]
        public Text txtCharm;
        
        
    }
}