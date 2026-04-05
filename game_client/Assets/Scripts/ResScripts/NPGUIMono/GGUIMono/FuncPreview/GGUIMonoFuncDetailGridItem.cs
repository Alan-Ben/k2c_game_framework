using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 功能列表item
    /// </summary>
    public class GGUIMonoFuncDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("选中时 显示的物体")]
        public List<GameObject> goListShowOnSelect;
        [ALHeader("选中时 隐藏的物体")]
        public List<GameObject> goListHideOnSelect;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("已解锁可领取需要显示的GO")]
        public List<GameObject> goCanGetShowList;
        [ALHeader("未解锁需要显示的GO")]
        public List<GameObject> goLockShowList;
        [ALHeader("已领取需要显示的GO")]
        public List<GameObject> goAlreadyGetShowList;
        [ALHeader("未解锁需要置灰的列表")]
        public List<MaskableGraphic> lockGrayList;
        [ALHeader("选中时文本颜色")]
        public Color selectTextColor = Color.white;
        [ALHeader("未选中时文本颜色")]
        public Color noSelectTextColor = Color.white;
    }
}