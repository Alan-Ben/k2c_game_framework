using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{    
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoTowerLogItem : _TALUGUIMonoGridItem
    {
	    [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon playerInfo;
        [ALHeader("描述文本")]
        public TextEx txtDesc;
        [ALHeader("时间文本")]
        public TextEx txtTime;
        [ALHeader("成功显示的go")]
        public List<GameObject> sucShowGos;	
        [ALHeader("失败显示的go")]
        public List<GameObject> failShowGos;
    }
}
