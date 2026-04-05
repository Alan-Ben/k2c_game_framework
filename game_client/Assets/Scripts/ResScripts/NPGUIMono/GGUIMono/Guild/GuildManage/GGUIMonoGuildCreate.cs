using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟创建界面
    /// </summary>
    public class GGUIMonoGuildCreate : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("旗帜图标")]
        public RawImage imgFlag;
        [ALHeader("更换旗帜按钮")]
        public GameObject btnChgFlag;
        [ALHeader("创建联盟按钮")]
        public GameObject btnCreate;
        [ALHeader("输入联盟名称")]
        public InputField inputGuildName;
        [ALHeader("输入名称字数")]
        public Text txtInputNameCount;
        [ALHeader("输入名称实时提示")]
        public Text txtInputNameTip;
        [ALHeader("输入联盟简称")]
        public InputField inputAbbreviation;
        [ALHeader("输入简称字数")]
        public Text txtInputAbbreviationCount;
        [ALHeader("输入简称实时提示")]
        public Text txtInputAbbreviationTip;
        [ALHeader("输入宣言")]
        public InputField inputDeclaration;
        [ALHeader("输入宣言字数")]
        public Text txtInputDeclarationCount;
        [ALHeader("创建联盟消耗")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("允许其他玩家随机加入")]
        public NPGGUIMonoCommonToggleEx allowOthersJoinRandomlyToggle;
        [ALHeader("不能创建联盟时需要置灰的列表")]
        public List<MaskableGraphic> grayList;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4901); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4901); } }
    }
}
