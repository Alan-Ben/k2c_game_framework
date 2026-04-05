using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会结算参宴信息item
    /// </summary>
    public class GGUIMonoDinnerCreateResultJoinItem : _AALBasicUIWndMono
    {
        [ALHeader("序号")]
        public Text txtNum;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("人气")]
        public Text txtScore;
    }
}
