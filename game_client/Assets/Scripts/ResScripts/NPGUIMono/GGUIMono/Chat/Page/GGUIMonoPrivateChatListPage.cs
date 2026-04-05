using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 私聊列表
    /// </summary>
    public class GGUIMonoPrivateChatListPage : _AALBasicUIWndMono
    {
        [ALHeader("频道的item")]
        public GGUIMonoChatListGrid privateChatGrid;
        [ALHeader("批量选择tog")]
        public NPGGUIMonoCommonToggleEx togSelected;
        [ALHeader("批量选择按钮")]
        public GameObject btnSelected;
        [ALHeader("取消批量选择")]
        public GameObject btnCancel;
        [ALHeader("删除")]
        public GameObject btnRemove;
        [ALHeader("选中批量选择的时候显示，反之隐藏")]
        public List<GameObject> showSelectedGoList;
        [ALHeader("选中批量选择的时候隐藏，反之显示")]
        public List<GameObject> hideSelectedGoList;
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1312); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1312);} }
    }
}