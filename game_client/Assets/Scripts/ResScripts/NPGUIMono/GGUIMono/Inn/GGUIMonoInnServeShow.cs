using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnServeShow : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("客人图标")]
        public RawImage imgGuestIcon;
        [ALHeader("客人名字")]
        public Text txtGuestName;
        [ALHeader("菜品图标")]
        public RawImage imgDishIcon;
        [ALHeader("对话文本")]
        public Text txtDialogue;
        [ALHeader("延迟多久删除")]
        public float deleteDelay;
        [ALHeader("有特殊客人时显示与隐藏的内容")]
        public List<GameObject> listSpecialGuestShow;
        public List<GameObject> listSpecialGuestHide;
        [ALHeader("对话Key列表")
        ,ALInfo("菜品名字会作为参数传进去")]
        public List<string> listDialogueKeys;


        public void setHasSpecialGuest(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listSpecialGuestShow, false);
            ALUGUICommon.setGameObjEnable(listSpecialGuestHide, false);
            ALUGUICommon.setGameObjEnable(_show ? listSpecialGuestShow : listSpecialGuestHide, true);
        }
    }
}