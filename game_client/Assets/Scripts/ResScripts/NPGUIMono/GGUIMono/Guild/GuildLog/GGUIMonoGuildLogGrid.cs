
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟日志列表
    /// </summary>
    public class GGUIMonoGuildLogGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildLogGridItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}
