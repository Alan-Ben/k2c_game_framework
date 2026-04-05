using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreBossEventTip : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("是否完成的显示隐藏内容")]
        public List<GameObject> listIsDoneShow;
        public List<GameObject> listIsDoneHide;
        [ALHeader("该地点堆叠了几个事件相关")]
        public Text txtBackEventNum;
        public List<GameObject> listMoreThanOneShow;
        public List<GameObject> listMoreThanOneHide;
        [ALHeader("作为新事件出现时的动画")]
        public Animation newEventShowAnim;
        public string newEventShowAnimName;
        [ALHeader("作为新事件出现时的特效")]
        public Transform newEventSfxParent;
        public long newEventSfxId;
        
        
        public void setIsDone(bool _isDone)
        {
            ALUGUICommon.setGameObjEnable(listIsDoneShow, false);
            ALUGUICommon.setGameObjEnable(listIsDoneHide, false);
            ALUGUICommon.setGameObjEnable(_isDone ? listIsDoneShow : listIsDoneHide, true);
        }
        public void setBackEventNum(int _itemNum)
        {
            ALUGUICommon.setGameObjEnable(listMoreThanOneShow, false);
            ALUGUICommon.setGameObjEnable(listMoreThanOneHide, false);
            ALUGUICommon.setGameObjEnable(_itemNum > 1 ? listMoreThanOneShow : listMoreThanOneHide, true);
        }
    }
}
