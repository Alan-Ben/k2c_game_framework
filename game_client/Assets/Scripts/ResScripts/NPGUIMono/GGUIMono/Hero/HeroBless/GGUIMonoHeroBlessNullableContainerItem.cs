using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴家人可为空头像列表item
    /// </summary>
    public class GGUIMonoHeroBlessNullableContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("头像品质背景")]
        public Image imgIconBg;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        [ALHeader("未解锁需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("内容为空时需要展示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("内容为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}
