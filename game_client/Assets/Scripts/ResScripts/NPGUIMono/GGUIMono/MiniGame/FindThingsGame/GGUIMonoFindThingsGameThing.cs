using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class FindThingsGameThingInfo : _IFindThingsGameThingInfo
    {
        [SerializeField]
        [ALHeader("物品id")]
        private long thingId;//物品id
        
        [SerializeField]
        [ALHeader("物品名称")]
        private string thingName;//物品名称

        public long getThingId()
        {
            return thingId;
        }

        public string getThingName()
        {
            return thingName;
        }
    }
    
    /// <summary>
    /// 找东西小游戏物品
    /// </summary>
    public class GGUIMonoFindThingsGameThing : _AALBasicUIWndMono
    {
        [ALHeader("物品信息")]
        public FindThingsGameThingInfo thingInfo;

        [ALHeader("点击物体")]
        public GameObject btnClick;
        
        [ALHeader("未找到时显示物体列表")]
        public List<GameObject> notFindShowGoList;
        
        [ALHeader("找到时显示物体列表")]
        public List<GameObject> findShowGoList;

        [ALHeader("动画")]
        public Animation thingAnimation;
        
        [ALHeader("找到时播放的动画名")]
        public string findAnimName;

        [ALHeader("开始飞行的延迟时间")]
        public float startFlyDelay;
        [ALHeader("飞行时间")]
        public float flyTime;
    }
}