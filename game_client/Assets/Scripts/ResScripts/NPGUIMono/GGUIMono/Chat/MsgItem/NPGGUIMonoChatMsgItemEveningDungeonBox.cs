using ChatPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 晚间副本宝箱分享聊天消息 Mono
    /// </summary>
    public class NPGGUIMonoChatMsgItemEveningDungeonBox : _ANPGGUIMonoPlayerChatMsgItem_JumpTo
    {
        [ALHeader("宝箱不同状态显示配置")]
        public List<NPCommonEnumStatInfo<EMiddayDungeonBoxState>> statInfos;
        [ALHeader("宝箱Banner")]
        public RawImage boxBanner;
        [ALHeader("宝箱可领取次数")]
        public Text txtCount;
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1376); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1376);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1375); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1375);} }
    }
}
