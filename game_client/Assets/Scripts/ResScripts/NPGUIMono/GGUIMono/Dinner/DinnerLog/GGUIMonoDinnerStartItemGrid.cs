using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办历史item容器
    /// </summary>
    public class GGUIMonoDinnerStartItemGrid : _TALUGUIMonoGridWnd<GGUIMonoDinnerStartItem>
    {
        [ALHeader("列表为空的时候显示")]
        public List<GameObject> emptyListShow;
        [ALHeader("列表为空的时候隐藏")]
        public List<GameObject> emptyListHide;
    }
}
