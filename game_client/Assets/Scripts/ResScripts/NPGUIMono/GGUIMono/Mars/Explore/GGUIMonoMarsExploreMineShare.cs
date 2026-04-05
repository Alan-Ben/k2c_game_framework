using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineShare : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("item 列表")]
        public GGUIMonoMarsExploreMineShareGrid monoItemGrid;
        [ALHeader("数据数量描述文本")]
        public Text txtDataCountDesc;
        [ALHeader("我的矿/联盟矿切换按钮")]
        public NPGGUIMonoCommonTab toggleMyMine;
        public NPGGUIMonoCommonTab toggleGuildMine;
        [ALHeader("数据在加载中时的显隐对象")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        [ALHeader("没有联盟时显示的内容")]
        public List<GameObject> listNoGuildShow;
        public List<GameObject> listNoGuildHide;
        
        
        public void setLoading(bool _loading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_loading ? listLoadingShow : listLoadingHide, true);
        }
        public void setHasGuild(bool _hasGuild)
        {
            ALUGUICommon.setGameObjEnable(listNoGuildShow, false);
            ALUGUICommon.setGameObjEnable(listNoGuildHide, false);
            ALUGUICommon.setGameObjEnable(_hasGuild ? listNoGuildHide : listNoGuildShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7416); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7416); } }
    }
}