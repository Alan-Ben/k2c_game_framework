using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子指定邀约buff通用ToolTip
    /// </summary>
    public class GGUIMonoCommonToolTip_ConsortAssignInviteBuff : NPGGUIMonoCommonToolTip
    {
        [ALHeader("buff次数文本")]
        public TextEx txtBuffCount;
        [ALHeader("buff次数文本key(一个参数, 剩余次数)")]
        public string txtBuffCountKey;

        [ALHeader("有buff次数时")]
        public List<GameObject> hasBuffCountShow; 
        [ALHeader("没有buff次数时")]
        public List<GameObject> noBuffCountShow;
        
        [ALHeader("指定邀约的妃子列表")]
        public GGUIMonoConsortIconItemContainer monoAssignConsortContainer;
    }
}