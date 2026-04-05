using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 商店子页签列表item
    /// </summary>
    public class GGUIMonoShopSubTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("名称")]
        public Text txtName2;
        [ALHeader("页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("未解锁显示的列表")]
        public List<GameObject> goLockShowList;
    }
}
