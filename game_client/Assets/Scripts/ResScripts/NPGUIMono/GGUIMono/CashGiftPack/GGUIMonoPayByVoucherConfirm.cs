using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 使用代金券购买确认弹窗
    /// </summary>
    public class GGUIMonoPayByVoucherConfirm : _AALBasicUIWndMono
    {
        [ALHeader("代金券购买按钮")]
        public GameObject btnPayByVoucher;
        [ALHeader("现金购买按钮")] 
        public GameObject btnPayByCash;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("代金券道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("现金价格")]
        public Text txtPrice;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6706); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6706); } }
    }
}