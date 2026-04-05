using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键邀约奖励窗口
    /// </summary>
    public class GGUIMonoConsortAkeyInviteReward : _AALBasicUIWndMono
    {
        [ALHeader("邀约次数")]
        public TextEx txtInviteCount;
        [ALHeader("邀约次数key")]
        public string txtInviteCountKey;
        
        [ALHeader("显示奖励item列表")]
        public GGUIMonoConsortInviteRewardItemContainer rewardItemContainer;
        
        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1412); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1412);} }
    }
}