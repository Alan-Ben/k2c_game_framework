using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 账号绑定方式列表item
    /// </summary>
    public class NPGGUIMonoAccountBindContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage imgLoginType;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("已绑定需要展示的GO列表")]
        public List<GameObject> goBindShowList;
        [ALHeader("未绑定需要展示的GO列表")]
        public List<GameObject> goUnBindShowList;
    }
}