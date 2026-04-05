using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子卷王子嗣buff通用ToolTip
    /// </summary>
    public class GGUIMonoCommonToolTip_ConsortGiftedChildBuff : NPGGUIMonoCommonToolTip
    {
        [ALHeader("buff次数文本")]
        public TextEx txtBuffCount;
        [ALHeader("buff次数文本key(一个参数, 剩余次数)")]
        public string txtBuffCountKey;
        
        [ALHeader("有buff次数时")]
        public List<GameObject> hasBuffCountShow; 
        [ALHeader("没有buff次数时")]
        public List<GameObject> noBuffCountShow;
    }
}