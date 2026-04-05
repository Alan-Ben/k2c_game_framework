using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 星辉等级提升弹窗
    /// </summary>
    public class GGUIMonoConsortHaloLvlUp : _AALBasicUIWndMono
    {
        [ALHeader("是解锁弹窗时显示")]
        public List<GameObject> unlockShow;
        [ALHeader("是解锁弹窗时隐藏")]
        public List<GameObject> unlockHide;
        
        [ALHeader("星辉等级变更子窗口")]
        public GGUISubMonoConsortHaloLvlChg monoHaloLvlChg;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1415); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1415);} }
    }
}