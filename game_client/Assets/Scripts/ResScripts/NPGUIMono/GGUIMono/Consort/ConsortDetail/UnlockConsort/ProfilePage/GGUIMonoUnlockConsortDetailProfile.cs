using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面简介page
    /// </summary>
    public class GGUIMonoUnlockConsortDetailProfile : _AALBasicUIWndMono
    {
        [ALHeader("简介子窗口")]
        public GGUISubMonoConsortProfile monoConsortProfile;
        
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1424); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1424);} }
    }
}