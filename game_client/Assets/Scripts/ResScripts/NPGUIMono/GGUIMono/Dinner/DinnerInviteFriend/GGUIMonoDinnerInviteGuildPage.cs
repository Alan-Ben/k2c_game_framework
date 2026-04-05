using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoDinnerInviteGuildPage : GGUIMonoDinnerInviteBasePage
    {
        [ALHeader("分享到联盟")]
        public GameObject btnShare;
        [ALHeader("下次可分享倒计时")]
        public Text txtShareCd;
        [ALHeader("可分享需要显隐的Go")]
        public List<GameObject> canShareShowGoList;
        public List<GameObject> canShareHideGoList;
        [ALHeader("未加入联盟需要显隐的Go")]
        public List<GameObject> notJoinGuildShowGoList;
        public List<GameObject> notJoinGuildHideGoList;
        [ALHeader("联盟可分享人数为0需要显隐的Go")]
        public List<GameObject> guildMemberIsZeroShowGoList;
        public List<GameObject> guildMemberIsZeroHideGoList;
        //资源加载路径
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2908); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2908); } }
    }
}