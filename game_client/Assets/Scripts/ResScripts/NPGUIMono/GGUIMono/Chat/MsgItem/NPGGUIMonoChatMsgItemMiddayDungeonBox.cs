using ChatPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //午间活动宝箱分享
    public class NPGGUIMonoChatMsgItemMiddayDungeonBox : _ANPGGUIMonoPlayerChatMsgItem_JumpTo
    {
        [ALHeader("宝箱不同状态显示配置")]
        public List<NPCommonEnumStatInfo<EMiddayDungeonBoxState>> statInfos;
        [ALHeader("宝箱Banner")]
        public RawImage boxBanner;
        [ALHeader("宝箱可领取次数")]
        public Text txtCount;
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1369); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1369);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1368); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1368);} }
    }
}