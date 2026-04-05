using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 尾刀记录item列表
    /// </summary>
    public class GGUIMonoEveningDungeonFinalAttackRecordContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoEveningDungeonFinalAttackRecordItem>
    {
        [ALHeader("没有记录时显示的GO列表")]
        public List<GameObject> noItemShow;
    }
}