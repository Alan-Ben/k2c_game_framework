using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 分享家人CG的item
    /// </summary>
    public class GGUIMonoShareConsortCGGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("CG图片")]
        public RawImage texIcon;
        [ALHeader("CG名称")]
        public Text txtCGName;
        [ALHeader("家人名称")]
        public Text txtConsortName;
        [ALHeader("选中按钮")]
        public GameObject btnSelected;
        [ALHeader("选中显示")]
        public List<GameObject> selectedList;
    }
}
