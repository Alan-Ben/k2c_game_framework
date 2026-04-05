using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星到达新节点确认弹窗
    /// </summary>
    public class GGUIMonoMarsNewStageConfirm : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("关闭按钮")]
        public GameObject btnClose2;
        [ALHeader("banner图片")]
        public RawImage imgBanner;
        [ALHeader("新节点描述")]
        public Text txtDesc;
        [ALHeader("新节点到达描述")]
        public Text txtArriveDesc;
        [ALHeader("确认按钮描述")]
        public Text txtConfirm;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("到达时间")]
        public Text txtArriveTime;
        [ALHeader("留言输入框")]
        public InputField msgInputField;
        [ALHeader("留言随机按钮")]
        public GameObject btnRandomMsg;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7005); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7005); } }
    }
}
