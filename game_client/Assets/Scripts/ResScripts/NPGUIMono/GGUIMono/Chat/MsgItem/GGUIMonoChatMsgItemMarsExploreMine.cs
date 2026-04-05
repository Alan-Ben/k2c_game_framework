using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChatMsgItemMarsExploreMine : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("矿名字")]
        public Text txtName;
        [ALHeader("矿等级")]
        public Text txtLevel;
        [ALHeader("详情按钮")]
        public GameObject btnInfo;
        [ALHeader("占领者名字")]
        public Text txtOccupied;
        [ALHeader("占领与否的显示内容")]
        public List<GameObject> listOccupiedShow;
        public List<GameObject> listOccupiedHide;
        [ALHeader("不同占领者的颜色相关内容")]
        public List<Graphic> listOccupiedColor;
        public Color selfOccupiedColor = Color.green;
        public Color alliedOccupiedColor = Color.cyan;
        public Color enemyOccupiedColor = Color.red;
        [ALHeader("矿已经失效的显示内容")]
        public List<GameObject> listMineDisableShow;
        public List<GameObject> listMineDisableHide;
        [ALHeader("加载中显示内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;


#if NP_GAME
        public void setOccupiedState(bool _occupied, bool _isSelf, bool _isGuildMember)
        {
            ALUGUICommon.setGameObjEnable(listOccupiedShow, false);
            ALUGUICommon.setGameObjEnable(listOccupiedHide, false);
            ALUGUICommon.setGameObjEnable(_occupied ? listOccupiedShow : listOccupiedHide, true);
            if (_occupied)
            {
                Color color = _isSelf ? selfOccupiedColor : (_isGuildMember ? alliedOccupiedColor : enemyOccupiedColor);
                ALUGUICommon.setUIObjColor(listOccupiedColor, color);
            }
        }
        public void setLoadingShow(bool _isLoading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isLoading ? listLoadingShow : listLoadingHide, true);
        }
        public void setDisableShow(bool _isDisable)
        {
            ALUGUICommon.setGameObjEnable(listMineDisableShow, false);
            ALUGUICommon.setGameObjEnable(listMineDisableHide, false);
            ALUGUICommon.setGameObjEnable(_isDisable ? listMineDisableShow : listMineDisableHide, true);
        }
#endif
        
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1387); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1387);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1386); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1386);} }
    }
}