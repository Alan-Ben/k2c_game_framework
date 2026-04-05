using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 可为空的大臣头像item
    /// </summary>
    public class GGUIMonoHeroIconNullableItem : _AALBasicUIWndMono
    {
        [ALHeader("骑士信息")] 
        public GGUIMonoHeroIconItem monoHeroInfo;

        [ALHeader("有数据时显示")]
        public List<GameObject> hasInfoShow;
        [ALHeader("没有数据时显示")]
        public List<GameObject> noInfoShow;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}