using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励步骤界面
    /// </summary>
    public class GGUIMonoActivityStepRewardStep : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("步骤列表")]
        public GGUIMonoActivityStepRewardStepGrid monoStepGrid;


        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6006); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6006); } }
    }
}
