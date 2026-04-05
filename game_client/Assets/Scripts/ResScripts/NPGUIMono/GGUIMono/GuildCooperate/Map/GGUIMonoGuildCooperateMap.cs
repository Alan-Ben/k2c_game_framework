using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作地图预览弹窗
    /// </summary>
    public class GGUIMonoGuildCooperateMap : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("区域列表，区域ID按配置顺序自动设置")]
        public List<GGUIMonoGuildCooperateMapAreaItem> monoAreaItemList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4933); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4933); } }
    }
}