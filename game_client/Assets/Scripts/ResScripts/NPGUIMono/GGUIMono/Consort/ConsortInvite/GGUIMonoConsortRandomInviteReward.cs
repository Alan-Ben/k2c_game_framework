using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子随机邀约奖励窗口
    /// </summary>
    public class GGUIMonoConsortRandomInviteReward : _AALBasicUIWndMono
    {
        [ALHeader("显示奖励item")]
        public GGUIMonoConsortInviteRewardItem rewardItem;

        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1411); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1411);} }
    }
}