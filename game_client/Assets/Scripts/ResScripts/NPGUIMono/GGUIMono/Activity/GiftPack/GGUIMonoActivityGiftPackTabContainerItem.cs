using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包页签列表item
    /// </summary>
    public class GGUIMonoActivityGiftPackTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("页签名称")]
        public Text txtTabName;
        [ALHeader("页签名称")]
        public Text txtTabName2;
        [ALHeader("页签按钮")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("在列表第一个需要展示的GO列表")]
        public List<GameObject> goFirstShowList;
        [ALHeader("在列表中间需要展示的GO列表")]
        public List<GameObject> goMiddleShowList;
        [ALHeader("在列表最后需要展示的GO列表")]
        public List<GameObject> goLastShowList;
    }
}
