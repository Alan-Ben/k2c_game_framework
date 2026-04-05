using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExplorePvPLog : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("我的战报/联盟战报切换按钮")]
        public NPGGUIMonoCommonTab toggleMyLog;
        public NPGGUIMonoCommonTab toggleGuildLog;
        [ALHeader("日志数量提示")]
        public Text txtLogNumTip;
        [ALHeader("日志列表容器")]
        public GGUIMonoMarsExplorePvPLogGrid monoLogGrid;
        [ALHeader("各种状态下显示的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listEmptyShow;
        public List<GameObject> listNormalShow;
        [ALHeader("没有联盟时显示的内容")]
        public List<GameObject> listNoGuildShow;
        public List<GameObject> listNoGuildHide;


        public void setHasGuild(bool _hasGuild)
        {
            ALUGUICommon.setGameObjEnable(listNoGuildShow, false);
            ALUGUICommon.setGameObjEnable(listNoGuildHide, false);
            ALUGUICommon.setGameObjEnable(_hasGuild ? listNoGuildHide : listNoGuildShow, true);
        }

        public void setState(bool _isLoading, bool _isEmpty)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listNormalShow, false);
            if (_isLoading)
                ALUGUICommon.setGameObjEnable(listLoadingShow, true);
            else
                ALUGUICommon.setGameObjEnable(_isEmpty ? listEmptyShow : listNormalShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7414); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7414); } }
    }
}