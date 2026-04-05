using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineTip : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("采集点资源剩余")]
        public Slider sldResourceRemain;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("矿的图标")]
        public RawImage imgIcon;
        [ALHeader("矿的等级")]
        public Text txtLevel;
        [ALHeader("我方占领时队伍信息")]
        public RawImage imgMyTeamHeroIcon;
        public Text txtMyTeamNum;
        public Text txtMyTeamName;
        public Text txtCollectRemainTime;
        public Slider sldCollectProgress;
        [ALHeader("各种状态下的显示隐藏内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listCollectingShow;
        public List<GameObject> listEnemyOccupyShow;
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
        [ALHeader("颜色变化的Graphic列表")]
        public List<Graphic> listColorChg;


        public void setState(bool _isEmpty, bool _isOccupiedByMe)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listCollectingShow, false);
            ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, false);

            if (_isEmpty)
                ALUGUICommon.setGameObjEnable(listEmptyShow, true);
            else if (_isOccupiedByMe)
                ALUGUICommon.setGameObjEnable(listCollectingShow, true);
            else
                ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, true);
        }
        public void setBackEventNum(int _itemNum)
        {
            ALUGUICommon.setGameObjEnable(listMoreThanOneShow, false);
            ALUGUICommon.setGameObjEnable(listMoreThanOneHide, false);
            ALUGUICommon.setGameObjEnable(_itemNum > 1 ? listMoreThanOneShow : listMoreThanOneHide, true);
        }
    }
}