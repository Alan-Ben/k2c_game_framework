using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDMonoInnScene : MonoBehaviour
    {
        [ALHeader("单位的父节点")]
        public Transform unitRoot;
        [ALHeader("收银机位置")]
        public Transform cashRegisterPosition;
        [ALHeader("服务台")]
        public GTDMonoInnServeCounter monoServeCounter;
        [ALHeader("客人离开位置")]
        public Transform guestLeavePos;
        [ALHeader("客人移动速度")]
        public float guestMoveSpeed = 1;
        [ALHeader("同屏最大的客人数量")]
        public int maxGuestCount = 10;
        [ALHeader("有特殊客人时显示与隐藏的内容")]
        public List<GameObject> listSpecialGuestShow;
        public List<GameObject> listSpecialGuestHide;
        
        
        public void setHasSpecialGuest(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listSpecialGuestShow, false);
            ALUGUICommon.setGameObjEnable(listSpecialGuestHide, false);
            ALUGUICommon.setGameObjEnable(_show ? listSpecialGuestShow : listSpecialGuestHide, true);
        }
    }
}