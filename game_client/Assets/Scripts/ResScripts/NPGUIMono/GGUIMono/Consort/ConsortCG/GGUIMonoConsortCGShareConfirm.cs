using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 家人CG分享确认弹窗
    /// </summary>
    public class GGUIMonoConsortCGShareConfirm : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("描述文本")]
        public Text txtDesc;
        [ALHeader("CG名称")]
        public Text txtCGName;
        [ALHeader("家人名称")]
        public Text txtConsortName;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("聊天频道列表")]
        public List<GGUIMonoConsortCGShareChannelItem> monoChannelItemList;

        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1430); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1430);} }
    }
}