using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 攻击结果弹窗
    /// </summary>
    public class GGUIMonoEveningDungeonAttackResult : _AALBasicUIWndMono
    {
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        [ALHeader("伤害数值")]
        public TextEx txtDamage;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5505); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5505);} }
    }
}