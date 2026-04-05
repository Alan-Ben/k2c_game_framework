using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟委托处理记录itemGrid
    /// </summary>
    public class GGUIMonoGuildEntrustRecordItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildEntrustRecordItem>
    {
        [ALHeader("没有item时需要显示的GO列表")]
        public List<GameObject> noItemShow;
    }
}