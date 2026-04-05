
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟申请列表
    /// </summary>
    public class GGUIMonoGuildOthersApplyListGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildOthersApplyListGridItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}
