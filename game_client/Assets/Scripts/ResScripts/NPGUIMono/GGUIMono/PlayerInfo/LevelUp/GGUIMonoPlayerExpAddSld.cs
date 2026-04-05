using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家经验增加进度条
    /// </summary>
    public class GGUIMonoPlayerExpAddSld : _AALBasicUIWndMono
    {
        [ALHeader("经验进度条")]
        public NPGGUIMonoProgress monoExpSld;
        [ALHeader("进度条开始变化延迟")]
        public float sldStartChgDelayTime;
        [ALHeader("进度条变化总时长")]
        public float sldChgTime;
        
        [ALHeader("有经验增加时显示go")]
        public List<GameObject> hasExpAddShowGo;
        [ALHeader("增加的经验")]
        public TextEx txtAddExp;

        [ALHeader("当等级提升时显示go")]
        public List<GameObject> onLevelUpShow;
        
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
    }
}