using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 好友分组编辑
    /// </summary>
    public class GGUIMonoFriendGruopEditor:_AALBasicUIWndMono
    {
        [ALHeader("关闭按钮按钮")]
        public GameObject btnClose;
        [ALHeader("分组名字文本")]
        public InputField groupNameInputField;
        [ALHeader("分组名当前字符数量")]
        public TextEx txtGroupNameCharCount;
        [ALHeader("字符数量剩余足够的时候显示的颜色")]
        public Color charEmoughColor = Color.green;
        [ALHeader("字符数量已达上限的时候显示的颜色")]
        public Color charMaxColor = Color.red;
        [ALHeader("改变组名按钮")]
        public GameObject btnChgGroupName;
        [ALHeader("删除分组按钮")]
        public GameObject btnRemoveGroup;
        [ALHeader("关闭分组列表")]
        public GameObject btnCloseGroupContainer;
        [ALHeader("移动好友按钮")]
        public NPGGUIMonoCommonToggleEx togMoveFriend;
        [ALHeader("选择的数量")]
        public TextEx txtSelectedCount;
        [ALHeader("是默认分组时需要显示的GO列表")]
        public List<GameObject> goDefaultShowList;
        [ALHeader("是默认分组时需要隐藏的GO列表")]
        public List<GameObject> goDefaultHideList;
        
        [ALHeader("好友列表Grid")]
        public GGUIMonoFriendGruopEditorFriendListGrid friendListGrid;
        [ALHeader("点击移动好友弹出的分组列表")]
        public GGUIMonoFriendGruopEditorGroupItemContainer groupItemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1315); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1315);} }
    }
}