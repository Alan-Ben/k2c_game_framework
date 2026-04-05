using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟杂物委托
    /// </summary>
    public class GGUIMonoGuildEntrust : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("品质文本")]
        public Text txtQuality;
        [ALHeader("需要根据品质修改延时的UI列表")]
        public List<Graphic> changeColorList;

        [ALHeader("委托Banner图标")]
        public RawImage entrustBannerImg;
        
        [ALHeader("委托名")]
        public TextEx txtEntrustName;
        [ALHeader("委托描述")]
        public TextEx txtEntrustDesc;

        [ALHeader("每次捐赠获取的金币")] 
        public TextEx txtPerDealGainSilver;

        [ALHeader("委托完成进度")]
        public NPGGUIMonoProgress monoEntrustProgress;
        [ALHeader("委托奖励预览按钮")]
        public GameObject btnEntrustRewardPreview;

        [ALHeader("自动处理Toggle")]
        public NPGGUIMonoCommonToggleEx monoAutoDealToggle;

        [ALHeader("处理按钮")]
        public GameObject btnDeal;

        [ALHeader("处理事件的lazyCd")]
        public GGUIMonoCommonLazyCDCountResume monoDealLazyCd;

        [ALHeader("处理记录按钮")]
        public GameObject btnDealRecord;

        [ALHeader("处理杂物委托特效父节点")]
        public Transform entrustSfxParent;
        [ALHeader("处理杂物委托特效id")]
        public long entrustSfxId;
        [ALHeader("处理杂物委托特效同时存在最大数量")]
        public long maxSfxCount = 5;
        [ALHeader("暴击上浮center_tip id")]
        public long multipleCenterTipID;
        [ALHeader("暴击上浮提示展示父节点")]
        public Transform multipleCenterTipParent;
        [ALHeader("委托暴击特效id")]
        public long multipleSfxId;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4918); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4918); } }
    }
}