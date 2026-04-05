using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友添加分组
    /// </summary>
    public class GGUIMonoFriendAdd:_AALBasicUIWndMono
    {
        [ALHeader("关闭按钮按钮")]
        public GameObject btnClose;
        
        [ALHeader("好友添加tab")]
        public NPGGUIMonoCommonToggleEx addFriendTog;
        [ALHeader("好友申请tab")]
        public NPGGUIMonoCommonToggleEx requestFriendTog;
        [ALHeader("页面加载的父节点")]
        public Transform pageParent;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1316); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1316);} }
    }
}