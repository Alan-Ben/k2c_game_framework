using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineShareGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("矿的名字")]
        public Text txtMineName;
        [ALHeader("矿的等级")]
        public Text txtMineLevel;
        [ALHeader("矿的图标")]
        public RawImage txtMineIcon;
        [ALHeader("发现者名字")]
        public Text txtOwnerName;
        [ALHeader("占领者名字")]
        public Text txtOccupierName;
        [ALHeader("剩余资源数量")]
        public Text txtRemainResNum;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("数据在加载中时的显隐对象")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        [ALHeader("颜色变化数据")]
        public List<Graphic> listColorChangeList;
        public Color colorAlliance;
        public Color colorFriend;
        public Color colorOtherAlliance;
        public Color colorStranger;
        [ALHeader("占领状态显隐对象")]
        public List<GameObject> listOccupiedShow;
        public List<GameObject> listOccupiedHide;
        [ALHeader("是否有被别的联盟攻击过的显示隐藏对象")]
        public List<GameObject> listHadAttackedByOthersShow;
        public List<GameObject> listHadAttackedByOthersHide;


        public void setLoading(bool _loading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_loading ? listLoadingShow : listLoadingHide, true);
        }
        public void setColor(bool _isAlliance, bool _isFriend, bool _isOtherAlliance)
        {
            Color targetColor = colorStranger;
            if (_isAlliance)
                targetColor = colorAlliance;
            else if (_isOtherAlliance)
                targetColor = colorOtherAlliance;
            else if (_isFriend)
                targetColor = colorFriend;
            ALUGUICommon.setUIObjColor(listColorChangeList, targetColor);
        }
        public void setOccupied(bool _occupied)
        {
            ALUGUICommon.setGameObjEnable(listOccupiedShow, false);
            ALUGUICommon.setGameObjEnable(listOccupiedHide, false);
            ALUGUICommon.setGameObjEnable(_occupied ? listOccupiedShow : listOccupiedHide, true);
        }
        public void setHadAttackedByOthers(bool _hadAttacked)
        {
            ALUGUICommon.setGameObjEnable(listHadAttackedByOthersShow, false);
            ALUGUICommon.setGameObjEnable(listHadAttackedByOthersHide, false);
            ALUGUICommon.setGameObjEnable(_hadAttacked ? listHadAttackedByOthersShow : listHadAttackedByOthersHide, true);
        }
    }
}