using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHomeInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("能源消耗文本")]
        public Text txtEnergyCost;
        [ALHeader("氧气产量文本")
        ,ALInfo("先做文本的，表现确定再改")]
        public Text txtOxygenYield;
        [ALHeader("开关")]
        public NPGGUIMonoCommonToggleEx monoSwitchOn;
        [ALHeader("最大功率开关")]
        public NPGGUIMonoCommonToggleEx monoSwitchOverdrive;
        [ALHeader("三种开关状态下显示的内容")]
        public List<GameObject> listOffShow;
        public List<GameObject> listOnShow;
        public List<GameObject> listOverdriveShow;
        [ALHeader("产量速度计")]
        public GGUIMonoMarsBuildingHomeYieldSpeedometer monoYieldSpeedometer;
        [ALHeader("氧气显示部分")]
        public List<GGUIMainMarsOxygenShow> listOxygenShow;
        [ALHeader("总人口")]
        public Text txtPeopleCount;
        
        
        public void setSwitchState(bool _isOn, bool _isOverdrive)
        {
            ALUGUICommon.setGameObjEnable(listOffShow, false);
            ALUGUICommon.setGameObjEnable(listOnShow, false);
            ALUGUICommon.setGameObjEnable(listOverdriveShow, false);
            
            if (!_isOn)
                ALUGUICommon.setGameObjEnable(listOffShow, true);
            else
                ALUGUICommon.setGameObjEnable(_isOverdrive ? listOverdriveShow : listOnShow, true);
        }
        public void setOxygenValue(long _value)
        {
            if (listOxygenShow is not { Count: > 0 })
                return;
        
            foreach (GGUIMainMarsOxygenShow data in listOxygenShow)
            {
                if (data == null)
                    continue;
            
                ALUGUICommon.setGameObjEnable(data.listShow, false);
            }
        
            foreach (GGUIMainMarsOxygenShow data in listOxygenShow)
            {
                if (data?.range == null || !data.range.inRange(_value))
                    continue;
            
                ALUGUICommon.setGameObjEnable(data.listShow, true);
            }
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7116); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7116); } }
    }
}