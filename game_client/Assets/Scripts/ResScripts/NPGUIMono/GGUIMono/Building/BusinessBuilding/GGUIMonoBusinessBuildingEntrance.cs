
using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoBusinessBuildingEntrance : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("建筑的名字")]
        public TextMeshProUGUI txtName;
        [ALHeader("当前员工的数量")]
        public TextMeshProUGUI txtEmployeeNum;
        [ALHeader("红点展示对象")]
        public List<GameObject> listRedTipShow;
    }
}