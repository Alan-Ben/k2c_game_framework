using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// 标本转化窗口
    /// </summary>
    public class GGUIMonoTreasureHuntSpecimenConvert : _AALBasicUIWndMono
    {
        [ALHeader("转化的item列表")]
        public GGUIMonoTreasureHuntSpecimenConvertItemGrid monoSpecimenConvertItemGrid;

        [ALHeader("普通矿石转化数量描述")]
        public TextEx txtNormalConvertNum;
        [ALHeader("普通矿石转化数量描述Key")]
        public string txtNormalConvertNumKey;
        
        [ALHeader("高级矿石转化数量描述")]
        public TextEx txtAdvancedConvertNum;
        [ALHeader("高级矿石转化数量描述key")]
        public string txtAdvancedConvertNumKey;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6806); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6806); } }

    }
}