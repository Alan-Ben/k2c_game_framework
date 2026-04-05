using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 充值返利页签列表item
    /// </summary>
    public class GGUIMonoRechargeRebatePageTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("页签名称")]
        public Text txtTabName;
        [ALHeader("页签名称")]
        public Text txtTabName2;
        [ALHeader("页签按钮")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签红点")]
        public GameObject goRedTip;
        [ALHeader("是首个页签时显示的GO列表")]
        public List<GameObject> goFirstShowList;
        [ALHeader("是首个页签时隐藏的GO列表")]
        public List<GameObject> goFirstHideList;
    }
}
