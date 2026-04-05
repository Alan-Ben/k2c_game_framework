using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoIconTextTip : NPGGUIMonoCommonTip
    {
        [ALHeader("文本列表")]
        public List<Text> txtStrList;
        [ALHeader("图标")]
        public RawImage icon;

        [ALHeader("点击按钮")] 
        public GameObject btnClick;
    }
}
