using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-礼包宝箱页面
    /// </summary>
    public class GGUIMonoGuildGiftBoxPage : GGUIMonoGuildBoxPageBase
    {
        [ALHeader("匿名发送")]
        public NPGGUIMonoCommonToggleEx toggleAnonymousSend;
        [ALHeader("可领取数量达到显示一键领取")]
        public int giftBoxShowCollectAllCount = 20;
        [ALHeader("可以一键领取显示Go")]
        public List<GameObject> canCollectAllShowList;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8705); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8705);} }
    }
}