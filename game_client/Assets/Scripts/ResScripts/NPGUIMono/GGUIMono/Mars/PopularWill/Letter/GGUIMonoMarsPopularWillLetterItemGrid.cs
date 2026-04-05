using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意信件列表
    /// </summary>
    public class GGUIMonoMarsPopularWillLetterItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoMarsPopularWillLetterItem>
    {
        [ALHeader("没有信件时显示")]
        public List<GameObject> noItemShow;
    }
}