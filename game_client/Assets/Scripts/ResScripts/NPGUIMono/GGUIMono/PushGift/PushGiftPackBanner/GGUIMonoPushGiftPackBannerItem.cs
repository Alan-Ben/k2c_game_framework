using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 推送礼包BannerItem
    /// </summary>
    public class GGUIMonoPushGiftPackBannerItem : _ANPGGUIMonoSelectContainerItem
    {
        [ALHeader("Banner图片")]
        public RawImage bannerImage;

        [ALHeader("礼包名称文本")]
        public List<TextEx> txtPushGiftNameList;
        [ALHeader("礼包名称TMP文本")]
        public List<TMP_Text> tmpPushGiftNameList;
    }
}