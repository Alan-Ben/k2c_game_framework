using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息联盟成员列表item
    /// </summary>
    public class GGUIMonoGuildOtherInfoMemberGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("职位")]
        public Text txtPosition;
    }
}
