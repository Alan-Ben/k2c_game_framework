using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMuseumItemListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("品质图标")]
        public RawImage imgQualityIcon;
        [ALHeader("品质背景")]
        public RawImage imgQualityBg;
        [ALHeader("品质的 GO 显示")]
        public GGUISubMonoQualityShowGo monoQualityShowGo;
        [ALHeader("品质文本")]
        public Text txtQuality;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("三种状态的显示内容")]
        public List<GameObject> listNoGainShow;
        public List<GameObject> listNoActivateShow;
        public List<GameObject> listActivateShow;
        public List<MaskableGraphic> listNoGainGray;
        public List<MaskableGraphic> listNoActivateGray;
        [ALHeader("等级相关显示内容")] 
        public Text txtLevel;
        public List<GameObject> listCanLevelUpShow;
        public List<GameObject> listCannotLevelUpShow;


#if NP_GAME
        public void setState(bool _isGain, bool _isActivate, bool _canLevelUp)
        {
            ALUGUICommon.setGameObjEnable(listNoGainShow, false);
            ALUGUICommon.setGameObjEnable(listNoActivateShow, false);
            ALUGUICommon.setGameObjEnable(listActivateShow, false);
            ALUGUICommon.setGameObjEnable(listCanLevelUpShow, false);
            ALUGUICommon.setGameObjEnable(listCannotLevelUpShow, false);
            if (_isGain)
            {
                ALUGUICommon.setGameObjEnable(_isActivate ? listActivateShow : listNoActivateShow, true);
                GGameCommonInfo.disgrayImage(listNoGainGray);
                ALUGUICommon.setGameObjEnable(_canLevelUp ? listCanLevelUpShow : listCannotLevelUpShow, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(listNoGainShow, true);
                GGameCommonInfo.grayImage(listNoGainGray);
            }
            if (_isActivate)
                GGameCommonInfo.disgrayImage(listNoActivateGray);
            else
                GGameCommonInfo.grayImage(listNoActivateGray);
        }
#endif
    }
}