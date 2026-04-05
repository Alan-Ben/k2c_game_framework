using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class NPGGUIBoxShareMsgShowParam
    {
        [ALHeader("宝箱状态")]
        public ENPBoxChatStatus status;
        [ALHeader("显示的物体")]
        public List<GameObject> goList;
    }

    /// <summary>
    /// 通用宝箱分享banner附加窗口
    /// </summary>
    public class NPGGUIMonoChatMsgItemCommonBoxSubPrefab : _AALBasicUIWndMono
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayer;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("宝箱名字")]
        public Text txtName;
        [ALHeader("宝箱描述")]
        public Text txtContent;
        [ALHeader("宝箱Banner图标")]
        public RawImage imgBannerIcon;
        [ALHeader("剩余宝箱个数")]
        public Text txtLeftCount;
        [ALHeader("不同消息状态的显示配置")]
        public List<NPGGUIBoxShareMsgShowParam> msgStatusShowParams;
    }
}
