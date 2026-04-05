using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 尾刀记录窗口
    /// </summary>
    public class GGUIMonoEveningDungeonFinalAttackRecord : _AALBasicUIWndMono
    {
        [ALHeader("尾刀记录item列表")]
        public GGUIMonoEveningDungeonFinalAttackRecordContainer monoRecordContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5507); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5507);} }
    }
}