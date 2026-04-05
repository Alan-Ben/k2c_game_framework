using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoItemTextTextTip : NPGGUIMonoCommonTip
    {
        [ALHeader("文本1")]
        public Text txtStrOne;
        [ALHeader("文本2")]
        public Text txtStrTow;
        [ALHeader("item")]
        public NPGGUIMonoCommonItem item;
        [ALHeader("item特殊显示品质框")]
        public Image itemQualityImg;
    }
}
