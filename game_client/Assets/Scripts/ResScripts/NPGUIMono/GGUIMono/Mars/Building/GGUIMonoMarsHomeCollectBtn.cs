using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsHomeCollectBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("获取能量按钮")]
        public GameObject btnCollect;

        [ALHeader("达到需要显隐红点的时候显隐的Go")]
        public List<GameObject> listRedShow;
        public List<GameObject> listRedHide;
    }
}