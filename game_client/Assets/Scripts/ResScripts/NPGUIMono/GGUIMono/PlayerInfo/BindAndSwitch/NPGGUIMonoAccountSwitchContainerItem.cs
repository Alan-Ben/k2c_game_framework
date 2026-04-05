using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 账号切换方式列表item
    /// </summary>
    public class NPGGUIMonoAccountSwitchContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage imgLoginType;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("当前登录文本颜色")]
        public Color curLoginTextColor = Color.white;
        [ALHeader("不是当前登录文本颜色")]
        public Color notCurLoginTextColor = Color.black;
        [ALHeader("已登录需要展示的GO列表")]
        public List<GameObject> goLoginShowList;
        [ALHeader("未登录需要展示的GO列表")]
        public List<GameObject> goNoLoginShowList;
    }
}