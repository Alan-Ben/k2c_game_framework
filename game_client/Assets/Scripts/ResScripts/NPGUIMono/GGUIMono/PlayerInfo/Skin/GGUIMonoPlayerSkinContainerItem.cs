using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤item
    /// </summary>
    public class GGUIMonoPlayerSkinContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("可解锁或升级红点")]
        public GameObject goRedTip;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        [ALHeader("未解锁需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("是当前佩戴时需要显示的GO列表")]
        public List<GameObject> goCurrentShowList;
        [ALHeader("是当前佩戴时需要隐藏的GO列表")]
        public List<GameObject> goCurrentHideList;
    }
}
