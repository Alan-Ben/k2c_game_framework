using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 最后一击结果弹窗
    /// </summary>
    public class GGUIMonoEveningDungeonFinalAttackResult : _AALBasicUIWndMono
    {
        [ALHeader("最后一击玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayerInfo;
        
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5501); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5501);} }
    }
}