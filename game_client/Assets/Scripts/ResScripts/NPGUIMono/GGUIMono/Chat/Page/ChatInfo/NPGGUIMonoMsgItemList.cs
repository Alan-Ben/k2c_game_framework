
using System.Collections.Generic;
using ChatPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoMsgItemList : GUIMonoChatMsgList
    {
        [ALHeader("有未读消息时显示的内容")]
        public List<GameObject> unreadMsgShowList;
        [ALHeader("切到最底端按钮")]
        public GameObject toBottomBtn;
        [ALHeader("历史消息获取数量上限")]
        public int maxHistroyInfoCount = 100;
    }
}