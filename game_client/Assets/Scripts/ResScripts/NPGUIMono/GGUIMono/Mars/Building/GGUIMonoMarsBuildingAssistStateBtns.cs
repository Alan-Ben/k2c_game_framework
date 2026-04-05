using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 科研所状态常驻hud按钮
    /// </summary>
    public class GGUIMonoMarsBuildingAssistStateBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("帮助盟友加速按钮")]
        public GameObject btnAssist;
        [ALHeader("当前盟友求助数量")]
        public Text txtAssistCount;
        [ALHeader("1个求助的状态隐藏的列表")]
        public List<GameObject> oneAssistHideList;
        [ALHeader("有可帮助的状态显示")]
        public List<GameObject> hasAssistShowList;
    }
}