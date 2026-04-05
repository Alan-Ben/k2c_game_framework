using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 好友添加分组
    /// </summary>
    public class GGUIMonoFriendAddGruop:_AALBasicUIWndMono
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
        [ALHeader("添加分组按钮")]
        public GameObject btnAddGroup;
        [ALHeader("分组数量限制文本")]
        public TextEx txtGroupLimit;
        
        [ALHeader("好友列表Grid")]
        public GGUIMonoFriendGruopEditorFriendListGrid friendListGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1325); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1325);} }
    }
}