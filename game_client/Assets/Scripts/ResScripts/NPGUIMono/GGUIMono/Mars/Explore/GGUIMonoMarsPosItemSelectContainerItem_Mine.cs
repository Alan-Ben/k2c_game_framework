using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsPosItemSelectContainerItem_Mine : _AALBasicUIWndMono
    {
        [ALHeader("等级文本")]
        public Text txtLevel;
        [ALHeader("图标图片")]
        public RawImage imgIcon;
        [ALHeader("名称文本")]
        public Text txtName;
        [ALHeader("剩余次数文本")]
        public Text txtRemainCount;
        [ALHeader("各种状态下显示的内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listCollectingShow;
        public List<GameObject> listEnemyOccupyShow;
        public List<GameObject> listGuildMemberShow;
        [ALHeader("探索按钮")]
        public GameObject btnGo;
        [ALHeader("加载中显示的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        
        public void setState(bool _isEmpty, bool _isSelf, bool _isGuildMember)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listCollectingShow, false);
            ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, false);

            if (_isEmpty)
                ALUGUICommon.setGameObjEnable(listEmptyShow, true);
            else if (_isSelf)
                ALUGUICommon.setGameObjEnable(listCollectingShow, true);
            else if(_isGuildMember)
                ALUGUICommon.setGameObjEnable(listGuildMemberShow, true);
            else
                ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, true);
        }
        public void setLoadingShow(bool _isLoading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isLoading ? listLoadingShow : listLoadingHide, true);
        }
        
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7421); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7421); } }
    }
}