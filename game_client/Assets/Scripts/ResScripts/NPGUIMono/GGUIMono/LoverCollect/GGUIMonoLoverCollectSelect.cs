using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoLoverCollectSelect : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("情人选项列表")]
        public List<GGUIMonoLoverCollectSelectItem> listLoverItems;
        [ALHeader("选择情人后的表演组ID")]
        public long performGroupId;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(9100); } }
        public static string objName { get { return UIResPathAssistant.getObjName(9100); } }
    }
}
