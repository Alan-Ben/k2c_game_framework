using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 居民补充窗口
    /// </summary>
    public class GGUIMonoMarsResidentReplenish : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        
        [ALHeader("补充消耗道具")]
        public NPGGUIMonoCommonItem itemConsume;
        
        [ALHeader("补充按钮")]
        public GameObject btnReplenish;

        [ALHeader("居民数量达到上限时显示的物体列表")]
        public List<GameObject> residentNumReachLimitShow;
        [ALHeader("居民数量达到上限时隐藏的物体列表")]
        public List<GameObject> residentNumReachLimitHide;
        
        [ALHeader("可获得居民数量描述文本")]
        public TextEx txtCanGetResidentCountDesc;
        [ALHeader("可获得居民数量描述文本key(一个参数, 居民居住上限-已拥有居民数量)")]
        public string txtCanGetResidentCountDescStr;
        
        [ALHeader("扩展居民上限按钮")]
        public GameObject btnExpandResidentLimit;
        
        [ALHeader("剩余可补充次数文本")]
        public TextEx txtLeftCount;// 今日剩余请求补给次数: {0}
        
        [ALHeader("居民补充状态显示信息")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>> replenishStateShowList;
        
        //资源加载路径
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7201); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7201);} }
    }
}