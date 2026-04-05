using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 切换入口妃子窗口
    /// </summary>
    public class GGUIMonoChangeEntranceConsort : _AALBasicUIWndMono
    {
        [ALHeader("有妃子时显示")]
        public List<GameObject> hasConsortShow;
        [ALHeader("没有妃子时显示")]
        public List<GameObject> noConsortShow;
        
        [ALHeader("选中妃子的详细信息")]
        public GGUISubMonoUnlockConsortDetailInfo monoSelectConsortDetailInfo;

        [ALHeader("妃子头像列表")]
        public GGUIMonoConsortIconItemContainer monoConsortHeadContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1428); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1428);} }
    }
}