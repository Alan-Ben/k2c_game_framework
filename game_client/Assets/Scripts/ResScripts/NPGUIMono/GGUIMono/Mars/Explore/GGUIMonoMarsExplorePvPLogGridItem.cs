using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExplorePvPLogGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("战斗时间")]
        public Text txtTime;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("收获物品")]
        public NPGGUIMonoCommonItem monoItem;
        [ALHeader("各种情况下显示的内容")]
        public List<GameObject> listSuccessShow;
        public List<GameObject> listFailShow;
        [ALHeader("有无物品显示的内容")]
        public List<GameObject> listHasItemShow;
        public List<GameObject> listNoItemShow;
        [ALHeader("有无详情显示的内容")]
        public List<GameObject> listHasDetailShow;
        public List<GameObject> listNoDetailShow;
        
        
        public void setState(bool _isSuc, bool _hasItem, bool _hasDetail)
        {
            ALUGUICommon.setGameObjEnable(listHasItemShow, false);
            ALUGUICommon.setGameObjEnable(listNoItemShow, false);
            ALUGUICommon.setGameObjEnable(_hasItem ? listHasItemShow : listNoItemShow, true);
            
            ALUGUICommon.setGameObjEnable(listHasDetailShow, false);
            ALUGUICommon.setGameObjEnable(listNoDetailShow, false);
            ALUGUICommon.setGameObjEnable(_hasDetail ? listHasDetailShow : listNoDetailShow, true);
        
            ALUGUICommon.setGameObjEnable(listSuccessShow, false);
            ALUGUICommon.setGameObjEnable(listFailShow, false);
            ALUGUICommon.setGameObjEnable(_isSuc ? listSuccessShow : listFailShow, true);
        }
    }
}