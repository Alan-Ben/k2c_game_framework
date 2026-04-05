using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 建筑详情窗口
    /// </summary>
    public class GGUIMonoMarsBuildingDetail : _AALBasicUIWndMono
    {
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("建筑名称文本")]
        public TextEx txtBuildingName;
     
        [ALHeader("建筑图标")]
        public RawImage buildingIcon;

        [ALHeader("建筑等级文本")]
        public TextEx txtLevel;
        
        [ALHeader("建筑描述")]
        public TextEx txtDesc;
        
        [ALHeader("属性显示列表")]
        public GGUIMonoMarsPropertyShowItemContainer monoPropertyShowContainer;

#if NP_GAME
        [ALHeader("建筑不同状态显示物体列表")]
        public List<NPCommonEnumStatMutexShowInfo<MarsBuildingInfo.StateType>> stateTypeShowList;
#endif
        
        [ALHeader("取消升级按钮")]
        public GameObject btnCancelUpgrade;
        
        [ALHeader("查看更多详情按钮")]
        public GameObject btnMoreDetail;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
    }
}