using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟旗帜列表item
    /// </summary>
    public class GGUIMonoGuildFlagSelectGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnSelect;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
    }
}
