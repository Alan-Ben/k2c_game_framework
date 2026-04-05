using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 跑马灯窗口
    /// </summary>
    public class GGUIMonoMarquee : _AALBasicUIWndMono
    {

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4000); } }
    }
}