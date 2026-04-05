using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

//子嗣属性详情
namespace GOE
{
    public class NPGGUIMonoCommonToolTip_ChildAttrDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("亲密关系值")]
        public Text intimacyChildTxt;

        [ALHeader("亲密关环值")]
        public Text intimacyRoundTxt;
    }
}
