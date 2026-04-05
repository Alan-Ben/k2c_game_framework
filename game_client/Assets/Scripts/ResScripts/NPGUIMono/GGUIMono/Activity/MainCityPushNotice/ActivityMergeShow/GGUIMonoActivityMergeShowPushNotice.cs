using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动合并展示推送弹窗
    /// </summary>
    public class GGUIMonoActivityMergeShowPushNotice : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;

        [ALHeader("活动ItemGrid")]
        public GGUIMonoActivityMergeShowActivityItemGrid monoActivityItemGrid;

        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8800); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8800); } }
    }
}