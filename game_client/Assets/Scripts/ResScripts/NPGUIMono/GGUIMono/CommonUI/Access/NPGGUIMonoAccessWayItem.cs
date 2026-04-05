using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoAccessWayItem : _AALBasicUIWndMono
    {
        [ALHeader("跳转按钮")]
        public GameObject btnGoTo;

        [ALHeader("图标")]
        public RawImage imgIcon;

        [ALHeader("获取途径标题")]
        public Text txtTitle;

        [ALHeader("获取途径描述")]
        public Text txtDesc;

        [ALHeader("无跳转效果时 隐藏的物体")]
        public List<GameObject> goListHideOnNoGoToEffect;

        [ALHeader("获取途径未开启原因描述")]
        public Text txtLockDesc;

        [ALHeader("必定获得时 显示的物体")]
        public List<GameObject> goListShowOnSure;

        [ALHeader("获取途径未开启时 显示的物体")]
        public List<GameObject> goListShowOnLock;

        [ALHeader("获取途径未开启时 隐藏的物体")]
        public List<GameObject> goListHideOnLock;
    }
}
