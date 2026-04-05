using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 物品自动转化窗口mono
    /// </summary>
    public class NPGGUIMonoCommonAutoConversion:_AALBasicUIWndMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("转化前的item")]
        public NPGGUIMonoCommonItem sourceItem;
        [ALHeader("转化后的item")]
        public NPGGUIMonoCommonItem targetItem;
        [ALHeader("转化描述")]
        public TextEx textDesc;
        [ALHeader("转化特效父节点")]
        public Transform sfxParent;
        [ALHeader("转化特效Id")]
        public long sfxId;


        /************
        * 资源加载路径
        */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_MUSEUM_FARGMENT_AUTO_COMPOSE); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_MUSEUM_FARGMENT_AUTO_COMPOSE);} }
    }
}